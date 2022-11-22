using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputModuleController : InputModuleBase
{
    protected override void HandleDirection()
    {
        base.HandleDirection(new Vector2(Input.GetAxisRaw("HorizontalJoy"), Input.GetAxisRaw("VerticalJoy")));
    }

    protected override void HandleMouse()
    {
        base.HandleMouse(new Vector2(Input.GetAxisRaw("CameraX"), -Input.GetAxisRaw("CameraY")));
    }
}
