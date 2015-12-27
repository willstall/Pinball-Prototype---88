using UnityEngine;
using System.Collections;

[AddComponentMenu("SimpleScripts/Movement/SimpleRotator")]
public class SimpleRotator : MonoBehaviour {

	public Vector3 axisOfRotation = Vector3.up;
	public float speed= 10.0f;
	public bool useLocalSpace = true;

	private Transform _transform;

	void Start () 
    {
		_transform = this.transform;	
	}
	
	void Update () 
    {
		Space space = (useLocalSpace) ? Space.Self : Space.World ;
		_transform.Rotate(axisOfRotation.normalized * speed * Time.deltaTime, space);	
	}

    public void SetRotationSpeed( float speed )
    {
        this.speed = speed;  
    }

    public void SwitchDirection()
    {
        speed *= -1;
    }
}
