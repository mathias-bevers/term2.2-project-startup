using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputModuleController : InputModuleBase
{
    protected override void HandleDirection()
    {
        base.HandleDirection(new Vector2(Input.GetAxisRaw(movementAxisHorizontal), Input.GetAxisRaw(movementAxisVertical)));
    }

    protected override void HandleMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        base.HandleMouse(new Vector2(Input.GetAxisRaw(cameraAxisHorizontal) * 3, -Input.GetAxisRaw(cameraAxisVertical) * 3));
    }
}
