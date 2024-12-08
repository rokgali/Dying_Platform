using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PusherController : MonoBehaviour, IWalkingEnemy, IEnemy
{
    [SerializeField]
    private float movementSpeed = 4f;
    Vector3 direction;

    public Vector3 DetectPlayerPosition()
    {
        throw new System.NotImplementedException();
    }

    public void Move()
    {
        throw new System.NotImplementedException();
    }
}
