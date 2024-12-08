using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractor
{
    string Name { get; }
    void Interact(GameObject obj);
    GameObject GetGameObject();
}
