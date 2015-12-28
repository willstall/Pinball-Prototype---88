using UnityEngine;
using System.Collections;

public class HorizontalMovement : MonoBehaviour
{
	public float movementSpeed = 1.0f;
	public ForceMode mode = ForceMode.Impulse;
	Rigidbody rb;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			rb.AddForce( Vector3.left * movementSpeed, mode );
		}

		if(Input.GetKey(KeyCode.RightArrow))
		{
			rb.AddForce( Vector3.right * movementSpeed, mode );
		}		
	}
}
