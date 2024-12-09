using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardStaff : RangedWeapon
{
    [field: SerializeField] public override float Range { get; }

    [field: SerializeField] public override float FireRate { get; }

    [field: SerializeField] public override int AmmoCapacity { get; }

    [field: SerializeField] public override int MaxAmmo { get; }

    [field:SerializeField] public override int Damage { get; }
    public override void OnDrop()
    {
        base.OnDrop();
    }

    public override void Shoot()
    {
        Debug.Log("Shooting wizard staff!");
    }

    public override void SpecialShoot()
    {
        Debug.Log("Wizard staff special ability!");
    }

    public override void Use() => Shoot();

    public override void UseSpecial() => SpecialShoot();
}
