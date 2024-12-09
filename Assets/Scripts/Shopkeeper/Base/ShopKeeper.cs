using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : Interactable
{
    public override void OnInteraction(IInteractor interactor)
    {
        Debug.Log("I'm opening shop nowe");
    }

    public override void OnMouseEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void OnMouseExit()
    {
        throw new System.NotImplementedException();
    }

    public override void OnMouseOver()
    {
        throw new System.NotImplementedException();
    }
}
