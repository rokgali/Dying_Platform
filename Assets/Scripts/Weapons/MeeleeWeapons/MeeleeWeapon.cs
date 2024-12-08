using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeeleeWeapon : Weapon
{
    public abstract float SwingRate { get; }
    public abstract void Swing();
    public abstract void SpecialSwing();
}
