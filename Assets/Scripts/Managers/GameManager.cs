using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject platformPrefab;
    private GameObject currentObject;
    private Floor floor;
    [SerializeField]
    private PlayerController player;
    private float raycastDistance = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        // FloorGenerator platformGenerator = new FloorGenerator(platformPrefab);
        // floor = platformGenerator.CreateRectangleFloor(new Vector3(-15, 0, -15), 30, 30);
    }

    private void Update()
    {
    }
}
