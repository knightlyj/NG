using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public partial class Player : Entity
{
    public Transform WeaponPrefab;
    Weapon weapon = null;
    void ChangeWeapon()
    {
        Transform mainHand = transform.FindChild("Body").FindChild("MainUpperArm").FindChild("MainHand");
        Transform trWeapon = Instantiate(WeaponPrefab, mainHand) as Transform;
        trWeapon.localPosition = new Vector3(0.079f, -0.113f, 0.1f);
        trWeapon.localRotation = Quaternion.Euler(0, 0, -80);
        trWeapon.localScale = Vector3.one;
        weapon = trWeapon.GetComponent<Weapon>();
        weapon.apprearance = Weapon.AppearanceType.TestGun;
        weapon.shotType = Weapon.ShotType.Pistol;
        //weapon.apprearance = Weapon.AppearanceType.TestGun;
        //weapon.shotType = Weapon.ShotType.Projectile;
        //weapon.projectile = Spawner.ProjectileType.BaseRocket;
    }

    void CheckShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            weapon.Shoot(this.Properties, state.targetPos, this.atkLayerMask);
        }
        if (Input.GetMouseButtonDown(1))
        {

        }
    }
}
