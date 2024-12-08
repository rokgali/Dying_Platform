using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    Player player;
    [SerializeField]
    private float gravityForce = 9.81f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent <Rigidbody>();
        rb.useGravity = false;
        player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        Vector3 downwardForce = Vector3.down * gravityForce;

        rb.AddForce(downwardForce, ForceMode.Force);
    }
}
