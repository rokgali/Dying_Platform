using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformDetector : MonoBehaviour
{
    [SerializeField] Player Player;
    // private List<IPickable> _pickableObjectsInRange;
    private void OnTriggerEnter(Collider other)
    {
        var pickableObject = other.GetComponent<IPickable>();

        if(pickableObject != null)
        {
            other.GetComponent<IPickable>().Highlight(true);
            //_pickableObjectsInRange.Add(pickableObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
    }

    private void OnTriggerExit(Collider other)
    {
        var pickableObject = other.GetComponent<IPickable>();

        if (pickableObject != null)
        {
            other.GetComponent<IPickable>().Highlight(false);
            //_pickableObjectsInRange.Remove(other.GetComponent<IPickable>());
        }
    }
}
