// %BANNER_BEGIN%
// ---------------------------------------------------------------------
// %COPYRIGHT_BEGIN%
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// %COPYRIGHT_END%
// ---------------------------------------------------------------------
// %BANNER_END%

using UnityEngine;
using UnityEngine.XR.MagicLeap;

namespace MagicLeap {
    /// <summary>
    /// This class allows the user to cycle between various PlacementContent
    /// objects and see a visual representation of a valid or invalid location.
    /// Once a valid location has been determined the user may place the content.
    /// </summary>
    [RequireComponent(typeof(Placement))]
    public class JengaPlacement : MonoBehaviour {
        #region Private Variables
        [SerializeField, Tooltip("The controller that is used in the scene to cycle and place objects.")]
        public ControllerConnectionHandler _controllerConnectionHandler;

        public Placement _placement;
        private PlacementObject _placementObject;
        private GameObject original;
        #endregion

        #region Unity Methods
        void Start() {
            if (_controllerConnectionHandler == null) {
                Debug.LogError("Error: PlacementExample._controllerConnectionHandler is not set, disabling script.");
                enabled = false;
                return;
            }

            MLInput.OnTriggerUp += HandleOnTriggerUp;
            MLInput.OnTriggerDown += HandleOnTriggerDown;

        }

        void Update() {
            // Update the preview location, inside of the validation area.
            if (_placementObject != null) {
                _placementObject.transform.position = _placement.Position;
                _placementObject.transform.rotation = _placement.Rotation;
            }
        }

        void OnDestroy() {
            StopPlacement();
            MLInput.OnTriggerUp -= HandleOnTriggerUp;
            MLInput.OnTriggerDown -= HandleOnTriggerDown;
        }
        #endregion

        #region Event Handlers

        private void HandleOnTriggerUp(byte controllerId, float pressure) {
            _placement.Confirm();
            StopPlacement();
        }

        private void HandleOnTriggerDown(byte controllerId, float pressure) {
            //_placement.Confirm();
        }

        private void HandlePlacementComplete(Vector3 position, Quaternion rotation) {

            //GameObject content = Instantiate(_placementPrefabs[_placementIndex]);

            original.GetComponent<TowerPiece>().IsPlaced();
            //position.y += original.transform.localScale.y * original.GetComponent<BoxCollider>().size.y;
            original.transform.position = position;

            original.transform.rotation = rotation;
          //  Debug.Log("placement complete");

            //allow only one placement 
            //_placement.Resume();
            if (_placementObject != null) {
                var handler = _placementObject.GetComponent<HandlePlacementComplete>();
                if (handler != null) {
                    handler.OnPlacementComplete();
                }
                else {
                    Debug.LogWarning("No handler found");
                }
            }

        }
        #endregion

        #region Private Methods
        private PlacementObject CreatePlacementObject(GameObject jengaPiece) {
            // Destroy previous preview instance
            if (_placementObject != null) {
                Destroy(_placementObject.gameObject);
            }

            // Create the next preview instance.

            GameObject previewObject = Instantiate(jengaPiece);
            previewObject.AddComponent<PlacementObject>();
            // Detect all children in the preview and set children to ignore raycast.
            Collider[] colliders = previewObject.GetComponents<Collider>();
            for (int i = 0; i < colliders.Length; ++i) {
                colliders[i].enabled = false;
            }

            // Find the placement object.
            PlacementObject placementObject = previewObject.GetComponent<PlacementObject>();

            if (placementObject == null) {
                Destroy(previewObject);
                Debug.LogError("Error: PlacementExample.placementObject is not set, disabling script.");

                enabled = false;
            }
            return placementObject;
        }

        public void StartPlacement(GameObject jengaPiece) {
            original = jengaPiece;
            _placementObject = CreatePlacementObject(jengaPiece);
            _placementObject.GetComponent<MeshRenderer>().enabled = false;
            if (_placementObject != null) {
                StopPlacement();
                Vector3 volume = _placementObject.transform.localScale;
                volume.Scale(_placementObject.GetComponent<BoxCollider>().size);
                _placement.Place(_controllerConnectionHandler.transform, volume, _placementObject.AllowHorizontal, _placementObject.AllowVertical, HandlePlacementComplete);
            }
        }

        private void StopPlacement() {
            // print("stop placement");
            if (_placementObject != null) {
                _placement.Cancel();
            }
            if (_placementObject != null) {
                Destroy(_placementObject.gameObject);
            }
            GetComponent<PlacementVisualizer>().TurnOff();
        }

        #endregion

    }
}
