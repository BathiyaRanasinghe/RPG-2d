using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header ("Move Info")]
    public float moveSpeed = 12f;
    public float jumpForce = 12f;
 
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
   }



}

  