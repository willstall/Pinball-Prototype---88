using UnityEngine;
using System.Collections;


namespace ForceFields
{

	[ExecuteInEditMode]
	public class DebugForceField : MonoBehaviour 
	{

		public Color displayColor = new Color(0.5f, 0.92f, 0.5f);
		public float displayResolution = 0.5f;
		public float displayScale = 1;

		[Range(0,1)]
		public float displayZ = 0.5f;

		bool showGizmos = false;

		public void OnEnable()
		{
			showGizmos = true;
		}

		public void OnDisable()
		{
			showGizmos = false;
		}

		void OnDrawGizmosSelected()
		{	if(showGizmos == false)
				return;

			BoxCollider collider = GetComponent<BoxCollider>();
			Vector3 size = collider.size;
			Vector3 center = collider.center;
			Vector3 halfSize = 0.5f * size;

			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.color = displayColor;

			ForceField[] fields = GetComponents<ForceField>();

			//for( float z = -halfSize.z; z < halfSize.z; z += displayResolution ) {

			float z = Mathf.Lerp( -halfSize.z + center.z, halfSize.z + center.z, displayZ );
			for( float y = -halfSize.y + center.y; y < halfSize.y + center.y; y += displayResolution ) {
			for( float x = -halfSize.x + center.x; x < halfSize.x + center.x; x += displayResolution )
			{
				Vector3 pos = new Vector3(x, y, z);
				Vector3 vel = Vector3.zero;
				Vector3 localPos = transform.TransformPoint(pos);
				foreach( ForceField field in fields )
				{
					if( field.enabled )
						vel += field.GetVectorField( localPos );
				}
				
				Gizmos.DrawLine( pos, pos + vel * displayScale );
			}}
			//}

		}


	}


}