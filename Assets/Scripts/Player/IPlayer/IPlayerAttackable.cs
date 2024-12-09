using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IPlayerAttackable
{
    void OnShoot(InputAction.CallbackContext ctx);
    void OnShootCancel(InputAction.CallbackContext ctx);
    void LaunchPickableObject(Vector3 launchDirection);
}
