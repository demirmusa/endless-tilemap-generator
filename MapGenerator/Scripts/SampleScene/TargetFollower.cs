using UnityEngine;
using System.Collections;

/** \brief Sample script to follow a target */
public class TargetFollower : MonoBehaviour 
{
	public Transform Target;

    [SerializeField]
    private int _minDist = 2;

    [SerializeField]
    private int _speed = 3;

    void Update () 
	{		
		transform.LookAt(Target);		

		if(Vector3.Distance(transform.position,Target.position) >= _minDist)
		{
			transform.position += transform.forward*_speed*Time.deltaTime;
		}
	}
}
