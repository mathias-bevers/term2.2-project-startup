using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputModuleKeyboardAndMouse : InputModuleBase
{
    protected override void HandleDirection()
    {
        base.HandleDirection(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));
    }

    protected override void HandleMouse()
    {
        base.HandleMouse(new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y")));
    }
}
