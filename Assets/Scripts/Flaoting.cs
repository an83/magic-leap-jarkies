using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flaoting : MonoBehaviour {
    public  float waveSpeed = 25;
    public float waveCount = 1;
    public float waveHeight = .15f;
    
    // Use this for initialization
	void Start ()
	{
	    
	}
	
	// Update is called once per frame
	void Update () {

	    Vector3 p = this.transform.localPosition;

	    
	    float phase = Time.timeSinceLevelLoad / 20.0f * waveSpeed;
	    float offset = (p.x * (p.z * 0.2f)) * waveCount;
	    p.y = Mathf.Sin(phase + offset) * waveHeight;


	    this.transform.localPosition = p;
    }
}
