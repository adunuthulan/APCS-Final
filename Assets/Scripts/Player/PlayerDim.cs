using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PlayerDim : MonoBehaviour {
    public LayerMask blockLayer;
    public float posX;

    public Vector3 pos;

    private float distToGround;

    private void Start()
    {
        // Add method to event listener
        DimensionManager.instance.OnChangeDimension += ChangeDimension;

        posX = transform.position.x;
        pos = transform.position;

        distToGround = GetComponent<Collider>().bounds.extents.y;

    }

    private void Update()
    {
        // If 3D, update the 3D saved value all the time
        if (DimensionManager.instance.currentDimension == DimensionManager.Dimension.ThreeD)
        {
            posX = transform.position.x;
            pos = transform.position;
        }
        // If 2D
        else
        {
            RaycastHit hit;

            // If the old one was good keep it

            if (Physics.Raycast(pos, Vector3.down, out hit))
            {
                if (hit.collider.gameObject.GetComponent<Dimensioner>() != null)
                {
                    return;
                }
            }

            // If there is a cube under the player, change the player's 3D saved value to that block's 3D saved value
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                Dimensioner dim = hit.collider.gameObject.GetComponent<Dimensioner>();
                if (dim != null)
                {
                    posX = dim.originalPos.x;
                }
            }
        }

    }

    public void ChangeDimension(DimensionManager.Dimension dim)
    {
        // 2D -> 3D
        if (dim == DimensionManager.Dimension.ThreeD)
        {
            transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        }
        // 3D -> 2D
        else
        {
            if (!Physics.Raycast(transform.position, Vector3.left, 10f, blockLayer))
            {
                transform.position = new Vector3(0, transform.position.y, transform.position.z);
            }
        }
    }

    private void OnDestroy()
    {
        // Remove method from event listener so it doesn't do errors
        DimensionManager.instance.OnChangeDimension -= ChangeDimension;
    }


}
