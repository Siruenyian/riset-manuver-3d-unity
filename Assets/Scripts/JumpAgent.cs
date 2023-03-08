using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAgent : MonoBehaviour
{
    // Start is called before the first frame update
    //Keycodes

    [Header("Jump Settings")]

    [Tooltip("Initial jump force")]
    public float jumpForce = 110f;
    [Tooltip("Continuous jump force")]
    public float jumpAccel = 10f;
    [Tooltip("Max jump up time")]
    public float jumpTime = 0.4f;
    [Tooltip("How long you have to jump after leaving a ledge (seconds)")]
    public float coyoteTime = 0.2f;
    [Tooltip("buffer jump input (seconds)")]
    public float jumpBuffer = 0.1f;
    [Tooltip("How long do I have to wait before I can jump again")]
    public float jumpCooldown = 0.6f;
    [Tooltip("Fall quicker")]
    public float extraGravity = 0.1f;
    [Tooltip("The tag that will be considered the ground")]
    public string groundTag = "Ground";


    [Tooltip("The key used to jump")]
    public KeyCode jump = KeyCode.Space;
    [Tooltip("Are we on the ground?")]
    public bool areWeGrounded = true;
    public float currentSpeed=5f;
    Vector3 input = new Vector3();
    private float coyoteTimeCounter, jumpBufferCounter, startJumpTime, endJumpTime;
    private bool wantingToJump = false, jumpCooldownOver = true;
    //Variables
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
       
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        wantingToJump = Input.GetKey(jump);
    }

    private void FixedUpdate()
    {
        //tldr handlehitground set to true
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y - transform.localScale.y + 0.1f, transform.position.z), Vector3.down, 0.1f))
        {
            handleHitGround();
            //Debug.Log("yodayo");
        }
        //Handle late jump like the cartoon coyote
        if (areWeGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;
        if (wantingToJump)
            jumpBufferCounter = jumpBuffer;
        else
            jumpBufferCounter -= Time.deltaTime;

        //there is a problem with coyotetime, where it will go down to minus for some reason
        Debug.Log(coyoteTimeCounter + " " + jumpBufferCounter + " " + jumpCooldownOver);
        
        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && jumpCooldownOver)
        {
            //start jumping mechanism
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            jumpCooldownOver = false;
            areWeGrounded = false;
            jumpBufferCounter = 0f;
            endJumpTime = Time.time + jumpTime;

            Invoke(nameof(jumpCoolDownCountdown), jumpCooldown);
        }
        else if (wantingToJump && !areWeGrounded && endJumpTime > Time.time)
        {
            rb.AddForce(Vector3.up * jumpAccel, ForceMode.Acceleration);
        }
        input = input.normalized;
        Vector3 forwardVel = transform.forward * currentSpeed * input.z;
        Vector3 horizontalVel = transform.right * currentSpeed * input.x;
        rb.velocity = horizontalVel + forwardVel + new Vector3(0, rb.velocity.y, 0);

        rb.AddForce(new Vector3(0, -extraGravity, 0), ForceMode.Impulse);
    }
    private void jumpCoolDownCountdown()
    {
        jumpCooldownOver = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == groundTag)
            handleHitGround();
    }

   
    public void handleHitGround()
    {
        areWeGrounded = true;
    }

    private void OnDrawGizmos()
    {
        Vector3 p = this.transform.position;

        //floor sensors (left, right, back, forward)
        Gizmos.color = Color.magenta;
        Gizmos.DrawRay(new Vector3(p.x - 0.5f, p.y, p.z), Vector3.down + (Vector3.left * 0.5f));
        Gizmos.DrawRay(new Vector3(p.x + 0.5f, p.y, p.z), Vector3.down + (Vector3.right * 0.5f));
        Gizmos.DrawRay(new Vector3(p.x, p.y, p.z - 0.5f), Vector3.down + (Vector3.back * 0.5f));
        Gizmos.DrawRay(new Vector3(p.x, p.y, p.z + 0.5f), Vector3.down + (Vector3.forward * 0.5f));
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y - transform.localScale.y + 0.1f, transform.position.z), Vector3.down);
        // down
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(p, Vector3.down);
    }
    
}
