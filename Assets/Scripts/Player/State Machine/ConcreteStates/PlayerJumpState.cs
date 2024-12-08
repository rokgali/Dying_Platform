using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJumpState : PlayerCarryingState
{
    public PlayerJumpState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        if(player.IsGrounded)
        {
            player.RB.AddForce(player.JumpForce * player.JumpVelocity, ForceMode.Impulse);
        }
    }

    public override void ExitState()
    {
        base.ExitState();

        player.JumpVelocity = Vector3.zero;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (player.IsGrounded)
        {
            playerStateMachine.ChangeState(player.IdleState);
        }
    }
}
