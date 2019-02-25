
using MagicLeap;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class ControllerManager : MonoBehaviour {

    public static ControllerManager Singleton { get; private set; }

    public Transform CurrentTarget {
        get;
        private set;
    }

    public GameObject PlacementHandler;
    new MeshCollider collider;


    private enum Status { NONE, SELECTED, PLACING }

    private Status status;

    private void Start() {
        if (Singleton != null) {
            Debug.LogError("Raycast controller singleton already exists");
            Destroy(this);
        }
        Singleton = this;
        CurrentTarget = null;
        SkinnedMeshRenderer meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        collider = GetComponentInChildren<MeshCollider>();
        Mesh colliderMesh = new Mesh();
        meshRenderer.BakeMesh(colliderMesh);
        collider.sharedMesh = null;
        collider.sharedMesh = colliderMesh;

        MLInput.OnTriggerDown += TriggerDown;
        MLInput.OnTriggerUp += TriggerUp;
    }

    private void Update() {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 20.0f)) {
            if (status == Status.NONE) {
                OnRaycastHit(hit);
            }
            GetComponentInChildren<TargettingBeam>().OnRaycastHit(transform.position, hit);
        }
        else {
            GetComponentInChildren<TargettingBeam>().OnRaycastMiss(transform.position, transform.TransformDirection(Vector3.forward));
            if (status == Status.NONE && CurrentTarget != null) {
                if (CurrentTarget.GetComponent<TowerPiece>() != null) {
                    CurrentTarget.GetComponent<TowerPiece>().ExitRaycast();
                }
                CurrentTarget = null;
            }
        }

    }

    /*
     *  @Requires !selected && !isPlacing
     */
    public void OnRaycastHit(RaycastHit result) {

        if (result.transform.GetComponent<TowerPiece>() != null) {
            if (result.transform != CurrentTarget) {
                if (CurrentTarget != null) {
                    CurrentTarget.GetComponent<TowerPiece>().ExitRaycast();
                }
                CurrentTarget = result.transform;
                CurrentTarget.GetComponent<TowerPiece>().EnterRaycast();
            }
        }
        else if (CurrentTarget != null) {
            if (CurrentTarget.GetComponent<TowerPiece>() != null) {
                CurrentTarget.GetComponent<TowerPiece>().ExitRaycast();
            }
            CurrentTarget = null;
        }

    }

    private void TriggerDown(byte controllerId, float triggerValue) {
        if (CurrentTarget != null && status == Status.NONE) {
            CurrentTarget.GetComponent<TowerPiece>().EnterSelect(gameObject);
            if (CurrentTarget.GetComponent<TowerPiece>().isTower) {
                status = Status.SELECTED;
            }
            else { EnterPlacement(CurrentTarget.GetComponent<TowerPiece>()); }
            GetComponentInChildren<Animator>().SetBool("isPinching", true);
            collider.enabled = false;
        }
    }

    private void TriggerUp(byte controllerId, float triggerValue) {
        if (CurrentTarget != null && status != Status.NONE) {
            if (CurrentTarget.GetComponent<TowerPiece>() != null) {
                CurrentTarget.GetComponent<TowerPiece>().ExitSelect();
            }
            PlacementHandler.SetActive(false);
            status = Status.NONE;
            GetComponentInChildren<Animator>().SetBool("isPinching", false);
            collider.enabled = true;
        }
    }

    public void EnterPlacement(TowerPiece placementObject) {
        if (placementObject.gameObject != CurrentTarget.gameObject) {
            Debug.LogError("Placement object is not current object");
        }
        PlacementHandler.SetActive(true);
        status = Status.PLACING;
        GetComponentInChildren<JengaPlacement>().StartPlacement(placementObject.gameObject);
    }

    public void ExitPlacement() {

        status = Status.NONE;
        //   currentTarget = null;
        PlacementHandler.SetActive(false);
    }
}
