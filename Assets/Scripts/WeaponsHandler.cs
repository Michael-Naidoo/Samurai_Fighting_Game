using System;
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
                primaryAttackDistance = 1.5f;
                primaryCooldown = 0.25f;
                if (swordObj != null)
                {
                    swordObj.SetActive(true);
                }

                if (daggerObj != null)
                {
                    daggerObj.SetActive(false);
                }

                if (maceObj != null)
                {
                    maceObj.SetActive(false);
                }

                if (bowObj != null)
                {
                    bowObj.SetActive(false);
                }
                break;
            case Weapons.dagger:
                primaryDamage = 0.5f;
                primaryAttackDistance = 1f;
                primaryCooldown = 0.1f;
                if (swordObj != null)
                {
                    swordObj.SetActive(false);
                }

                if (daggerObj != null)
                {
                    daggerObj.SetActive(true);
                }

                if (maceObj != null)
                {
                    maceObj.SetActive(false);
                }

                if (bowObj != null)
                {
                    bowObj.SetActive(false);
                }
                break;
            case Weapons.mace:
                primaryDamage = 1.5f;
                primaryAttackDistance = 2f;
                primaryCooldown = 0.5f;
                if (swordObj != null)
                {
                    swordObj.SetActive(false);
                }

                if (daggerObj != null)
                {
                    daggerObj.SetActive(false);
                }

                if (maceObj != null)
                {
                    maceObj.SetActive(true);
                }

                if (bowObj != null)
                {
                    bowObj.SetActive(false);
                }
                break;
            case Weapons.bow:
                primaryDamage = 0.75f;
                primaryAttackDistance = 10f;
                primaryCooldown = 0.15f;
                if (swordObj != null)
                {
                    swordObj.SetActive(false);
                }

                if (daggerObj != null)
                {
                    daggerObj.SetActive(false);
                }

                if (maceObj != null)
                {
                    maceObj.SetActive(false);
                }

                if (bowObj != null)
                {
                    bowObj.SetActive(true);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        switch (secondaryWeapon)
        {
            case Weapons.sword:
                secondaryDamage = 1f;
                secondaryAttackDistance = 1.5f;
                secondaryCooldown = 0.25f;
                if (swordObj != null)
                {
                    swordObj.SetActive(true);
                }

                if (daggerObj != null)
                {
                    daggerObj.SetActive(false);
                }

                if (maceObj != null)
                {
                    maceObj.SetActive(false);
                }

                if (bowObj != null)
                {
                    bowObj.SetActive(false);
                }
                break;
            case Weapons.dagger:
                secondaryDamage = 0.5f;
                secondaryAttackDistance = 1f;
                secondaryCooldown = 0.1f;
                if (swordObj != null)
                {
                    swordObj.SetActive(false);
                }

                if (daggerObj != null)
                {
                    daggerObj.SetActive(true);
                }

                if (maceObj != null)
                {
                    maceObj.SetActive(false);
                }

                if (bowObj != null)
                {
                    bowObj.SetActive(false);
                }
                break;
            case Weapons.mace:
                secondaryDamage = 1.5f;
                secondaryAttackDistance = 2f;
                secondaryCooldown = 0.5f;
                if (swordObj != null)
                {
                    swordObj.SetActive(false);
                }

                if (daggerObj != null)
                {
                    daggerObj.SetActive(false);
                }

                if (maceObj != null)
                {
                    maceObj.SetActive(true);
                }

                if (bowObj != null)
                {
                    bowObj.SetActive(false);
                }
                break;
            case Weapons.bow:
                secondaryDamage = 0.75f;
                secondaryAttackDistance = 10f;
                secondaryCooldown = 0.15f;
                if (swordObj != null)
                {
                    swordObj.SetActive(false);
                }

                if (daggerObj != null)
                {
                    daggerObj.SetActive(false);
                }

                if (maceObj != null)
                {
                    maceObj.SetActive(false);
                }

                if (bowObj != null)
                {
                    bowObj.SetActive(true);
                }
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
                    if (swordObj != null)
                    {
                        swordObj.SetActive(true);
                    }

                    if (daggerObj != null)
                    {
                        daggerObj.SetActive(false);
                    }

                    if (maceObj != null)
                    {
                        maceObj.SetActive(false);
                    }

                    if (bowObj != null)
                    {
                        bowObj.SetActive(false);
                    }
                    break;
                case Weapons.dagger:
                    if (swordObj != null)
                    {
                        swordObj.SetActive(false);
                    }

                    if (daggerObj != null)
                    {
                        daggerObj.SetActive(true);
                    }

                    if (maceObj != null)
                    {
                        maceObj.SetActive(false);
                    }

                    if (bowObj != null)
                    {
                        bowObj.SetActive(false);
                    }
                    break;
                case Weapons.mace:
                    if (swordObj != null)
                    {
                        swordObj.SetActive(false);
                    }

                    if (daggerObj != null)
                    {
                        daggerObj.SetActive(false);
                    }

                    if (maceObj != null)
                    {
                        maceObj.SetActive(true);
                    }

                    if (bowObj != null)
                    {
                        bowObj.SetActive(false);
                    }
                    break;
                case Weapons.bow:
                    if (swordObj != null)
                    {
                        swordObj.SetActive(false);
                    }

                    if (daggerObj != null)
                    {
                        daggerObj.SetActive(false);
                    }

                    if (maceObj != null)
                    {
                        maceObj.SetActive(false);
                    }

                    if (bowObj != null)
                    {
                        bowObj.SetActive(true);
                    }
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
                    if (swordObj != null)
                    {
                        swordObj.SetActive(true);
                    }

                    if (daggerObj != null)
                    {
                        daggerObj.SetActive(false);
                    }

                    if (maceObj != null)
                    {
                        maceObj.SetActive(false);
                    }

                    if (bowObj != null)
                    {
                        bowObj.SetActive(false);
                    }
                    break;
                case Weapons.dagger:
                    if (swordObj != null)
                    {
                        swordObj.SetActive(false);
                    }

                    if (daggerObj != null)
                    {
                        daggerObj.SetActive(true);
                    }

                    if (maceObj != null)
                    {
                        maceObj.SetActive(false);
                    }

                    if (bowObj != null)
                    {
                        bowObj.SetActive(false);
                    }
                    break;
                case Weapons.mace:
                    if (swordObj != null)
                    {
                        swordObj.SetActive(false);
                    }

                    if (daggerObj != null)
                    {
                        daggerObj.SetActive(false);
                    }

                    if (maceObj != null)
                    {
                        maceObj.SetActive(true);
                    }

                    if (bowObj != null)
                    {
                        bowObj.SetActive(false);
                    }
                    break;
                case Weapons.bow:
                    if (swordObj != null)
                    {
                        swordObj.SetActive(false);
                    }

                    if (daggerObj != null)
                    {
                        daggerObj.SetActive(false);
                    }

                    if (maceObj != null)
                    {
                        maceObj.SetActive(false);
                    }

                    if (bowObj != null)
                    {
                        bowObj.SetActive(true);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        Debug.Log("Damage is " + gameObject.GetComponent<PlayerMovement>().weaponDamage +  
        ", Attack Distance is " + gameObject.GetComponent<PlayerMovement>().attackDistance + 
        ", Cooldown is " + gameObject.GetComponent<PlayerMovement>().currentCooldown);
    }
}