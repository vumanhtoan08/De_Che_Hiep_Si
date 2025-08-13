using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Soldier : Unit
{
    [SerializeField] protected UNIT_TYPE type;

    public override void Attack(Unit target)
    {
        base.Attack(target);
    }

    public override void Move(Vector2 target)
    {
        base.Move(target);  
    }
}
