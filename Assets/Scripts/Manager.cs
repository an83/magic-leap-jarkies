using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.MagicLeap;

public class Manager : MonoBehaviour {

    [SerializeField, Tooltip("Controller")]
    ControllerConnectionHandler _controller;

    public GameObject AliensPrefab;
    public GameObject CityPrefab;

    private bool _isTriggered;
    private bool _cityAdded;
    private bool _aliensAdded;

    void Awake()
    {
        if (_controller == null)
        {
            Debug.LogError("Error: _controller is not set, disabling script.");
            enabled = false;
            return;
        }
        
        MLInput.OnControllerButtonDown += HandleControllerButtonDown;
        MLInput.OnTriggerUp += TriggerUp;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDestroy()
    {
        MLInput.OnControllerButtonDown -= HandleControllerButtonDown;
        MLInput.OnTriggerUp -= TriggerUp;
    }

    void HandleControllerButtonDown(byte controllerId, MLInputControllerButton button)
    {
        if (!_controller.IsControllerValid(controllerId))
        {
            return;
        }

        if (button == MLInputControllerButton.Bumper)
        {
            Debug.Log("bumper");

            if (!_aliensAdded)
            {
                _aliensAdded = true;
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

        }
        else if (button == MLInputControllerButton.HomeTap)
        {
            Debug.Log("home");
            Application.Quit();
        }
    }

    private void TriggerUp(byte controllerId, float triggerValue)
    {
        if (!_controller.IsControllerValid(controllerId))
        {
            return;
        }

        //Skip if aliens added 
        if(_aliensAdded) return;

        Debug.Log("trigger");
        _isTriggered = true;
    }

    public void OnRaycastHit(MLWorldRays.MLWorldRaycastResultState state, RaycastHit result, float confidence)
    {
        if(!_isTriggered) return;
        

        if (state != MLWorldRays.MLWorldRaycastResultState.RequestFailed && state != MLWorldRays.MLWorldRaycastResultState.NoCollision)
        {

            //transform.position = result.point;
            //transform.LookAt(result.normal + result.point);
            //transform.localScale = Vector3.one;

            if (!_cityAdded)
            {
                AddCitysObj(result);
            }
            else
            {
                AddAliensObj(result);
            }

            _isTriggered = false;
        }
    }

    private void AddAliensObj(RaycastHit result)
    {
        if (AliensPrefab == null)
        {
            Debug.LogWarning("obj prefab not defined");
            return;
        }

        var obj = Instantiate(AliensPrefab, result.point, Quaternion.identity);
        //var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //obj.transform.position = result.point;
        //obj.transform.LookAt(result.normal + result.point);
        obj.transform.localScale = new Vector3(.25f, .25f, .25f);
        //obj.transform.localScale = result.transform.localScale;

        //character.transform.SetParent(planeVisual.transform);
        //character.transform.localPosition = Vector3.zero;

        var spawnComponent = obj.GetComponent<Spawn>();
        if (spawnComponent != null)
        {
            spawnComponent.enabled = true;
        }

        Debug.Log("object added");
    }


    private void AddCitysObj(RaycastHit result)
    {
        if (CityPrefab == null)
        {
            Debug.LogWarning("city prefab not defined");
            return;
        }

        var obj = Instantiate(CityPrefab, result.point, Quaternion.identity);
        //var obj = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //obj.transform.position = result.point;
        //obj.transform.LookAt(result.normal + result.point);
        //obj.transform.localScale = new Vector3(.25f, .25f, .25f);
        //obj.transform.localScale = result.transform.localScale;

        //character.transform.SetParent(planeVisual.transform);
        //character.transform.localPosition = Vector3.zero;

        var spawnComponent = obj.GetComponent<Spawn>();
        if (spawnComponent != null)
        {
            spawnComponent.enabled = true;
        }

        Debug.Log("city added");

        _cityAdded = true;
    }
}
