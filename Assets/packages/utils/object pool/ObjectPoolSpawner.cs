using UnityEngine;
using System.Collections;

public class ObjectPoolSpawner : MonoBehaviour
{	
	public GameObject prefabToSpawn;
	public Transform prefabParent;	
	public bool autoStart = true;
	public bool spawnAtStart = true;
	public bool poolObjects = true;
	public float spawnDuration = 1.0f;
	public float collisionRadius = 3.0f;
	public LayerMask collisionLayer = -1;

	Timer timer;
	
	void Start()
	{
		if(prefabToSpawn == null)
			return;

		timer = gameObject.AddComponent<Timer>();
		timer.durationInSeconds = spawnDuration;
		timer.autoStart = autoStart;

		if(spawnAtStart == true)
			 Spawn();
	}
	
	void Update()
	{
		if(timer.isFinished)
		{
			if(	Spawn() )
			{
				timer.durationInSeconds = spawnDuration;
				timer.ResetTimer();
			}
		}
	}

	bool Spawn()
	{
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, collisionRadius, collisionLayer);
        
        if(hitColliders.Length > 0)
		{
        	return false;
		}
		GameObject prefab;

		if(poolObjects == true)
		{
			prefab = prefabToSpawn.Spawn( transform.position , transform.rotation);		
		}else{
			prefab = Instantiate( prefabToSpawn, transform.position , transform.rotation ) as GameObject;
		}
		prefab.transform.parent = prefabParent;

		return true;
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, collisionRadius);
	}

}
