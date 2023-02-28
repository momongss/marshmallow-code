
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem_Speed : SpawnItem
{
    public override void Spawn(float ratio, float duration)
    {
        base.Spawn(ratio, duration);


    }

    public override void Take()
    {
        print("Speed Up");
        PlayerSquad.I.SpeedUP(ratio, duration);

        base.Take();
    }
}
