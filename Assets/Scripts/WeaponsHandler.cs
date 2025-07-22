using System;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponsHandler : MonoBehaviour
{
    public bool facingRight;

    [Header("Primary Weapon")]
    public Weapons primaryWeapon;

    public Vector2[] primaryWeaponHitBox = new Vector2[4];
    
    [Header("Secondary Weapon")]
    public Weapons secondaryWeapon;

    public Vector2[] secondaryWeaponHitBox = new Vector2[4];

    [Header("Sword Parameters")] 
    public float swordHitBoxHeight = 0.5f;

    public float swordHitBoxLength = 1.5f;
    
    [Header("Dagger Parameters")] 
    public float daggerHitBoxHeight = 0.5f;

    public float daggerHitBoxLength = 0.5f;

    public enum Weapons
    {
    sword,
    dagger
    }

    private void Start()
    {
        if (gameObject.CompareTag("Player 1"))
        {
            facingRight = true;
        }
        else if (CompareTag("Player 2"))
        {
            facingRight = false;
        }
        else
        {
            Debug.LogWarning("Weapons handler not attached to player. It is attached to " + gameObject.name);
        }
    }

    public void CalculateHitBox()
    {
        Vector2 position = gameObject.transform.position;
        switch (primaryWeapon)
        {
            case Weapons.sword:
                primaryWeaponHitBox[0] = new Vector2(position.x, position.y + swordHitBoxHeight / 2);
                primaryWeaponHitBox[0] = new Vector2(position.x, position.y - swordHitBoxHeight / 2);
                if (facingRight)
                {
                    primaryWeaponHitBox[0] = new Vector2(position.x + swordHitBoxLength, position.y + swordHitBoxHeight / 2);
                    primaryWeaponHitBox[0] = new Vector2(position.x + swordHitBoxLength, position.y - swordHitBoxHeight / 2);
                }
                else
                {
                    primaryWeaponHitBox[0] = new Vector2(position.x - swordHitBoxLength, position.y + swordHitBoxHeight / 2);
                    primaryWeaponHitBox[0] = new Vector2(position.x - swordHitBoxLength, position.y - swordHitBoxHeight / 2);
                }
                return;
            case Weapons.dagger:
                primaryWeaponHitBox[0] = new Vector2(position.x, position.y + daggerHitBoxHeight / 2);
                primaryWeaponHitBox[0] = new Vector2(position.x, position.y - daggerHitBoxHeight / 2);
                if (facingRight)
                {
                    primaryWeaponHitBox[0] = new Vector2(position.x + daggerHitBoxLength, position.y + daggerHitBoxHeight / 2);
                    primaryWeaponHitBox[0] = new Vector2(position.x + daggerHitBoxLength, position.y - daggerHitBoxHeight / 2);
                }
                else
                {
                    primaryWeaponHitBox[0] = new Vector2(position.x - daggerHitBoxLength, position.y + daggerHitBoxHeight / 2);
                    primaryWeaponHitBox[0] = new Vector2(position.x - daggerHitBoxLength, position.y - daggerHitBoxHeight / 2);
                }
                return;
            return;
        }
    }
}
