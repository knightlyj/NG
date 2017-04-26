using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public partial class Player
{
    void CheckShoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BattleInfo info = GameObject.FindWithTag("BattleInfo").GetComponent<BattleInfo>();
            info.AddDamageText(transform.position, 100, false);
            //weapon.Shoot(this.Properties, state.targetPos, this.atkLayerMask);
        }
        if (Input.GetMouseButtonDown(1))
        {
            BattleInfo info = GameObject.FindWithTag("BattleInfo").GetComponent<BattleInfo>();
            info.AddDamageText(transform.position, 200, true);
        }
    }
}
