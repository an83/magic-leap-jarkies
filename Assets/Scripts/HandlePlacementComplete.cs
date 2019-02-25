using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePlacementComplete : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPlacementComplete()
    {
        Debug.Log("OnPlacementComplete");

        var plane = gameObject.transform.GetChild(0).gameObject;
        plane.SetActive(true);

        Debug.Log("activated");
    }
}
