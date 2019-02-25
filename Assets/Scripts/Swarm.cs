using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm : MonoBehaviour
{

    private Vector3 currentTarget;
    public float maxX = 1;
    public float maxZ = 1;

    // Use this for initialization
    void Start()
    {

        RandomTarget();

    }

    private void RandomTarget()
    {
        currentTarget = new Vector3(Random.Range(-maxX, maxX), this.transform.localPosition.y, Random.Range(-maxZ, maxZ));
    }

    // Update is called once per frame
    void Update()
    {

        this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, currentTarget, Time.deltaTime * 0.25f);

        this.transform.localRotation = Quaternion.RotateTowards(
            this.transform.localRotation,
            Quaternion.LookRotation(currentTarget - this.transform.localPosition, Vector3.up),
            Time.deltaTime * 180.0f);

        float distance = Vector3.Distance(this.transform.localPosition, currentTarget);
        if (distance < 0.1f) RandomTarget();
    }



}