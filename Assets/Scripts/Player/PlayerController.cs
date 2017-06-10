using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInputManager;

public class PlayerController : MonoBehaviour {

    public float moveForce = 25f;
    public float jumpForce = 250f;

    public LayerMask allowedCollisions;

    public LayerMask notJumpingCollisions;

    private float jump;
    private Vector3 velocity;

    private bool grounded;
    private float distToGround;

    private Rigidbody rb;
    private Animator animator;

    public float boostFactor;

    private bool in2D;

	public AudioSource source;
	public AudioClip boostSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        distToGround = GetComponent<Collider>().bounds.extents.y + .156f;
        boostFactor = 1;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Check if the player is on the ground
        grounded = (Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f, allowedCollisions));
        //grounded = CollisionOnCorners(transform.position, allowedCollisions);

        //checks to see if the player is not in the air
        if (!grounded)
        {
            animator.SetBool("Jump", false);
        }

        // Check if the player presses jump and is grounded
        if (CustomInput.OnKey("Jump") && grounded)
        {
            jump = jumpForce * boostFactor;
            animator.SetBool("Jump", true);
            //Animator[] a = animator.GetAnimationClips (GameObject);
            //a["Jump"].speed = 
        }

        // Check for player pressing the movement keys (wasd and arrow keys)
        float moveH = CustomInput.OnAxis("Horizontal");
        float moveV = CustomInput.OnAxis("Vertical");

        //changes sprite for movement
        if (moveH > 0 || moveV < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            if (grounded)
            {
                animator.SetBool("WalkingL", false);
                animator.SetBool("WalkingR", true);
            }
        }
        else if (moveH < 0 || moveV > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            if (grounded)
            {
                animator.SetBool("WalkingR", false);
                animator.SetBool("WalkingL", true);
            }
        }
        else
        {
            if (grounded)
            {
                animator.SetBool("WalkingR", false);
                animator.SetBool("WalkingL", false);
            }
        }

        // Modify what each key controls based on current dimension (left and right keys should always go left and right, whatever the dimension is)
        if (DimensionManager.instance.currentDimension == DimensionManager.Dimension.ThreeD)
        {
            if (in2D == true)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
            }
            in2D = false;
            velocity = new Vector3(moveH, 0.0f, moveV).normalized * boostFactor;
        }
        else
        {
            if (in2D == false)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);
            }
            in2D = true;
            velocity = new Vector3(0.0f, 0.0f, -moveH).normalized * boostFactor;
        }
    }

    private void FixedUpdate()
    {
        if (CheckMovingDirection(transform.position, notJumpingCollisions, velocity))
        {
            jump = 0f;
        }

        // If the player is trying to jump, add that upward force
        if (jump != 0f)
        {
            //rb.AddForce(Vector3.up * jump, ForceMode.Acceleration);
            rb.velocity = rb.velocity + (Vector3.up * jumpForce);
            jump = 0f;

        }

        if (!grounded)
            velocity = velocity / 2;

        // If the player is trying to move, then move
        if (velocity != Vector3.zero)
        {
            //RaycastHit hit;
            //if (rb.SweepTest(velocity, out hit))
            rb.MovePosition(rb.position + (velocity * moveForce * Time.fixedDeltaTime));
            //rb.AddForce(new Vector3(velocity.x * moveForce, rb.velocity.y, velocity.z * moveForce), ForceMode.Force);
        }
        else
        {
            //rb.AddForce(new Vector3(0.0f, rb.velocity.y, 0.0f), ForceMode.Force);
            rb.velocity = new Vector3(0.0f, rb.velocity.y, 0.0f);
        }


        //rb.velocity = rb.velocity + (movements * moveForce);
    }

    // Method to stop all movements for a while
    [ContextMenu("Stop Movements")]
    public void StopMovements()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        rb.velocity = Vector3.zero;
    }

    // Not Used Anymore (Keeping it because it might be useful later?)
    // Checks all four corners if there is a collision under the player
    public bool CollisionOnCorners(Vector3 basePosition, LayerMask mask)
    {
        bool corner1 = Physics.Raycast(new Vector3(basePosition.x + distToGround, basePosition.y, basePosition.z + distToGround), Vector3.down, distToGround + 0.1f, mask);
        bool corner2 = Physics.Raycast(new Vector3(basePosition.x - distToGround, basePosition.y, basePosition.z + distToGround), Vector3.down, distToGround + 0.1f, mask);
        bool corner3 = Physics.Raycast(new Vector3(basePosition.x + distToGround, basePosition.y, basePosition.z - distToGround), Vector3.down, distToGround + 0.1f, mask);
        bool corner4 = Physics.Raycast(new Vector3(basePosition.x - distToGround, basePosition.y, basePosition.z - distToGround), Vector3.down, distToGround + 0.1f, mask);

        return (corner1 || corner2 || corner3 || corner4);
    }

    public bool CheckMovingDirection(Vector3 basePos, LayerMask mask, Vector3 direction)
    {
        return Physics.Raycast(basePos, direction, distToGround, mask);
    }

    // Start the speed boost
    public void EnableSpeedBoost(float time, float factor)
    {
		source.PlayOneShot (boostSound);
        StartCoroutine(SpeedBoost(time, factor));
    }

    // Start speed and jump boost, wait for pickupTime, then disable it
    private IEnumerator SpeedBoost(float pickupTime, float speedBoostFactor)
    {



        boostFactor = speedBoostFactor;

        float x = pickupTime;

        PlayerUI.instance.powerTime = x;

        while (x > 0)
        {
            x -= .1f;

            //update UI
            PlayerUI.instance.powerTime = x;

            yield return new WaitForSeconds(.1f);
        }

        PlayerUI.instance.powerTime = 0.0f;
        boostFactor = 1;
    }
}