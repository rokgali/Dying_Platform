using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    void Move(Vector3 carryPosition, float carryTimeMilliseconds, float maxElapsedCarryTimeMilliseconds);
    void Launch(Vector3 direction, float launchPower);
    GameObject GetGameObject();
    MeshRenderer GetMeshRenderer();
    Vector3 GetPosition();
    void Highlight(bool highlight);
    bool GetIsHighlighted();
    float GetPickupTimeMilliseconds();
    float GetWeight();
    float GetMaxCarryTimeMilliseconds(float playerStrength);
}
