using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

namespace Reflection
{
	[CustomPropertyDrawer( typeof(ComponentProperty) )]
	public class ComponentPropertyDrawer : PropertyDrawer {

		public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
		{

			
			SerializedObject obj = property.serializedObject;
			Component component = obj.targetObject as Component;
			if( component == null )
			{
				string shortName = property.FindPropertyRelative("componentType").stringValue;
				position = EditorGUI.PrefixLabel( position, GUIUtility.GetControlID( FocusType.Passive ), new GUIContent( shortName + "." + property.FindPropertyRelative("propertyName").stringValue ) );
				EditorGUI.SelectableLabel( position, property.FindPropertyRelative("xml").stringValue );
				return;
			}
			
			position = EditorGUI.PrefixLabel( position, GUIUtility.GetControlID( FocusType.Passive ), label );
			bool state = GUI.enabled;

			int indentLevel = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
			
			
			Rect componentRect = new Rect( position.x, position.y, position.width / 2 - 3, position.height );
			Rect memberRect = new Rect( position.x + position.width / 2 + 5, position.y, position.width / 2 - 2 , position.height );


			
			if( obj.isEditingMultipleObjects )
			{
				string[] options = new string[]{"--"};
				bool enabled = GUI.enabled;
				GUI.enabled = false;

				EditorGUI.Popup(componentRect, 0, options);
				EditorGUI.Popup(memberRect, 0, options);
				
				GUI.enabled = enabled;

				return;
			}

			Type filterType = null;
			FieldInfo fieldInfo = property.serializedObject.targetObject.GetType().GetField( property.name );
			if( fieldInfo != null )
			{
				PropertyTypeAttribute attribute = (PropertyTypeAttribute)System.Attribute.GetCustomAttribute( fieldInfo, typeof(PropertyTypeAttribute) );			
				if( attribute != null ) filterType = attribute.Type;
			}
			
			SerializedProperty componentProp = property.FindPropertyRelative("component");
			Component[] components = ComponentProperty.GetComponents( component, filterType );

			int componentIndex = System.Array.IndexOf( components, componentProp.objectReferenceValue );

			List<string> componentNames = components.Select( x => x.GetType().Name ).ToList();
			int[] componentNameCounts = new int[ componentNames.Count ];

			for(int i = 0; i < componentNames.Count; i++)
			{
				if( componentNameCounts[i] > 0 ) continue;
				int j = 1;

				for( int k = i + 1; k < componentNames.Count; k++ )
				{
					if( componentNames[k] == componentNames[i] )
					{
						j++;
						componentNameCounts[k] = j;
						componentNameCounts[i] = 1;
					}
				}
			}

			componentNames = componentNames.Select( (x, i) => componentNameCounts[i] > 0 ? x + " (" +componentNameCounts[i] + ")" : x ).ToList();

			componentNames.Insert(0, "Not Selected");	
			
			componentIndex = EditorGUI.Popup(componentRect, componentIndex + 1, componentNames.ToArray() ) - 1;
			SerializedProperty typeProp = property.FindPropertyRelative("type");

			
			
			if( componentIndex >= 0 )
			{
				componentProp.objectReferenceValue = components[ componentIndex ];
				SerializedProperty memberProp = property.FindPropertyRelative("propertyName");
				List<string> fieldNames = ComponentProperty.GetFieldNames( componentProp.objectReferenceValue as Component, filterType );
				List<string> methodNames = ComponentProperty.GetMethodNames( componentProp.objectReferenceValue as Component, filterType );
				
				int enumVal = 0;
				if( methodNames.Contains( memberProp.stringValue ) )
				{
					enumVal = (int)ComponentPropertyType.Method;
				} else {
					enumVal = (int)ComponentPropertyType.Field;				
				}
				
				List<string> allNames = fieldNames.ToList();
				allNames.AddRange(methodNames);
				allNames = allNames.OrderBy( x => x ).ToList();
				allNames.Insert(0, "Not Selected");
				string[] names = allNames.ToArray();
				
				int memberIndex = System.Array.IndexOf( names, memberProp.stringValue );
				memberIndex = EditorGUI.Popup(memberRect, memberIndex, names);
				if( memberIndex >= 0 )
				{
					memberProp.stringValue = names[ memberIndex ];
					typeProp.intValue = enumVal;
					
				} else {
					memberProp.stringValue = "";
				}
			
			} else {
				componentProp.objectReferenceValue = null;
				GUI.enabled = componentIndex >= 0 ? state : false;
				EditorGUI.Popup(memberRect, 0, new string[]{"Not Selected"});
			}
			

			
			EditorGUI.indentLevel = indentLevel;
			GUI.enabled = state;
		}
		
		public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
		{
			float h = base.GetPropertyHeight( property, label );
			Component component = property.serializedObject.targetObject as Component;
			if( component == null )
			{
				string[] arr = property.FindPropertyRelative("xml").stringValue.Split("\n"[0]);
				return arr.Length * h;
			} else {
				return h;
			}
		}
	}
}