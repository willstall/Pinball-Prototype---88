using UnityEngine;
using System.Collections;

public class OscillateRotation : MonoBehaviour
{
    
    public float frequency = 2;
    public float amplitude = 1;    
    public float offset = 0;
    public float baseRotation = 0;    
    
    void Start()
    {
    
    }
        
    void Update ()
    {
        float r = baseRotation + Mathf.Sin( ( Time.time + offset )* frequency) * amplitude;

        Quaternion rotation = Quaternion.Euler( 0, 0, r );

        transform.localRotation = rotation;
        
    }
}