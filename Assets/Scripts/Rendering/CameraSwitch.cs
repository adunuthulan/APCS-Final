using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//switches camera dimensions
public class CameraSwitch : MonoBehaviour {

    public static CameraSwitch instance;

	public GameObject threeDCamera;
	public GameObject twoDCamera;

    private void Awake()
    {
        // Check for duplicates to use instances
        if (instance != null)
            Debug.LogError("There are more than one CameraSwitches in the scene");
        else
            instance = this;
    }

    void Start () {
		// Add method to event listener
		DimensionManager.instance.OnChangeDimension += ChangeDimension;

		ChangeDimension(Config.START_DIMENSION);
	}
	
    // Changes camera when dimension changes
	public void ChangeDimension (DimensionManager.Dimension dim) {
		// 2D -> 3D
		if (dim == DimensionManager.Dimension.ThreeD) {
			threeDCamera.SetActive(true);
			twoDCamera.SetActive(false);
		}
		// 3D -> 2D
		else {
			twoDCamera.SetActive(true);
			threeDCamera.SetActive(false);
		}
	}

    // Get camera currently being used
    public GameObject GetCurrentCamera ()
    {
        // 3D
        if (DimensionManager.instance.currentDimension == DimensionManager.Dimension.ThreeD)
        {
            return threeDCamera;
        }
        // 2D
        else
        {
            return twoDCamera;
        }
    }
}
