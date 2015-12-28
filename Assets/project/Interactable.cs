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

	public string activateMessage = "Activated";

	public float timeout = 1.0f;

	Material mat;
	Color color;
	
	Collider collider;

	bool isSelected;
	float timer;

	GameObject target;

	void Start ()
	{
		mat = GetComponent<Renderer>().material;
		color = mat.color;
		collider = GetComponent<Collider>();

		isSelected = false;
		timer = timeout;

		SetAlpha( idleAlpha );
	}

	void Update ()
	{
		if(isSelected)
		{
			SetAlpha( selectAlpha );
		}else{
			SetAlpha( idleAlpha );
		}

		if(Input.GetKeyDown( KeyCode.Space ))
		{	
			if(timer >= timeout)
			{	
				SetAlpha( activeAlpha );	
				timer = 0;				
				if(isSelected)
					Activate();
			}		
		}

		timer += Time.deltaTime;
	}

	void Activate()
	{
		SendMessage(activateMessage, target, SendMessageOptions.DontRequireReceiver);		
		//Debug.Log("Activate");
	}

	void OnTriggerEnter( Collider other )
	{
		if( !ShouldAffect(other) )
			return;

		target = other.gameObject;
		isSelected = true;
	}

	void OnTriggerExit( Collider other )
	{
		if( !ShouldAffect(other) )
			return;	

		target = other.gameObject;
		isSelected = false;
	}

	void SetAlpha( float alpha )
	{
		color.a = alpha;
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
