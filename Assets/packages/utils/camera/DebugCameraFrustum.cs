using UnityEngine;
using System.Collections;

[AddComponentMenu("SimpleScripts/Utils/DebugCameraFrustrum")]
public class DebugCameraFrustum : MonoBehaviour {
	
	void OnDrawGizmos()
	{
		float z =  this.GetComponent<Camera>().nearClipPlane;
		Vector3 v1a = this.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0f, 0f, z));
		Vector3 v2a = this.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0f, 1f, z));
		Vector3 v3a = this.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1f, 1f, z));
		Vector3 v4a = this.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1f, 0f, z));
		
		z = this.GetComponent<Camera>().farClipPlane;
		Vector3 v1b = this.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0f, 0f, z));
		Vector3 v2b = this.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0f, 1f, z));
		Vector3 v3b = this.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1f, 1f, z));
		Vector3 v4b = this.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1f, 0f, z));
		
		Gizmos.DrawLine(v1a, v1b);
		Gizmos.DrawLine(v2a, v2b);
		Gizmos.DrawLine(v3a, v3b);
		Gizmos.DrawLine(v3a, v4b);
		
		Gizmos.DrawLine(v1a, v2a);
		Gizmos.DrawLine(v1a, v4a);
		Gizmos.DrawLine(v3a, v2a);
		Gizmos.DrawLine(v3a, v4a);
		
		Gizmos.DrawLine(v1b, v2b);
		Gizmos.DrawLine(v1b, v4b);
		Gizmos.DrawLine(v3b, v2b);
		Gizmos.DrawLine(v3b, v4b);
	}
}
