using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]		// need to fix weird edge case errors, be aware

[AddComponentMenu("SimpleScripts/Destroy/AutoDestroy")]
public class AutoDestroy : MonoBehaviour
{
	public float minTime = 1.0f;
	public float maxTime = 1.0f;
	public bool autoStart = true;
	
	private float _timer;
	
	private void Start ()
	{
		if(autoStart)
			Invoke("_Destroy", _GetInvokeTime());
	}
	
	private float _GetInvokeTime()
	{
		float invokeTime = Random.Range(minTime,maxTime);
		//Debug.Log(invokeTime);
		return invokeTime;
	}
	
	protected void _Destroy()
	{	
		Destroy (this.gameObject);	
	}
	
	public void DestroyCountdown()
	{
		DestroyCountdown( minTime, maxTime);
	}
	
	public void DestroyCountdown(float minTime, float maxTime)
	{
		this.minTime = minTime;
		this.maxTime = maxTime;
		Invoke("_Destroy", _GetInvokeTime());
	}
	
}
