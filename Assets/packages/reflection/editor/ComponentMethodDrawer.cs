using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

namespace Reflection
{
	[CustomPropertyDrawer( typeof(ComponentMethod) )]
	public class ComponentMethodDrawer : PropertyDrawer {

		int padding = 2;
		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{
			position.height -= 2 * padding;
			position.y += padding;
			ExpandRect( ref position, padding );
			GUI.Box( position, GUIContent.none );
			ExpandRect( ref position, -padding );
			position.height /= (HasParameter(property) ? 3 : 2);
			position = EditorGUI.PrefixLabel( position, GUIUtility.GetControlID( FocusType.Passive ), label );

			GameObject gameObject = property.FindPropertyRelative("target").objectReferenceValue as GameObject;
			if( gameObject == null )
			{
				SerializedObject obj = property.serializedObject;
				Component component = obj.targetObject as Component;
				if( component == null )
				{
					EditorGUI.LabelField( position, "Not in a Component" );
					return;
				} else {
					gameObject = component.gameObject;
					property.FindPropertyRelative("target").objectReferenceValue = gameObject;
					
				}
			}
			
			EditorGUI.PropertyField( position, property.FindPropertyRelative("target"), GUIContent.none );
	//		property.FindPropertyRelative("targetInstanceId").intValue = (property.FindPropertyRelative("target").objectReferenceValue as GameObject).GetInstanceID();
			
			position.y += position.height;
			bool state = GUI.enabled;
			
			List<MethodSignature> signatures = ComponentMethod.GetMethodSignatures( gameObject );
			List<string> names = signatures.Select( x => x.name ).ToList();
			List<string> displayNames = signatures.Select( x => x.ToString() ).ToList();
			names.Insert(0, "Not Selected");
			displayNames.Insert(0, "Not Selected");
			
			string name = property.FindPropertyRelative("methodName").stringValue;
			int index = names.IndexOf( name );
			if( index < 0 ) index = 0;
			index = EditorGUI.Popup( position, index, displayNames.ToArray() );
			if( index > 0 )
			{
				property.FindPropertyRelative("methodName").stringValue = names[index];
				property.FindPropertyRelative("type").enumValueIndex = (int)signatures[index-1].type;
			} else {
				property.FindPropertyRelative("methodName").stringValue = "";
				property.FindPropertyRelative("type").enumValueIndex = (int)MethodParameterType.None;
				return;
			}
			
			position.y += position.height;
			
			MethodSignature sig = signatures[index-1];
			SerializedProperty valueProp = null;
			switch(sig.type)
			{
				case MethodParameterType.String:
					valueProp = property.FindPropertyRelative("stringParameter");
				break;
				
				case MethodParameterType.Int:
					valueProp = property.FindPropertyRelative("intParameter");
				break;
				
				case MethodParameterType.Boolean:
					valueProp = property.FindPropertyRelative("boolParameter");
				break;
				
				case MethodParameterType.Float:
					valueProp = property.FindPropertyRelative("floatParameter");
				break;
			}
			
			if( sig.type == MethodParameterType.Object )
			{
				property.FindPropertyRelative("objectReferenceParameter").objectReferenceValue =
				EditorGUI.ObjectField(position, property.FindPropertyRelative("objectReferenceParameter").objectReferenceValue,
					sig.parameterType, !EditorUtility.IsPersistent( property.serializedObject.targetObject ));
			} else if(sig.type != MethodParameterType.None) {
				EditorGUI.PropertyField(position, valueProp, GUIContent.none);
			}
			
			GUI.enabled = state;
			
		}
		
		
		public void ExpandRect(ref Rect rect, float amount )
		{
			rect.xMax += amount;
			rect.yMax += amount;
			rect.xMin -= amount;
			rect.yMin -= amount;
		}
		
		public bool HasParameter( SerializedProperty property )
		{
			MethodParameterType type = (MethodParameterType)property.FindPropertyRelative("type").enumValueIndex;
			return type != MethodParameterType.None;
		}
		
		public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
		{
			return (HasParameter(property) ? 3 : 2) * base.GetPropertyHeight( property, label ) + 2 * padding;
		}
		

		
	}
}