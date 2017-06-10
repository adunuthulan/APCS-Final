using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//makes the important objects not be destroyed from stage to stage
public class DontDestroy : MonoBehaviour {

    public static List<DontDestroy> dontDestroyObjects = new List<DontDestroy>();

    private void Awake()
    {
        dontDestroyObjects.Add(this);
        DontDestroyOnLoad(transform.gameObject);
    }
}
