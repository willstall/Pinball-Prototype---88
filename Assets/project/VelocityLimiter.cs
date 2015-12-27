using UnityEngine;
using System.Collections;

public class VelocityLimiter : MonoBehaviour
{
	public float maximumVelocity = 10.0f;
	[Range(0.0f,0.99f)]
	public float deacceleration = 0.99f;

	Rigidbody rb;
	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}

	void FixedUpdate()
	{
		if(rb.velocity.sqrMagnitude > maximumVelocity)
		{
		 //smoothness of the slowdown is controlled by the 0.99f, 
		 //0.5f is less smooth, 0.9999f is more smooth
		     rb.velocity *= deacceleration;
		}  		
	}
}