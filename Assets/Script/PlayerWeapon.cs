using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerWeapon
{
    public string Name = "WeaponName";

    public float Damage = 10f;

    public float Range = 100f;

    public GameObject Graphics;

    public float FireRate = 0f;

    public float ReloadTime = 1;

    public int MaxBullet = 20;

    [HideInInspector]
    public int Bullets;
    
    public PlayerWeapon()
    {
        Bullets = MaxBullet;
    }
}

