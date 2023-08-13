using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;

    private string animBoolName;

    public PlayerState(Player _player, PlayerStateMachine _statemachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _statemachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
    }

    public virtual void Update(){
        Debug.Log("I entered " + animBoolName);
    }


    public virtual void Exit()
    {
       player.anim.SetBool(animBoolName, false);
    }
}
