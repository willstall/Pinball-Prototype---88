using UnityEngine;
using System.Collections;

namespace TouchInput
{

	[RequireComponent(typeof(Rigidbody))]
	public class TouchDragReceiver : MonoBehaviour 
	{

		public float force = 1.0f;
		public float maxForce = 10.0f;

		public float bumpingForce = 1.0f;
		public float mass = 1.0f;

		[Range(0,1)]
		public float massDeaccel = 0.3f;

		//public string layerWhileInteracting = "default";

		float originalMass;
		new Rigidbody rigidbody;
		TouchData data;
		int originalLayer;
		bool isBeindHeld;

		void Start()
		{
			rigidbody = GetComponent<Rigidbody>();
			originalMass = rigidbody.mass;
			isBeindHeld = false;
			//originalLayer = gameObject.layer;
		}

		public void OnCollisionEnter( Collision  collision )
		{
			if(isBeindHeld == false)
				return;

			Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
			if(rb != null)
			{
				//rb.AddForce( data.worldRunningDelta * bumpingForce, ForceMode.Impulse);				
				rb.AddForce( LimitForce( bumpingForce, data), ForceMode.Impulse);
			}
		}

		public void FixedUpdate()
		{
			rigidbody.mass = Mathf.Lerp( rigidbody.mass, originalMass , massDeaccel);

			//if( Mathf.Approximately(rigidbody.mass, originalMass ) )
				//gameObject.layer = originalLayer;
		}

		public void StartDrag( TouchData data )
		{
			UpdatePosition( data );
			this.data = data;
			isBeindHeld = true;
			//gameObject.layer = LayerMask.NameToLayer( layerWhileInteracting );
		}

		public void UpdateDrag( TouchData data )
		{
			UpdatePosition( data );
			this.data = data;
		}

		public void StopDrag( TouchData data )
		{			
			//rigidbody.AddForce( data.runningDelta * force, ForceMode.Impulse);

			rigidbody.AddForce( LimitForce( force, data ), ForceMode.Impulse);
			rigidbody.mass = mass;
			this.data = data;
			isBeindHeld = false;
		}

		void UpdatePosition( TouchData data )
		{
			Vector3 position = data.worldPositionWithOffset;
			position.z = transform.position.z;
			transform.position = position;		
		}

		public Vector3 LimitForce( float force , TouchData data)
		{
			Vector3 appliedForce = data.worldRunningDelta * force;
			appliedForce.x = Mathf.Clamp( appliedForce.x, maxForce * -1, maxForce );
			appliedForce.y = Mathf.Clamp( appliedForce.y, maxForce * -1, maxForce );
			return appliedForce;
		}

	}

}