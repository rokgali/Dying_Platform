using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponUser : MonoBehaviour
{
    public abstract List<Weapon> Weapons { get; }
    public virtual void PickUpWeapon(Weapon weapon)
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, Input.mousePosition, out hit, 20))
        {
            Debug.DrawRay(transform.position, Input.mousePosition * hit.distance, Color.red);

            if(hit.transform.gameObject.GetComponent<Weapon>() != null)
            {
                Weapons.Add(weapon);
            }
            else
            {
                Debug.Log("Dis not wepon");
            }
        }
    }
    public virtual void DropWeapon(Weapon weapon)
    {
        weapon.OnDrop();
    }

}
