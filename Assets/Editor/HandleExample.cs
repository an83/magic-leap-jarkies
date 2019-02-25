
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class HandleExample : MonoBehaviour {

    [DrawGizmo(GizmoType.InSelectionHierarchy)]
    static void DrawGameObjectName(Transform transform, GizmoType gizmoType) {
        //Handles.Label(transform.position, transform.gameObject.name);
    }
}