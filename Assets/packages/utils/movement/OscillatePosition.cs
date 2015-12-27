using UnityEngine;
using System.Collections;

public class OscillatePosition : MonoBehaviour
{
    
    public float frequency = 2;
    public float amplitude = 1;    
    public float offset = 0;
    public Vector3 movementDirection;
    

    Vector3 basePosition;
    
    void Start()
    {
    	basePosition = transform.localPosition;
    }
        
    void Update ()
    {
        Vector3 targetPosition = basePosition + movementDirection * Mathf.Sin( ( Time.time + offset )* frequency) * amplitude;
                //targetPosition.x += speed * Time.deltaTime * direction;
                        
        transform.localPosition = targetPosition;
        
    }
}