using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable: MonoBehaviour
{
    IInteractor interactor;
    [SerializeField] protected float MaxInteractionDistance;
    protected bool PlayerIsInRangeForTooltipDisplay;
    public abstract void OnInteraction(IInteractor interactor);
    public virtual void OnMouseEnter()
    {
        SetPlayerIsInRangeForTooltipDisplay();
    }
    public virtual void OnMouseOver()
    {
        SetPlayerIsInRangeForTooltipDisplay();
    }
    public abstract void OnMouseExit();

    private void SetPlayerIsInRangeForTooltipDisplay()
    {
        var distance = Vector3.Distance(GameManager.Instance.GetPlayerPosition(), transform.position);

        if (distance <= MaxInteractionDistance)
        {
            PlayerIsInRangeForTooltipDisplay = true;
        }
        else
        {
            PlayerIsInRangeForTooltipDisplay = false;
        }
    }
}
