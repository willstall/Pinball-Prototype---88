using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Linq;
using System.Reflection;

using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;

using System;

namespace Reflection
{
	[System.Serializable]
	public class ComponentMethod  {

		[XmlIgnore]
		public GameObject target;
		public int targetInstanceId;
		
		public string methodName;
		public MethodParameterType type;
		public string fullyQualifiedTypeName;
		
		public float floatParameter;
		public int intParameter;
		public string stringParameter;
		public bool boolParameter;

		[XmlIgnore]
		public UnityEngine.Object objectReferenceParameter;
		public int objectReferenceInstanceId;
		
		public void SaveReferences()
		{
			if( target != null )
				targetInstanceId = target.GetInstanceID();
			if( objectReferenceParameter != null )
				objectReferenceInstanceId = objectReferenceParameter.GetInstanceID();
		}
		
		public void LoadReferences()
		{
			Debug.Log( "UPDATING REFERENCE YALL: " + targetInstanceId );
		
			target = GameObjectReference.GetObject( targetInstanceId ) as GameObject;
			objectReferenceParameter = GameObjectReference.GetObject( objectReferenceInstanceId ) as GameObject;
		}
			
		public void SendMessage()
		{
			if( methodName != "" )
				target.gameObject.SendMessage(methodName, GetParameter());
		}

		object GetParameter()
		{
			object result = null;
			switch( type )
			{
				case MethodParameterType.String:
					result = stringParameter;
				break;
				
				case MethodParameterType.Int:
					result = intParameter;
				break;
				
				case MethodParameterType.Boolean:
					result = boolParameter;
				break;
				
				case MethodParameterType.Float:
					result = floatParameter;
				break;
				
				case MethodParameterType.Object:
					result = objectReferenceParameter;
				break;
			
			}
			
			return result;
		}
		
		public void InitializeFromMethodInfo( MethodInfo method )
		{

			ParameterInfo[] parameters = method.GetParameters();
			if( parameters.Length == 0 )
			{
				methodName = method.Name;
				type = MethodParameterType.None;
			} else if( parameters.Length == 1 ){
				switch( parameters[0].ParameterType.Name )
				{
					case "String":
						methodName = method.Name;
						type = MethodParameterType.String;
					break;
		
					case "Int32":
						methodName = method.Name;
						type = MethodParameterType.Int;
					break;
		
					case "Boolean":
						methodName = method.Name;
						type = MethodParameterType.Boolean;
					break;
		
					case "Single":
						methodName = method.Name;
						type = MethodParameterType.Float;
					break;
				}
				
				if( parameters[0].ParameterType.IsSubclassOf( typeof(Component) ) ||
					parameters[0].ParameterType.IsSubclassOf( typeof(GameObject) ))
				{
					methodName = method.Name;
					type = MethodParameterType.Object;
					fullyQualifiedTypeName = parameters[0].ParameterType.AssemblyQualifiedName;	
				}
			}
		
		}
		
		public Type ParameterType
		{
			get
			{
				return Type.GetType( fullyQualifiedTypeName );
			}
		}
		
		public static List<MethodSignature> GetMethodSignatures( GameObject gameObject )
		{
			Component[] components = gameObject.GetComponents<Component>();
			List<MethodSignature> result = new List<MethodSignature>();
			foreach( Component component in components )
			{
				System.Type type = component.GetType();
				if( Assembly.GetAssembly( type ).GetName().Name.IndexOf("Assembly") != 0 ) continue;
				MethodInfo[] methods = type.GetMethods().Where(x => x.DeclaringType == type).ToArray();
				foreach( MethodInfo method in methods )
				{
					ParameterInfo[] parameters = method.GetParameters();
					if( parameters.Length == 0 )
					{
						result.Add( new MethodSignature( method.Name, "", MethodParameterType.None ) );
					} else if( parameters.Length == 1 ){
						switch( parameters[0].ParameterType.Name )
						{
							case "String":
								result.Add( new MethodSignature( method.Name, parameters[0].Name, MethodParameterType.String ) );
							break;
				
							case "Int32":
								result.Add( new MethodSignature( method.Name, parameters[0].Name, MethodParameterType.Int ) );
							break;
				
							case "Boolean":
								result.Add( new MethodSignature( method.Name, parameters[0].Name, MethodParameterType.Boolean ) );
							break;
				
							case "Single":
								result.Add( new MethodSignature( method.Name, parameters[0].Name, MethodParameterType.Float ) );
							break;
						}
						
						if( parameters[0].ParameterType.IsSubclassOf( typeof(Component) ) ||
							parameters[0].ParameterType.IsSubclassOf( typeof(GameObject) ))
						{
							MethodSignature sig = new MethodSignature( method.Name, parameters[0].Name, MethodParameterType.Object );
							sig.parameterType = parameters[0].ParameterType;
							result.Add( sig );
							
						}
					}
				}
			}
			
			return result;
		}

	}

	public enum MethodParameterType
	{
		None,
		String,
		Int,
		Boolean,
		Float,
		Object
	}


	public struct MethodSignature
	{
		public string name;
		public string paramName;
		public MethodParameterType type;
		public Type parameterType;
		
		public MethodSignature( string name, string paramName, MethodParameterType type, Type parameterType = null )
		{
			this.name = name;
			this.paramName = paramName;
			this.type = type;
			this.parameterType = parameterType;
		}
		
		public override string ToString()
		{
			string p = "";
			switch( type )
			{
				case MethodParameterType.String:
					p = "string " + paramName;
				break;
				
				case MethodParameterType.Int:
					p = "int " + paramName;
				break;
				
				case MethodParameterType.Boolean:
					p = "bool " + paramName;
				break;
				
				case MethodParameterType.Float:
					p = "float " + paramName;
				break;
				
				case MethodParameterType.Object:
					p = parameterType.Name + " " + paramName;
				break;
			}
			
			return name + "(" + p + ")";
		}
	}
}