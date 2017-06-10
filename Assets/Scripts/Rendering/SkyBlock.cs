using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//makes skyblock invisible in 2d and visible in 3d
public class SkyBlock : MonoBehaviour {
	
	void Update () {
		if (DimensionManager.instance.currentDimension == DimensionManager.Dimension.ThreeD) {
			GetComponent<Renderer>().enabled = true;
		} else {
			GetComponent<Renderer>().enabled = false;
		}
	}
}
