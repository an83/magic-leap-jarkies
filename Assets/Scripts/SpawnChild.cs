using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChild : MonoBehaviour
{

    public GameObject objectToSpawn;
    public int numberOfObjectsToSpawn;

	// Use this for initialization
	void Start () {
	    for (int i = 0; i < numberOfObjectsToSpawn; i++)
	    {
	        Instantiate(objectToSpawn, transform);
	    }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
