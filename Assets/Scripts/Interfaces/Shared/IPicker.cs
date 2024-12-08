using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IPicker
{
    // GameObject StandingOn { get; set; }
    // IPickable ThrowableObject { get; set; }
    HashSet<IPickable> FindPickableObjectsInRange();
    void PickUpPickableObject();
    void OnPickUp(InputAction.CallbackContext ctx);
    void OnPickUpCancel(InputAction.CallbackContext ctx);
    void Carry();
}
