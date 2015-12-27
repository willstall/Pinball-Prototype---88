using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Xml.Serialization;
using System.Xml;
using System;
using System.IO;
using System.Text;

namespace Reflection
{
	[System.Serializable]
	public class ComponentProperty {

		[XmlIgnore]
		public Component component;

		[XmlAttribute("type")]
		public ComponentPropertyType type;

		[XmlAttribute("componentType")]
		public string componentType;

		[XmlAttribute("componentIndex")]
		public int componentIndex;

		[XmlAttribute("propertyName")]
		public string propertyName;

		public string xml;

		private static XmlWriter writer;

		public ComponentProperty(){}
		
		public ComponentProperty( Component component, FieldInfo field )
		{
			this.component = component;
			this.propertyName = field.Name;
			this.type = ComponentPropertyType.Field;
		}
		
		public ComponentProperty( Component component, string methodName )
		{
			this.component = component;
			this.propertyName = methodName;
			this.type = ComponentPropertyType.Method;
		}

		public bool CacheXml()
		{
	//		Debug.Log( PropertyType );
			
			object value = Get();
		
			bool result = false;
			
			if( value == null )
			{
				xml = "";
				return result;
			}
			
			if( value is ComponentProperty ) 
				result = (value as ComponentProperty).CacheXml();
				
			if( value is ComponentMethod )
			{
				(value as ComponentMethod).SaveReferences();
				result = true;
			}

			try
			{
				StringWriter writer = new StringWriter();
				XmlSerializer serializer = new XmlSerializer( PropertyType );
				serializer.Serialize(writer, value);
				xml = writer.ToString();
				result = true;
			} catch( InvalidOperationException e ) {
				Debug.LogWarning("Invalid Operation Exception " + e.Message);
				//Debug.Log( "Couldn't serialize " + PropertyType );
			}
			
			componentType = component.GetType().Name;
			var allComponents = component.gameObject.GetComponents( component.GetType() );

			componentIndex = System.Array.IndexOf( allComponents, component );
			
			return result;
		}
		
		public void LoadFromXml( GameObject gameObject )
		{
			var c = gameObject.GetComponent( componentType );
			if ( c == null ) return;
			
			var type = c.GetType();
			var allComponents = gameObject.GetComponents( type );

			component = allComponents[ componentIndex ];
			
			if( xml == null || PropertyType == null ) return;
			
			TextReader reader = new StringReader( xml );
			XmlSerializer serializer = new XmlSerializer(PropertyType);
			object obj = serializer.Deserialize( reader );
			
			if( obj is ComponentProperty )
			{
				ComponentProperty prop = obj as ComponentProperty;
				prop.LoadFromXml( gameObject );
			}
			
			if( obj is ComponentMethod )
			{
				GameObjectReference.AddCallback( (obj as ComponentMethod).LoadReferences );
			}
			
			Set( obj );
		}
		
		
		
		public void Set( object value )
		{
			if( type == ComponentPropertyType.Field )
			{
				if( field != null )
					field.SetValue( component, value );
			} else {
				if( setter != null )
					setter.Invoke( component, new object[]{value as object} );
			}
		}
		
		public object Get()
		{
			if( type == ComponentPropertyType.Field )
			{
				if( field != null )
					return field.GetValue( component );
				else
					return null;
			} else {
				if( getter != null )
					return getter.Invoke( component, null );
				else
					return null;
			}
		}
		
		public Type PropertyType
		{
			get
			{
				if( type == ComponentPropertyType.Field )
				{
					if( field == null ) return null;
					else return field.FieldType;
				} else {
					if( getter == null ) return null;
					else return getter.ReturnType;
				}
			}
		}
		
		public FieldInfo field
		{
			get
			{
				if( component == null ) return null;
				Type type = component.GetType();
				return type.GetField( propertyName );
			}
		}
		
		public MethodInfo getter
		{
			get
			{
				if( component == null ) return null;
				Type type = component.GetType();
				return type.GetMethod( "get_" + propertyName );
			}
		}
		
		public MethodInfo setter
		{
			get
			{
				if( component == null ) return null;
				Type type = component.GetType();
				return type.GetMethod( "set_" + propertyName );
			}
		}
		
		public bool IsReady
		{
			get
			{
				if( type == ComponentPropertyType.Field )
				{
					return ( field != null );
				} else {
					return ( getter != null );
				}
			}
		}
		
		public override bool Equals( object o )
		{
			return this == o;
		}


		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		
		public static bool operator ==(ComponentProperty prop1, object prop2)
		{
			if( prop2 == null ) return false;
			return prop2.Equals( prop1.Get() );
		}
		
		public static bool operator !=(ComponentProperty prop1, object prop2)
		{
			if( prop2 == null ) return false;
			return !prop2.Equals( prop1.Get() );
		}
		
		
		public static bool operator <(ComponentProperty prop1, object prop2)
		{
			if( prop2 == null ) return false;
			IComparable p = (IComparable)prop2;
			try{ 
				if(p != null) return p.CompareTo( prop1.Get() ) > 0;
				else return false;
			} catch (ArgumentException e) {
				Debug.LogWarning("Argument Exception " + e.Message);
				return false;
			}
		}
		
		public static bool operator >(ComponentProperty prop1, object prop2)
		{
			if( prop2 == null ) return false;
			IComparable p = (IComparable)prop2;
			try{ 
				if(p != null) return p.CompareTo( prop1.Get() ) < 0;
				else return false;
			} catch (ArgumentException e ) {
				Debug.LogWarning("Argument Exception " + e.Message);
				return false;
			}
		}
		
		public static bool operator <=(ComponentProperty prop1, object prop2)
		{
			if( prop2 == null ) return false;
			IComparable p = (IComparable)prop2;
			try{ 
				if(p != null) return p.CompareTo( prop1.Get() ) >= 0;
				else return false;
			} catch (ArgumentException e ) {
				Debug.LogWarning("Argument Exception " + e.Message);
				return false;
			}
		}
		
		public static bool operator >=(ComponentProperty prop1, object prop2)
		{
			if( prop2 == null ) return false;
			IComparable p = (IComparable)prop2;
			try{ 
				if(p != null) return p.CompareTo( prop1.Get() ) <= 0;
				else return false;
			} catch (ArgumentException e ) {
				Debug.LogWarning("Argument Exception " + e.Message);
				return false;
			}
		}
		
		
		
		
		
		public static Component[] GetComponents( Component component, Type filter = null )
		{
			if( component == null ) return new Component[0];
			
			GameObject go = component.gameObject;
		    Component[] result =  go.GetComponents<Component>().Where( x => x != component).ToArray();
		    if( filter ==  null ) return result;
		    else {
		    	return result.Where( x => GetFieldNames(x, filter).Count > 0 || GetMethodNames(x, filter).Count > 0 ).ToArray();
		    }
		}
		
		public static List<string> GetFieldNames( Component component, Type filter = null )
		{
			if( component == null ) return new List<string>();
			Type type = component.GetType();
			return type.GetFields().Where( x => filter == null ? true : x.FieldType == filter ).Select( x => x.Name ).ToList();
		}
		
		public static List<string> GetMethodNames( Component component, Type filter = null)
		{
			if( component == null ) return new List<string>();
			Type type = component.GetType();
			string[] getters = type.GetMethods().Where( x => x.Name.IndexOf( "get_" ) >= 0 ).Where( x => filter == null ? true : x.ReturnType == filter ).Select( x => x.Name.Substring(4) ).ToArray();
			string[] setters = type.GetMethods().Where( x => x.Name.IndexOf( "set_" ) >= 0 ).Select( x => x.Name.Substring(4) ).ToArray();
			return getters.Where( x => System.Array.IndexOf( setters, x ) >= 0 ).ToList();
		}
		
	}

	public enum ComponentPropertyType
	{
		Field,
		Method
	}

	public class PropertyTypeAttribute:PropertyAttribute
	{
		public System.Type Type;

		public PropertyTypeAttribute(System.Type type)
		{
			this.Type = type;
		}
	}
}