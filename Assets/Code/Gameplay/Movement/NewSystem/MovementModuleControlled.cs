using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputModule))]
public class MovementModuleControlled : MovementModule
{
    public InputModule inputModule { get; set; }

    internal bool hasInputModule { get; private set; }

    protected override void OnStart()
    {
        inputModule = GetComponent<InputModule>();
        base.OnStart();
    }

    protected override void Tick()
    {
        hasInputModule = inputModule != null;
        base.Tick();
    }
}
