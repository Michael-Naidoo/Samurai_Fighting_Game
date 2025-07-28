using System;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponsHandler : MonoBehaviour
{
    [Header("Primary Weapon")] public Weapons primaryWeapon;

    public float primaryAttackDistance;

    public float primaryCooldown;

    [Header("Secondary Weapon")] public Weapons secondaryWeapon;

    public float secondaryAttackDistance;

    public float secondaryCooldown;

    public bool usingPrimaryWeapon = true;
    
    public enum Weapons
    {
        sword = 1,
        dagger = 2
    }

    private void Start()
    {
        switch (primaryWeapon)
        {
            case Weapons.sword:
                primaryAttackDistance = 2f;
                primaryCooldown = 0.25f;
                break;
            case Weapons.dagger:
                primaryAttackDistance = 1f;
                primaryCooldown = 0.1f;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        switch (secondaryWeapon)
        {
            case Weapons.sword:
                secondaryAttackDistance = 2f;
                secondaryCooldown = 0.25f;
                break;
            case Weapons.dagger:
                secondaryAttackDistance = 1f;
                secondaryCooldown = 0.1f;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SwitchWeapon()
    {
        if (usingPrimaryWeapon)
        {
            gameObject.GetComponent<PlayerMovement>().attackDistance = secondaryAttackDistance;
            gameObject.GetComponent<PlayerMovement>().currentCooldown = secondaryCooldown;
            usingPrimaryWeapon = false;
        }
        else if (!usingPrimaryWeapon)
        {
            gameObject.GetComponent<PlayerMovement>().attackDistance = primaryAttackDistance;
            gameObject.GetComponent<PlayerMovement>().currentCooldown = primaryCooldown;
            usingPrimaryWeapon = true;   
        }
    }
}