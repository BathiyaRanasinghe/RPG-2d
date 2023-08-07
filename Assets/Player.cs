using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour

{
    private Rigidbody2D rb;
    private Animator anim;
    private int facingDir = 1;
    private bool facingRight = true;
    private bool isGrounded;

    [SerializeField] private float xInput;   
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [Header("Dash Info")]
    [SerializeField] private float dashDuration;
    private float dashTime;
    [SerializeField] private float dashSpeed;

    [SerializeField] private float dashCoolDown;
    private float dashCoolDownTimer;


    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    [Header("AttackInfo")]
    private bool isAttacking;
    private int comboCounter;
    private float comboTimeWindow;
    [SerializeField] private float comboTime = 0.3f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Movement();
        CheckInput();
        AnimatorControllers();
        flipController();
        CollisionChecks();

        dashTime -= Time.deltaTime;
        dashCoolDownTimer -= Time.deltaTime;
        comboTimeWindow -= Time.deltaTime;

        

    }

    public void AttackOver() {
        isAttacking = false;
        comboCounter++;

        if (comboCounter > 2) {
            comboCounter = 0;
        }
         
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void CheckInput()
    {
        xInput = Input.GetAxis("Horizontal");
        
         if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartAttackEvent();
        }



        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashAbility();
        }


    }

    private void StartAttackEvent()
    {
        if (!isGrounded)
        return;

        if (comboTimeWindow < 0)
        {
            comboCounter = 0;
        }
        isAttacking = true;
        comboTimeWindow = comboTime;
    }

    private void DashAbility()
    {
        if (dashCoolDownTimer < 0 && !isAttacking)
        {
        dashCoolDownTimer = dashCoolDown;
        dashTime = dashDuration;
        }
    }

    private void Movement()
    {

        if (isAttacking)
        {
            rb.velocity = new Vector2(0,0);
        }

        else if (dashTime > 0) {
            rb.velocity = new Vector2(facingDir * dashSpeed, 0);
        }
        else {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
        }
    }

    private void Jump()
    {
        if (isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void AnimatorControllers()
    {
    
       bool isMoving = rb.velocity.x != 0;
       anim.SetFloat("yVelocity", rb.velocity.y);

       anim.SetBool("isMoving", isMoving);
       anim.SetBool("isGrounded", isGrounded);
       anim.SetBool("isDashing", dashTime > 0);
       anim.SetBool("isAttacking", isAttacking);
       anim.SetInteger("comboCounter", comboCounter);

    }

    public void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
    }

    private void flipController()
    {
        if(rb.velocity.x > 0 && !facingRight) 
            Flip();
        else if(rb.velocity.x < 0 && facingRight) 
            Flip();
    
    }

    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
