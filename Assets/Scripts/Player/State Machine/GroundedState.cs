using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : PlayerCarryingState
{
    public GroundedState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
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

        if (player.JumpVelocity != Vector3.zero)
        {
            player.StateMachine.ChangeState(player.JumpState);
        }

        if (player.IsPickingUp)
        {
            player.StateMachine.ChangeState(player.PickableState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
