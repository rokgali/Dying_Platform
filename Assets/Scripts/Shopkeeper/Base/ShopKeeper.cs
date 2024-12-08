using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : IInteractable
{
    public void OnInteraction(IInteractor interactor)
    {
        Debug.Log("I'm opening shop nowe");
    }
}
