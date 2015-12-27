using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class DrawRectAtCameraDepth : MonoBehaviour 
{

	public float depth;
	public Color color = Color.white;

	void OnDrawGizmos()
	{
		Camera cam = GetComponent<Camera>();
		Vector3 p1 = cam.ViewportToWorldPoint( new Vector3(0,0,depth) );
		Vector3 p2 = cam.ViewportToWorldPoint( new Vector3(0,1,depth) );
		Vector3 p3 = cam.ViewportToWorldPoint( new Vector3(1,1,depth) );
		Vector3 p4 = cam.ViewportToWorldPoint( new Vector3(1,0,depth) );

		Gizmos.color = color;
		Gizmos.DrawLine( p1, p2 );
		Gizmos.DrawLine( p2, p3 );
		Gizmos.DrawLine( p3, p4 );
		Gizmos.DrawLine( p4, p1 );
	}	
}
