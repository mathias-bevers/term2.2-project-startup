using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementPlayer))]
public class HarpoonedMovement : HarpoonInfoRelay
{

    MovementPlayer player;

    [SerializeField] float harpoonedSpeed = 1.5f;

    private void Start()
    {
        player = GetComponent<MovementPlayer>();
    }


    private void Update()
    {
        if (info == null) return;
        player.LookAtProper(info.harpoon.shotFrom.transform);

        Vector3 inputDirection = harpoonedSpeed * player.inputModule.directionalInputNormalized * Time.deltaTime;

        player.controller.Move(transform.forward * inputDirection.y);
        player.controller.Move(transform.right * inputDirection.x);
    }
}
