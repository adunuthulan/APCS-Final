using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//creates spin for objects
public class Spin : MonoBehaviour {

    public float speed = 100;

	public Vector3 rotation = new Vector3(0, 1, 0);

	public bool pivoting;

	public Vector3 pivotPoint;

    private void Update()
    {
		if (pivoting){
			Debug.Log(transform.position + pivotPoint);
			transform.RotateAround(transform.position + pivotPoint, Vector3.up, speed * Time.deltaTime);
		}else {
			transform.Rotate(rotation * speed * Time.deltaTime);
		}
        //transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}
