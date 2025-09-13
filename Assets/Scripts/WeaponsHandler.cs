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
    private float secondaryDamage;
    private float primaryDamage;

    [SerializeField] private GameObject swordObj;
    [SerializeField] private GameObject daggerObj;
    [SerializeField] private GameObject maceObj;
    [SerializeField] private GameObject bowObj;

    public enum Weapons
    {
        sword = 1,
        dagger = 2,
        mace = 3,
        bow = 4
    }

    private void Start()
    {
        switch (primaryWeapon)
        {
            case Weapons.sword:
                primaryDamage = 1f;
                primaryAttackDistance = 2f;
                primaryCooldown = 0.25f;
                swordObj.SetActive(true);
                daggerObj.SetActive(false);
                maceObj.SetActive(false);
                bowObj.SetActive(false);
                break;
            case Weapons.dagger:
                primaryDamage = 0.5f;
                primaryAttackDistance = 1f;
                primaryCooldown = 0.1f;
                swordObj.SetActive(false);
                daggerObj.SetActive(true);
                maceObj.SetActive(false);
                bowObj.SetActive(false);
                break;
            case Weapons.mace:
                primaryDamage = 1.5f;
                primaryAttackDistance = 1.5f;
                primaryCooldown = 0.5f;
                swordObj.SetActive(false);
                daggerObj.SetActive(false);
                maceObj.SetActive(true);
                bowObj.SetActive(false);
                break;
            case Weapons.bow:
                primaryDamage = 0.75f;
                primaryAttackDistance = 10f;
                primaryCooldown = 0.15f;
                swordObj.SetActive(false);
                daggerObj.SetActive(false);
                maceObj.SetActive(false);
                bowObj.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        switch (secondaryWeapon)
        {
            case Weapons.sword:
                secondaryDamage = 1f;
                secondaryAttackDistance = 2f;
                secondaryCooldown = 0.25f;
                swordObj.SetActive(true);
                daggerObj.SetActive(false);
                maceObj.SetActive(false);
                bowObj.SetActive(false);
                break;
            case Weapons.dagger:
                secondaryDamage = 0.5f;
                secondaryAttackDistance = 1f;
                secondaryCooldown = 0.1f;
                swordObj.SetActive(false);
                daggerObj.SetActive(true);
                maceObj.SetActive(false);
                bowObj.SetActive(false);
                break;
            case Weapons.mace:
                secondaryDamage = 1.5f;
                primaryAttackDistance = 1.5f;
                primaryCooldown = 0.5f;
                swordObj.SetActive(false);
                daggerObj.SetActive(false);
                maceObj.SetActive(true);
                bowObj.SetActive(false);
                break;
            case Weapons.bow:
                secondaryDamage = 0.75f;
                primaryAttackDistance = 10f;
                primaryCooldown = 0.15f;
                swordObj.SetActive(false);
                daggerObj.SetActive(false);
                maceObj.SetActive(false);
                bowObj.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void SwitchWeapon()
    {
        Debug.Log("switch");
        if (usingPrimaryWeapon)
        {
            gameObject.GetComponent<PlayerMovement>().weaponDamage = secondaryDamage;
            gameObject.GetComponent<PlayerMovement>().attackDistance = secondaryAttackDistance;
            gameObject.GetComponent<PlayerMovement>().currentCooldown = secondaryCooldown;
            usingPrimaryWeapon = false;
            switch (secondaryWeapon)
            {
                case Weapons.sword:
                    swordObj.SetActive(true);
                    daggerObj.SetActive(false);
                    maceObj.SetActive(false);
                    bowObj.SetActive(false);
                    break;
                case Weapons.dagger:
                    swordObj.SetActive(false);
                    daggerObj.SetActive(true);
                    maceObj.SetActive(false);
                    bowObj.SetActive(false);
                    break;
                case Weapons.mace:
                    swordObj.SetActive(false);
                    daggerObj.SetActive(false);
                    maceObj.SetActive(true);
                    bowObj.SetActive(false);
                    break;
                case Weapons.bow:
                    swordObj.SetActive(false);
                    daggerObj.SetActive(false);
                    maceObj.SetActive(false);
                    bowObj.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        else if (!usingPrimaryWeapon)
        {
            gameObject.GetComponent<PlayerMovement>().weaponDamage = primaryDamage;
            gameObject.GetComponent<PlayerMovement>().attackDistance = primaryAttackDistance;
            gameObject.GetComponent<PlayerMovement>().currentCooldown = primaryCooldown;
            usingPrimaryWeapon = true;   
            switch (primaryWeapon)
            {
                case Weapons.sword:
                    swordObj.SetActive(true);
                    daggerObj.SetActive(false);
                    maceObj.SetActive(false);
                    bowObj.SetActive(false);
                    break;
                case Weapons.dagger:
                    swordObj.SetActive(false);
                    daggerObj.SetActive(true);
                    maceObj.SetActive(false);
                    bowObj.SetActive(false);
                    break;
                case Weapons.mace:
                    swordObj.SetActive(false);
                    daggerObj.SetActive(false);
                    maceObj.SetActive(true);
                    bowObj.SetActive(false);
                    break;
                case Weapons.bow:
                    swordObj.SetActive(false);
                    daggerObj.SetActive(false);
                    maceObj.SetActive(false);
                    bowObj.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}