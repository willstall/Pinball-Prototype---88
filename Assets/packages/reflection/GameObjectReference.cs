using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Reflection
{
	public class GameObjectReference {

		private static Dictionary<int, Object> instanceDatabase = new Dictionary<int, Object>();
		
		public delegate void UpdateReferenceFunction();
		private static event UpdateReferenceFunction DoUpdateReferences;
		
		public static void AddUpdatedReference( int id, Object go )
		{
			instanceDatabase[id] = go;
		}
		
		public static Object GetObject( int id )
		{
			Object obj = null;
			instanceDatabase.TryGetValue( id, out obj);
			return obj;
		}
		
		public static void AddCallback( UpdateReferenceFunction func )
		{
			DoUpdateReferences += func;
		}
		
		public static void UpdateReferences()
		{
			if( DoUpdateReferences != null )
			{
				DoUpdateReferences();
				DoUpdateReferences = null;
			}
			
			instanceDatabase.Clear();
		}
		
		
	}
}