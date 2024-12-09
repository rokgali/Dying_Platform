using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayerMoveable
{
    //Rigidbody RB { get; set; }
    //Vector3 FacingDirection { get; set; }
    //bool IsGrounded { get; set; }
    void SetPlayerFacingDirection(Vector3 velocity);
    void OnPlayerIsGrounded();
    void OnMove(InputAction.CallbackContext ctx);
    void OnMoveCancel(InputAction.CallbackContext ctx);
    void OnJump(InputAction.CallbackContext ctx);
    // void OnJumpCancel(InputAction.CallbackContext ctx);
}
