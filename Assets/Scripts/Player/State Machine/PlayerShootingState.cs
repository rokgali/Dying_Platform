using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingState : PlayerState
{
    public PlayerShootingState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }
    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if(player.FiringDirection != Vector3.zero)
        {
            Debug.Log("Firing in " + player.FiringDirection);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
