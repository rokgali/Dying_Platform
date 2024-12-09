using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponUser : MonoBehaviour
{
    public abstract WeaponMachine WeaponMachine { get; set; } 
    public abstract List<Weapon> Weapons { get;}
    public abstract void PickUpWeapon(Weapon weapon);
    public virtual void DropWeapon()
    {
        var currentWeapon = WeaponMachine.CurrentWeapon;

        Weapons.Remove(currentWeapon);

        currentWeapon.OnDrop();
    }
}
