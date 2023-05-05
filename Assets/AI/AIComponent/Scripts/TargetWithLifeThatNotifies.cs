using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetWithLifeThatNotifies : TargetWithLife
{
    public interface IDeathNotifiable
    {
        public void NotifyDeath();
    }

    protected override void CheckStillAlive()
    {
        base.CheckStillAlive();
        if (life <= 0f)
        { GetComponent<IDeathNotifiable>().NotifyDeath(); }
    }

    protected override bool DestroyOnAllLifeLost()
    {
        return false;
    }
}
