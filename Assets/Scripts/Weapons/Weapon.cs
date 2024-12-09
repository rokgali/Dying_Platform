using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon: Interactable
{
    protected string WeaponName;
    public abstract int Damage { get; }
    public abstract void Use();
    public abstract void UseSpecial();
    private string _message = "Press E to pick up weapon";
    public override void OnInteraction(IInteractor interactor)
    {
        if(interactor is WeaponUser && PlayerIsInRangeForTooltipDisplay)
        {
            WeaponUser weaponUser = (WeaponUser)interactor;

            weaponUser.PickUpWeapon(this);
            TooltipManager.Instance.HideToolTip();
        }
    }
    public virtual void OnDrop()
    {
        Debug.Log($"Player dropped ${WeaponName}");
        this.gameObject.SetActive(true);
        this.gameObject.transform.position = GameManager.Instance.GetPlayerPosition();
    }

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();

        if (PlayerIsInRangeForTooltipDisplay)
        {
            TooltipManager.Instance.SetAndShowTooltip(_message);
        }
    }

    public override void OnMouseOver()
    {
        base.OnMouseOver();

        if(PlayerIsInRangeForTooltipDisplay)
        {
            TooltipManager.Instance.SetAndShowTooltip(_message);
        }
        else
        {
            TooltipManager.Instance.HideToolTip();
        }
    }

    public override void OnMouseExit()
    {
        TooltipManager.Instance.HideToolTip();
    }

    public void HideWeapon()
    {
        gameObject.SetActive(false);
    }
}
