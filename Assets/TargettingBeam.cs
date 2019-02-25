using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class TargettingBeam : MonoBehaviour {

    [SerializeField, Tooltip("The default distance for the cursor when a hit is not detected.")]
    private float _defaultDistance = 9.0f;

    // Stores Renderer component
    private LineRenderer _render;

    #region Unity Methods
    /// <summary>
    /// Initializes variables and makes sure needed components exist.
    /// </summary>
    void Awake() {

        _render = GetComponent<LineRenderer>();
        if (_render == null) {
            Debug.LogError("Error: RaycastVisualizer._render is not set, disabling script.");
            enabled = false;
            return;
        }
    }
    #endregion

    #region Event Handlers
    /// <summary>
    /// Callback handler called when raycast has a result.
    /// Updates the transform an color on the Hit Position and Normal from the assigned object.
    /// </summary>
    /// <param name="state"> The state of the raycast result.</param>
    /// <param name="result"> The hit results (point, normal, distance).</param>
    /// <param name="confidence"> Confidence value of hit. 0 no hit, 1 sure hit.</param>
    public void OnRaycastHit(Vector3 origin, RaycastHit result) {
        // Update the cursor position and normal.
        _render.SetPosition(0, origin);
        _render.SetPosition(1, result.point);
        // Set the color to yellow if the hit is unobserved.
        _render.material.color = Color.green;

    }

    public void OnRaycastMiss(Vector3 origin, Vector3 direction) {
        _render.SetPosition(0, origin);
        _render.SetPosition(1, origin + direction.normalized * _defaultDistance);

        // Update the cursor position and normal.
        //  transform.position = (origin + (direction * _defaultDistance));
        //   transform.LookAt(origin);
        //  transform.localScale = Vector3.one;

        _render.material.color = Color.red;



    }
    #endregion
}
