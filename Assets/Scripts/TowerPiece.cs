using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TowerPiece : MonoBehaviour {

    public Material selectedMaterial;
    public Material outOfTowerMaterial;
    public Material setDownMaterial;
    private Material original;

    public bool isTop;
    public bool isTower = true;
    public float startingHeight = 0.0f;

    public AudioClip removeSound;

    private Vector3 position { get { return gameObject.transform.position; } }

    private static Bounds towerBounds;

    private GameObject followTarget = null;

    private Material material {
        get { return gameObject.GetComponent<Renderer>().material; }
        set { gameObject.GetComponent<Renderer>().material = value; }
    }

    private void Start() {
        gameObject.GetComponent<Rigidbody>().SetDensity(410.0f);
        startingHeight = position.y;
        if (towerBounds.size.magnitude == 0) {
            towerBounds = GetComponent<BoxCollider>().bounds;
        }
        towerBounds.Encapsulate(GetComponent<BoxCollider>().bounds);
    }

    private void Update() {
        if (followTarget != null && isTower && !isTop) {
            if (!GetComponent<BoxCollider>().bounds.Intersects(towerBounds)) {
                isTower = false;
                GetComponent<AudioSource>().clip = removeSound;
                GetComponent<AudioSource>().Play();
                material = outOfTowerMaterial;
                followTarget.GetComponent<ControllerManager>().EnterPlacement(this);
            }
        }
        else if (isTower || isTop) {
            if (Mathf.Abs(position.y - startingHeight) > gameObject.transform.localScale.y * gameObject.GetComponent<BoxCollider>().size.y) {
                GetComponent<AudioSource>().Play();
                GameManager.Singleton.TowerCollapse();
            }
        }
    }

    public void EnterRaycast() {
        original = material;
        material = selectedMaterial;

    }

    public void ExitRaycast() {
        material = isTower ? original : outOfTowerMaterial;

    }

    public void EnterSelect(GameObject target) {
        if (followTarget == null) {
            //print("selected");
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            followTarget = target;
            transform.parent = target.transform;
            GetComponent<Rigidbody>().isKinematic = true;
            gameObject.layer = 2;
            // GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void ExitSelect() {
        // print("exit select");
        transform.parent = null;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        followTarget = null;
        GetComponent<Rigidbody>().isKinematic = false;
        gameObject.layer = 0;
        // GetComponent<BoxCollider>().enabled = true;
    }

    public void IsPlaced() {
        GameManager.Singleton.BridgePlaced();
        material = setDownMaterial;
        ExitSelect();
        ExitRaycast();
        Destroy(GetComponent<Rigidbody>());
        Destroy(this);
    }

}
