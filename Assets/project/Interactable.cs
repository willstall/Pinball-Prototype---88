using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
	public LayerMask layersToAffect = -1;

	[Range(0.0f,1.0f)]
	public float idleAlpha = 0.2f;
	[Range(0.0f,1.0f)]
	public float activeAlpha = 0.7f;
	[Range(0.0f,1.0f)]
	public float selectAlpha = 0.45f;

	public string activateMessage = "Activate";

	Material mat;
	Color color;
	
	Collider collider;

	bool isSelected;

	void Start ()
	{
		mat = GetComponent<Renderer>().material;
		color = mat.color;
		collider = GetComponent<Collider>();

		isSelected = false;

		Idle();
	}

	void Update ()
	{
		if(!isSelected)
			return;

		if(Input.GetKeyDown( KeyCode.Space ))
		{
			SendMessage(activateMessage, SendMessageOptions.DontRequireReceiver);
			Active();
		}
	}

	void OnTriggerEnter( Collider other )
	{
		Debug.Log("hey");
		if( !ShouldAffect(other) )
			return;
		
		isSelected = false;
		Select();
	}

	void OnTriggerExit( Collider other )
	{
		if( !ShouldAffect(other) )
			return;	

		isSelected = true;
		Idle();
	}

	void Idle()
	{
		color.a = idleAlpha;
		mat.color = color;
	}

	void Active()
	{
		color.a = activeAlpha;
		mat.color = color;
	}

	void Select()
	{
		Debug.Log("do it");
		color.a = selectAlpha;
		mat.color = color;	
	}

	public bool ShouldAffect( Collider other )
	{
		return IsInLayerMask(other.gameObject, layersToAffect);
	}

	static bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
       return ((mask.value & (1 << obj.layer)) > 0);
    }
}
