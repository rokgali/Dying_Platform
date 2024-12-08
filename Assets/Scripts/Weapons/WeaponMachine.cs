using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMachine : MonoBehaviour
{
    public Weapon CurrentWeapon { get; private set; }

    public void ChangeWeapon(Weapon weapon)
    {
        CurrentWeapon = weapon;
    }
}
