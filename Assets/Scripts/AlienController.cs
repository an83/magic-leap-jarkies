using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Patrol))]
public class AlienController : MonoBehaviour
{
    //public bool IsWalking;
    //private Animator _animator;
    public float _maxX = 3;
    public float _maxZ = 3;

    private Transform _tower;

    // Use this for initialization
    void Start ()
	{
	    //_animator = GetComponent<Animator>();
	    var patrol = GetComponent<Patrol>();
	    patrol.enabled = false;
	    patrol.points = GenerateTargets(3);
	    patrol.enabled = true;

        _tower = GameObject.Find("JengaPlatform(Clone)").transform;
    }

    private Transform[] GenerateTargets(int count)
    {
        List<GameObject> targets = new List<GameObject>();

        for (var i = 0; i < count; i++)
        {
            var target = new GameObject("target");
            //target.transform.SetParent(_tower);
            target.transform.SetParent(transform.parent);
            target.transform.localPosition = GetRandomTarget();

            targets.Add(target);
        }

        return targets.Select(t => t.transform).ToArray();
    }

    private Vector3 GetRandomTarget()
    {
        return new Vector3(Random.Range(-_maxX, _maxX), transform.position.y, Random.Range(-_maxZ, _maxZ));
    }

    // Update is called once per frame
	void Update () {
		//_animator.SetBool("walk", IsWalking);

	}
}
