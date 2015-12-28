using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
	[Range(0.0f,1.0f)]
	public float idleAlpha = 0.2f;
	[Range(0.0f,1.0f)]
	public float activeAlpha = 0.7f;
	[Range(0.0f,1.0f)]
	public float selectAlpha = 0.45f;

	Material mat;
	Color color;
	
	void Start ()
	{
		mat = GetComponent<Renderer>().material;
		color = mat.color;
		Idle();
	}

	void Update ()
	{
	
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
		color.a = selectAlpha;
		mat.color = color;
	}
}
