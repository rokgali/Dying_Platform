using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : GroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Player.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        // Debug.Log("moving");
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Vector3 previousMoveDirection = Vector3.zero;

        if (player.MoveInput == Vector3.zero)
        {
            playerStateMachine.ChangeState(player.IdleState);
        }

        Vector3 cameraFowardDirection = player.Camera.transform.forward;
        Vector3 cameraRightDirection = player.Camera.transform.right;

        cameraFowardDirection.y = 0;
        cameraRightDirection.y = 0;

        cameraFowardDirection.Normalize();
        cameraRightDirection.Normalize();

        Vector3 moveDirection = (cameraRightDirection * player.MoveInput.x + cameraFowardDirection * player.MoveInput.z).normalized;
        moveDirection.y = 0;
        Vector3 move = moveDirection * player.MovementSpeed;

        if (moveDirection != Vector3.zero)
        {
            player.RB.AddForce(move, ForceMode.Force);
            player.SetPlayerFacingDirection(moveDirection);
        }

        if (previousMoveDirection != moveDirection)
        {
            previousMoveDirection = moveDirection;

            if (Vector3.Dot(previousMoveDirection, moveDirection) < 0)
            {
                player.RB.velocity = Vector3.zero;
            }
        }
    }
}
