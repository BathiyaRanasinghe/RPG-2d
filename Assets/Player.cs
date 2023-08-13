using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header ("Move Info")]
    public float moveSpeed = 12f;
    public float jumpForce = 12f;

    [Header ("Collison Info")]
    [SerializeField] private Transform groundCheck; 
    [SerializeField] private float groundCheckDistance; 

    [SerializeField] private Transform wallCheck; 
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    public int facingDir { get; private set; } = 1;
    private bool facingRight = true;


 
   #region Commponenets
   public Animator anim { get; private set;}
   public Rigidbody2D rb { get; private set; }
   public PlayerStateMachine stateMachine { get; private set; }
   #endregion
   #region States 
   public PlayerIdleState idleState { get; private set; }
   public PlayerMoveState moveState { get; private set; }
   public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
   #endregion


   private void Awake() {
    stateMachine = new PlayerStateMachine();

    idleState = new PlayerIdleState(this, stateMachine, "Idle");
    moveState = new PlayerMoveState(this, stateMachine, "Move");
    jumpState = new PlayerJumpState(this, stateMachine, "Jump");
    airState = new PlayerAirState(this, stateMachine, "Jump");
   }

   private void Start() {
       anim = GetComponentInChildren<Animator>();
       rb = GetComponent<Rigidbody2D>();

       stateMachine.Initialize(idleState);

   }

   private void Update() {
       stateMachine.currentState.Update();
   }

   public void SetVelocity(float _xvelocity, float _yvelocity) {
       rb.velocity = new Vector2(_xvelocity, _yvelocity);
       FlipController(_xvelocity);
   }

   public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);


   private void OnDrawGizmos() {
         Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y-groundCheckDistance));

         Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
   }

    public void Flip() {

         facingDir = facingDir * -1;
         facingRight = !facingRight;
         transform.Rotate(0f, 180f, 0f);
         
    }

    public void FlipController(float _x) {
        if(_x > 0 && !facingRight)
        {
            Flip();
        }
        else if(_x < 0 && facingRight)
        {
            Flip();
        }
    }





}

  