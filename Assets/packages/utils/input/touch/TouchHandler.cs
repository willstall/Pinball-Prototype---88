using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TouchInput
{

	public class TouchHandler : MonoBehaviour
	{

		public LayerMask layers;
		public bool allowMouseEmulation = true;	
		public List<TouchData> touchObjects;

		TouchData mouseData;

		void Start()
		{
			touchObjects = new List<TouchData>();
			mouseData = null;
		}

		void AddTouchData( TouchData data )
		{
			if( !HasTouchData(data.index) )
				touchObjects.Add( data );
		}

		bool HasTouchData( int fingerId )
		{
			for(int i = 0; i < touchObjects.Count; i++ )
			{
				if( touchObjects[i].index == fingerId )
					return true;
			}

			return false;
		}

		TouchData GetTouchData( int fingerId )
		{
			for(int i = 0; i < touchObjects.Count; i++)
			{
				if( touchObjects[i].index == fingerId )
					return touchObjects[i];
			}

			return null;
		}

		void RemoveTouchData( TouchData data )
		{
			touchObjects.Remove(data);
		}


		void FixedUpdate()
		{
			Vector3 position;
			RaycastHit hit;

			if(Input.touchCount > 0)
			{
				//Debug.Log("----");
				foreach (Touch touch  in Input.touches)
				{	
					position = touch.position;
					TouchData data;				
					Debug.Log(touch.fingerId);

					if(touch.phase == TouchPhase.Began)
					{
						// create data
						if( FindCollision( position, out hit) )
						{
							data = new TouchData( touch.fingerId, position, hit.collider.gameObject, hit );
							AddTouchData( data );
							
							// send message	
							data.gameObject.SendMessage("StartDrag",data,SendMessageOptions.DontRequireReceiver);											
						}
					}else if( HasTouchData( touch.fingerId ) )
					{
						data = GetTouchData( touch.fingerId );

						switch( touch.phase )
						{
							case TouchPhase.Moved:
								// update data
								data.UpdatePosition( position );
								// send message
								data.gameObject.SendMessage("UpdateDrag",data,SendMessageOptions.DontRequireReceiver);
								break;
							case TouchPhase.Stationary:
								break;
							case TouchPhase.Ended:				
								// update data
								data.UpdatePosition( position );
								// send message
								data.gameObject.SendMessage("StopDrag",data,SendMessageOptions.DontRequireReceiver);
								// remove data
								RemoveTouchData( data );				
								break;
							case TouchPhase.Canceled:
								break;
						}
					}
				}
			}

			if(allowMouseEmulation == false)
				return;

			position = Input.mousePosition;

			if(Input.GetMouseButtonDown(0))	
			{
				// find available object
				if( FindCollision( position, out hit) )
				{
					// create data
					mouseData = new TouchData( 0, position, hit.collider.gameObject, hit );
					// send message	
					mouseData.gameObject.SendMessage("StartDrag",mouseData,SendMessageOptions.DontRequireReceiver);					
				}else{
					mouseData = null;
				}			

			}else if(Input.GetMouseButtonUp(0))
			{
				// find available object
				if(mouseData == null)
					return;

				// update data
				mouseData.UpdatePosition( position );
				// send message
				if(mouseData.gameObject != null)
					mouseData.gameObject.SendMessage("StopDrag",mouseData,SendMessageOptions.DontRequireReceiver);
				// remove data
				mouseData = null;

			}else if(Input.GetMouseButton(0))
			{
				// find available object
				if(mouseData == null)
					return;

				// update data
				mouseData.UpdatePosition( position );
				// send message
				if(mouseData.gameObject != null)
					mouseData.gameObject.SendMessage("UpdateDrag",mouseData,SendMessageOptions.DontRequireReceiver);
			}

		}

		bool FindCollision( Vector3 screenPosition , out RaycastHit hit)
		{
			Ray ray = Camera.main.ScreenPointToRay( screenPosition );				
			hit = new RaycastHit();

			if(Physics.Raycast( ray,out hit, Mathf.Infinity, layers ))
			{
				return true;
			}

			return false;		
		}


		void OnGUI()
		{
			//GUILayout.Label( "Touches: " + touchObjects.Count.ToString() );
		}

		void OnDrawGizmos()
		{
			// find available object
			if(touchObjects.Count == 0)
				return;

			Gizmos.color = Color.cyan;

			foreach(TouchData data in touchObjects)
			{
				
				Gizmos.DrawWireSphere(data.worldPositionWithOffset,1.0f);	
			}

			if((allowMouseEmulation == false)||(mouseData == null))
				return;

			Gizmos.DrawWireSphere(mouseData.worldPositionWithOffset,1.0f);	
		}
	}

	[System.Serializable]
	public class TouchData
	{
		
		public Vector3 runningDelta;
		public Vector3 worldRunningDelta;

		public int index;
		public bool isHeld = false;

		public Vector3 position;
		public GameObject gameObject;
		public RaycastHit hit;

		Vector3 delta;
		Vector3 worldDelta;
		Vector3 hitOffset;

		public TouchData( int index, Vector3 position , GameObject gameObject, RaycastHit hit)
		{
			this.index = index;
			this.delta = Vector3.zero;
			this.worldDelta = Vector3.zero;
			this.runningDelta = Vector3.zero;
			this.worldRunningDelta = Vector3.zero;
			this.position = position;
			this.gameObject = gameObject;
			this.hit = hit;
			this.hitOffset = hit.point - gameObject.transform.position;
		}

		public void UpdatePosition( Vector3 position )
		{
			Vector3 newWorldPosition = CalcWorldPosition( position );
			worldDelta = newWorldPosition - worldPosition;
			
			worldRunningDelta += worldDelta;
			worldRunningDelta *= 0.5f;

			delta = position - this.position;
			this.position = position;

			runningDelta += delta;
			runningDelta *= 0.5f;
		}

		Vector3 CalcWorldPosition( Vector3 position )
		{
			Vector3 worldPosition = new Vector3( position.x, position.y, gameObject.transform.position.z - Camera.main.transform.position.z);
			return Camera.main.ScreenToWorldPoint( worldPosition );	
		}

		public Vector3 worldPosition
		{
			get
			{
				return CalcWorldPosition( this.position );
			}	
		}
		
		public Vector3 worldPositionWithOffset
		{
			get
			{
				Vector3 position = new Vector3( this.position.x, this.position.y, gameObject.transform.position.z - Camera.main.transform.position.z);
				return Camera.main.ScreenToWorldPoint( position ) - hitOffset;
			}
		}

	}


}