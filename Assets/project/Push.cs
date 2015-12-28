using UnityEngine;
using System.Collections;

public class Push : MonoBehaviour
{
	public float force = 10.0f;

	void Activated( GameObject other )
	{
		if(!other)
			return;

		other.GetComponent<Rigidbody>().AddForce( transform.up * force, ForceMode.Impulse );	
	}
}
