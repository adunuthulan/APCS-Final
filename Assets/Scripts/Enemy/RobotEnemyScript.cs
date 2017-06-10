using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotEnemyScript : MonoBehaviour {

    public float velocity = 1f;
    public float distToCollision;
    private Rigidbody rb;
    public LayerMask detectWhat;

    public bool isDead;
    public bool coll;
    public bool in2D;

    Animator animator;

    //calls before Start
    void Awake()
    {
        distToCollision = GetComponent<Collider>().bounds.extents.z;
        animator = GetComponent<Animator>();
    }

    // Use this for initialization
    void Start()
    {
        if (GetComponent<Transform>().rotation.eulerAngles.y == 270 || GetComponent<Transform>().rotation.eulerAngles.y == 90)
        {
            in2D = true;
        }
        else
        {
            in2D = false;
        }
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(0, rb.velocity.y, velocity);

        //2D movement
        if (in2D)
        {
            if ((Colliding() || coll))
            {
                GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
                velocity *= -1;
                coll = false;
            }
        }
        if (!in2D)
        {
            //3D Movement

            //turns robot
            if ((Colliding() || coll))
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.eulerAngles.y + 180, transform.rotation.z);
                velocity *= -1;
                coll = false;
            }
        }
    }

    //is colliding with an object in front so it can turn around
    private bool Colliding()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.forward * velocity, out hit, distToCollision, detectWhat))
        {

            return true;
        }

        return false;
    }

    //checks collisions
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // If it's the player and he's on top of the enemy kill this object
            if (collision.gameObject.GetComponent<Collider>().bounds.min.y > transform.position.y && Mathf.Abs(collision.transform.position.x - transform.position.x) < 0.5f && Mathf.Abs(collision.transform.position.z - transform.position.z) < 0.5f)
            {
                velocity = 0;
                StartCoroutine(WaitSec());
            }
            else
            {
                //if the player hits the side of the enemy make player take damage
                collision.gameObject.GetComponent<Player>().TakeDamage(3);
            }
        }

        //if this object collides with the side or another enemy or a block make it turn
        if (collision.gameObject.tag == "Sides" || collision.gameObject.layer == 10 || collision.gameObject.layer == 8)
        {
            coll = true;
            float rot = transform.rotation.eulerAngles.y;

            //changes sprite
            if (!in2D)
            {
                if (rot % 360 == 180)
                {
                    animator.SetBool("in3DB", false);
                    animator.SetBool("in3DF", true);
                }
                if (rot % 360 == 0)
                {
                    animator.SetBool("in3DF", false);
                    animator.SetBool("in3DB", true);
                }
            }
        }
    }

    private IEnumerator WaitSec()
    {
        yield return new WaitForSeconds(0.5f);
        transform.GetChild(0).GetComponent<Animator>().SetBool("dead", true);
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}
