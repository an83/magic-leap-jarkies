using UnityEngine;
using System.Collections;

// The GameObject is made to bounce using the space key.
// Also the GameOject can be moved forward/backward and left/right.
// Add a Quad to the scene so this GameObject can collider with a floor.

public class Move : MonoBehaviour {
    public float speed = 0.2f;

    private Vector3 moveDirection = Vector3.zero;

    void Update() {

        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")).normalized * speed;
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection = moveDirection * speed;

        transform.position += moveDirection;

    }
}