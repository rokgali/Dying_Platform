using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCarryingState : PlayerShootingState
{
    public PlayerCarryingState(Player player, PlayerStateMachine playerStateMachine) : base(player, playerStateMachine)
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if(player.PickedUpObjects.Count > 0)
        {
            foreach (var pickedUpObject in player.PickedUpObjects)
            {
                pickedUpObject.GetGameObject().transform.position = player.CarryingPosition.position;
            }
        }
    }
}
