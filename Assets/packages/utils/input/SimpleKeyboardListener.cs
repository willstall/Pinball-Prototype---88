using UnityEngine;
using System.Collections;

[AddComponentMenu("SimpleScripts/Input/SimpleKeyboardListener")]
public class SimpleKeyboardListener : MonoBehaviour
{	
	public KeyboardListener[] listeners = new KeyboardListener[1];
	
	void Update ()
	{
		foreach( KeyboardListener listener in listeners)
		{
            switch(listener.state)
            {
                case KeyboardState.DOWN:                
            		if(Input.GetKeyDown(listener.key))
            			SendMessage(listener.methodName);
                    break;  
                case KeyboardState.UP:
            		if(Input.GetKeyUp(listener.key))
            			SendMessage(listener.methodName);
            		break;
            	case KeyboardState.ACTIVE:
            		if(Input.GetKey(listener.key))
            			SendMessage(listener.methodName);
            		break;
            }
		}	
	}
}

[System.Serializable]
public class KeyboardListener
{
	public KeyCode key;
	public KeyboardState state;
	public string methodName;
	
	public KeyboardListener( KeyCode key, KeyboardState state, string methodName)
	{
		this.key = key;
		this.state = state;
		this.methodName = methodName;
	}
}

public enum KeyboardState
{
	DOWN,
	UP,
	ACTIVE
}
