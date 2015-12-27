using UnityEngine;
using System.Collections;

[AddComponentMenu("SimpleScripts/UI/SimpleButton")]
public class SimpleButton : MonoBehaviour 
{
	
	public GameObject messageRecieverObject;
	public string messageToSendOnPress = "ButtonPressed";
	
	private void Start()
	{
		if(messageRecieverObject == null)
			messageRecieverObject = this.gameObject;
	}
	
	private void OnMouseUpAsButton()
	{
		messageRecieverObject.SendMessage(messageToSendOnPress, this.gameObject, SendMessageOptions.DontRequireReceiver);
	}

}
