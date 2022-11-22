using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementPlayer))]
public class HarpoonedMovement : HarpoonInfoRelay
{

    MovementPlayer player;

    private void Start()
    {
        player = GetComponent<MovementPlayer>();
    }


    private void Update()
    {
        if (info == null) return;
        player.LookAtProper(info.harpoonShooter.transform);
    }
}
