using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon: IInteractable
{
    protected abstract WeaponUser WeaponUser { get; set; }
    protected string WeaponName;
    public abstract int Damage { get; }
    public abstract void Use();
    public abstract void UseSpecial();
    public void OnInteraction(IInteractor interactor)
    {
        WeaponUser foundWeaponUser = interactor.GetGameObject().GetComponent<WeaponUser>();

        if (foundWeaponUser != null)
        {
            WeaponUser = foundWeaponUser;
            WeaponUser.Weapons.Add(this);

            Debug.Log($"Player picked up ${WeaponName}");
        }
        else
        {
            Debug.Log("This guy is not a weapon user, THEREFORE HE'S NOT ALLOWED to wield this weapon");
        }
    }
    public virtual void OnDrop()
    {
        WeaponUser.Weapons.Remove(this);
        WeaponUser = null;

        Debug.Log($"Player dropped ${WeaponName}");
    }
}
