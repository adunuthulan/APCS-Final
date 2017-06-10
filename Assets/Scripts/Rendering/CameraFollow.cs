using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//allows camera to follow player
public class CameraFollow : MonoBehaviour
{

	public Vector3 offset;
	public Transform target;

	void Update ()
	{
		if (target) {
			if (DimensionManager.instance.currentDimension == DimensionManager.Dimension.ThreeD)
				ThreeDCamera ();
			else
				TwoDCamera ();
		}
	}

	void ThreeDCamera ()
	{
		transform.position = target.transform.position + offset;
	}

	void TwoDCamera ()
	{
		transform.position = target.transform.position + offset;
	}
}
