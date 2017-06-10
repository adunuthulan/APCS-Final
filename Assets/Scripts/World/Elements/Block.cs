using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Dimensioner))]
public class Block : MonoBehaviour {

    public GameObject objectInside;

    [HideInInspector]
    public bool used = false;

    private AudioSource source;
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (used)
            return;

        // If it's the player and he's under the box
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<Collider>().bounds.max.y < transform.position.y && Mathf.Abs(collision.transform.position.x - transform.position.x) < 0.9f)
        {
            // Make sure all the player was also under it if it was in 3D
            if (DimensionManager.instance.currentDimension == DimensionManager.Dimension.ThreeD && Mathf.Abs(collision.transform.position.z - transform.position.z) > 0.9f) return;


            // Make the block used, so it can't be used more than once
            used = true;

            // Play sound
            source.PlayOneShot(source.clip);

            if (gameObject.tag != "Brick")
            {
                // Spawn the item
                GameObject newObj = Instantiate(objectInside, transform.position, Quaternion.identity);
                newObj.transform.name = objectInside.name;

                newObj.GetComponent<Dimensioner>().originalPos = (GetComponent<Dimensioner>().originalPos + new Vector3(0, 1, 0));

                // Start item animation from inside -> out
                StartCoroutine(MoveUp(newObj));
            }
            else
            {
                StartCoroutine(Brick());
            }

        }
    }

    private void Update()
    {
        // When player is restarting, the brick needs to reappear, so re-enable everything
        if (!used && gameObject.tag != "Brick" && !GetComponent<Collider>().enabled)
        {
            GetComponent<Collider>().enabled = true;
            GetComponent<MeshRenderer>().enabled = true;
        }
    }

    // Makes the passed game object move up by 0.001f every frame until it has gone 1 unit higher
    private IEnumerator MoveUp(GameObject newObj)
    {
        while (newObj.transform.position != transform.position + Vector3.up)
        {
            newObj.transform.position = Vector3.MoveTowards(newObj.transform.position, transform.position + Vector3.up, 0.175f);
            yield return new WaitForSeconds(0.001f);
            newObj.GetComponent<Dimensioner>().originalPos = (GetComponent<Dimensioner>().originalPos + new Vector3(0, 1, 0));
        }
    }

    // Plays a sound and disappear
    private IEnumerator Brick()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(source.clip.length - 0.25f);
    }
}
