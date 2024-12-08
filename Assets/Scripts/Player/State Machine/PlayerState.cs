using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected Player player;
    protected PlayerStateMachine playerStateMachine;

    public PlayerState(Player player, PlayerStateMachine playerStateMachine)
    {
        this.player = player;
        this.playerStateMachine = playerStateMachine;
    }

    public virtual void EnterState()
    {
        playerStateMachine.CurrentPlayerState = this;
    }

    public virtual void ExitState()
    {
        playerStateMachine.CurrentPlayerState = null;
    }

    public virtual void FrameUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }

    public virtual void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
    {

    }
}
