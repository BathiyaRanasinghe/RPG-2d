using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Entity

{

   

    [SerializeField] private float xInput;   

    [Header("MoveInfo")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [Header("Dash Info")]
    [SerializeField] private float dashDuration;
    private float dashTime;
    [SerializeField] private float dashSpeed;

    [SerializeField] private float dashCoolDown;
    private float dashCoolDownTimer;


 

    [Header("AttackInfo")]
    private bool isAttacking;
    private int comboCounter;
    private float comboTimeWindow;
    [SerializeField] private float comboTime = 0.3f;


    protected override void Start(){
        base.Start();
    }

    protected override void Update()
    {
        base.Update();  

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

 

    private void flipController()
    {
        if(rb.velocity.x > 0 && !facingRight) 
            Flip();
        else if(rb.velocity.x < 0 && facingRight) 
            Flip();
    
    }

   
}
