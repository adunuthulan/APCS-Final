using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    public int TrackDist;
    public float velocity = 1f;
    public float distToCollision;
    private Rigidbody rb;
    public LayerMask detectWhat;

    public bool doDamage = true;

    private bool isDead;
    public bool hasAI;
    public bool fPlayer;
    public bool in2D;
    private bool coll;

    private Animator animator;
    private AudioSource source;
    private GameObject pD;

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
        pD = GameObject.FindGameObjectWithTag("Player");

        if (pD == null)
        {
            return;
        }
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
		//if the robot falls off the stage make it come back
		if (transform.position.x < 0) {
			transform.position = new Vector3 (0, transform.position.y, transform.position.z);
		}


		Vector3 pVec = pD.GetComponent<PlayerDim> ().pos;

        //if a robot with ai is too far from player or a robot does not have ai, then use regular movement
		if ((Vector3.Distance(transform.position, new Vector3(transform.position.x, pVec.y, pVec.z)) > TrackDist && hasAI) || !hasAI)
        {

            //changes the sprite depending on the sprite that you have up

            if (fPlayer == true)
            {
                if (animator.GetBool("in3DB") == false && animator.GetBool("in3DF") == true)
                {
                    velocity = 1f;
                }
                else if (animator.GetBool("in3DB") == true && animator.GetBool("in3DF") == false)
                {
                    velocity = -1f;
                }
            }
            fPlayer = false;

            //2D regular movement
            if (in2D)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);
                if ((Colliding() || coll))
                {
                    GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
                    velocity *= -1;
                    coll = false;
                }
            }
            //3D regular movement
            if (!in2D)
            {
                //3D Movement turns robot
                if ((Colliding() || coll))
                {

                    transform.Rotate(new Vector3(0, 180, 0));
                    velocity *= -1;
                    coll = false;
                }

                //changes sprite depending on rotation
                float rot = transform.rotation.eulerAngles.y;

                //fixes rotation issue
                if (Mathf.RoundToInt(rot * Mathf.Pow(10, 5)) == 1)
                {
                    transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
                }

                if (rot == 0)
                {
                    animator.SetBool("in3DB", false);
                    animator.SetBool("in3DF", true);
                }
                else if (Mathf.RoundToInt(rot) == 180)
                {
                    animator.SetBool("in3DF", false);
                    animator.SetBool("in3DB", true);
                }
                else
                {
                    animator.SetBool("in3DF", false);
                    animator.SetBool("in3DB", false);
                }
            }
        }
        //if player has an AI and is close to the player then move towards him
        else
        {
            fPlayer = true;

            //set the velocity depending on location of player relative to robot (Follow Player)
            if (pD.transform.position.z < transform.position.z)
            {
                velocity = -1;
            }
            else
            {
                velocity = 1;
            }

            //if in 3D change the x position of the robot and look at the player
            if (DimensionManager.instance.currentDimension == DimensionManager.Dimension.ThreeD)
            {
                if (in2D)
                    transform.position = Vector3.Lerp(transform.position, pD.GetComponent<PlayerDim>().pos, Time.deltaTime);
                in2D = false;

                var point = pD.transform.position;
                point.y = transform.position.y;
                transform.LookAt(point);

                //changes sprite based on velocity
                if (velocity > 0)
                {
                    animator.SetBool("in3DB", false);
                    animator.SetBool("in3DF", true);
                }
                else if (velocity < 0)
                {
                    animator.SetBool("in3DF", false);
                    animator.SetBool("in3DB", true);
                }
            }
            //if in 2D and targetting player
            else
            {
                if (!in2D)
                {
                    transform.position = Vector3.Lerp(transform.position, new Vector3(0, transform.position.y, transform.position.z), 1);
                    animator.SetBool("in3DB", false);
                    animator.SetBool("in3DF", false);
                }
                in2D = true;
                transform.rotation = Quaternion.Euler(transform.rotation.x, 90 * velocity, 0);

            }
        }

        if ((!in2D && !hasAI) || (Vector3.Distance(transform.position, pD.GetComponent<PlayerDim>().pos) > TrackDist && hasAI) || in2D)
            rb.velocity = new Vector3(0, rb.velocity.y, velocity);
        else
        {
            rb.velocity = new Vector3(pD.GetComponent<PlayerDim>().posX - rb.position.x, rb.velocity.y, velocity);
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
		if (Physics.Raycast(new Vector3(transform.position.x+.5f, transform.position.y, transform.position.z), Vector3.forward * velocity, out hit, distToCollision, detectWhat))
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
                if (doDamage)
                {
                    //if the player hits the side of the enemy make player take damage
                    if (hasAI)
                        collision.gameObject.GetComponent<Player>().TakeDamage(2);
                    else
                        collision.gameObject.GetComponent<Player>().TakeDamage(3);
                }
            }
        }

        //allow enemy to collide with objects
        if (collision.gameObject.tag == "Sides" || collision.gameObject.tag == "EndLevel" || collision.gameObject.layer == 10 || collision.gameObject.layer == 8 || collision.gameObject.layer == 13)
        {
            coll = true;
        }
    }

	//kills the enemy 
    private IEnumerator WaitSec()
    {
        if (!isDead)
        {
            source.PlayOneShot(source.clip);
        }
        isDead = true;
        transform.GetChild(0).GetComponent<Animator>().SetBool("dead", true);
        yield return new WaitForSeconds(0.53f);
        Destroy(this.gameObject);
    }
}
