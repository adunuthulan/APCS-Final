using UnityEngine;
using System.Collections;

//makes text face player
public class BillboardSprite : MonoBehaviour
{
    private Transform cam;
    [Tooltip("Check to always be parallel to camera")]
    public bool alignNotLook = false;

    void LateUpdate()
    {
        // Try to update current camera
        if (CameraSwitch.instance != null)
            cam = CameraSwitch.instance.GetCurrentCamera().transform;

        if (cam == null) return;

        // Align to screen border in 2D
        alignNotLook = (DimensionManager.instance.currentDimension != DimensionManager.Dimension.ThreeD);

        // Align camera in specific way
        if (alignNotLook)
            transform.forward = cam.forward;
        else
        {
            transform.LookAt(new Vector3(cam.position.x, transform.position.y, cam.position.z), Vector3.up);
            transform.Rotate(new Vector3(0, 180, 0));
        }
    }
}