using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Dimensioner : MonoBehaviour {


    public Vector3 originalPos;
    public LayerMask blockLayer;

    private void Start()
    {
        if (DimensionManager.instance == null)
        {
            Debug.LogError("No GameManager found! What are you doing?!");
            return;
        }

        // Add method to event listener
        DimensionManager.instance.OnChangeDimension += ChangeDimension;

        originalPos = transform.position;
    }

    public void ChangeDimension(DimensionManager.Dimension dim)
    {
        // 2D -> 3D
        if (dim == DimensionManager.Dimension.ThreeD)
        {
            transform.position = originalPos;

            // move polayer back to saved position in 3d if it exists
        }
        // 3D -> 2D
        else
        {
            if (!Physics.Raycast(transform.position, Vector3.left, 10f, blockLayer))
            {
                transform.position = new Vector3(0, originalPos.y, originalPos.z);
            }

            if (this.gameObject.tag == "Rotate")
            {
                transform.Rotate(Vector3.left);
                //Debug.Log ("Rotated "+this.gameObject.tag);
            }
        }
    }

    private void OnDestroy()
    {
        // Remove method from event listener so it doesn't do errors
        DimensionManager.instance.OnChangeDimension -= ChangeDimension;
    }


}
