using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : Unit
{
    [SerializeField] protected UNIT_TYPE type;

    public override void Attack(Unit target)
    {
        base.Attack(target);
    }
}