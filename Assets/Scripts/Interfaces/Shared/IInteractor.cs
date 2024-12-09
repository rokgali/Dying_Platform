using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractor
{
    void Interact(Interactable interactableObj);
    GameObject GetGameObject();
}
