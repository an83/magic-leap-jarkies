using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {

    public GameObject jengaPiece;
    public Transform spawnLocationBase = null;

    private int layer = 0;
    public int layerCount = 18;

    public GameObject preppedTower;
    // Use this for initialization
    void Start() {
        if (spawnLocationBase == null) {
            spawnLocationBase = transform;
        }
        //GameObject.Instantiate(preppedTower, spawnLocationBase.transform.position,Quaternion.identity);
    }

    private void Update() {
        if (layer < layerCount) {
            for (int i = -1; i < 2; i++) {
                Quaternion q = Quaternion.identity;
                Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f);
                if (layer % 2 == 1) {
                    pos.x = jengaPiece.transform.localScale.z * jengaPiece.GetComponent<BoxCollider>().size.z * i;
                    q = Quaternion.AngleAxis(90, new Vector3(0, 1, 0));
                }
                else {
                    pos.z = jengaPiece.transform.localScale.z * jengaPiece.GetComponent<BoxCollider>().size.z * i;
                }
                pos.y = (layer + 0.01f) * (jengaPiece.transform.localScale.y * jengaPiece.GetComponent<BoxCollider>().size.y);

                GameObject piece = Instantiate(jengaPiece, pos + spawnLocationBase.transform.position, q);
                if(layer == layerCount - 1) {
                    piece.GetComponent<TowerPiece>().isTop = true;
                }
            }
        }
        else {
            Destroy(this);
        }
        layer++;
    }
}
