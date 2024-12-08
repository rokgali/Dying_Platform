using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangedWeapon : Weapon
{
    public abstract float Range { get; }
    public abstract float FireRate { get; }
    public abstract int AmmoCapacity { get; }
    public abstract int MaxAmmo { get; }
    public abstract void Shoot();
    public abstract void SpecialShoot();
}
