using UnityEngine;
using System.Collections;

public class ObjectPoolAutoDestroy : MonoBehaviour
{

	public float durationTillDeath = 10.0f;

	Timer timer;
	void Awake()
	{
		timer = gameObject.AddComponent<Timer>();
		timer.autoStart = false;		
	}
	
	void OnEnable()
	{
		timer.durationInSeconds = durationTillDeath;		
		timer.StartTimer();
	}

	void OnDisable()
	{
		timer.StopTimer();
	}

	void Update ()
	{
		if(timer.isFinished)
		{
			gameObject.Recycle();	
		}
	}
}
