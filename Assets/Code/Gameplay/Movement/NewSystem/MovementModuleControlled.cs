using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputModuleBase))]
public class MovementModuleControlled : MovementModule
{
    public InputModuleBase inputModule { get; set; }

    internal bool hasInputModule { get; private set; }

    protected override void OnStart()
    {
        inputModule = GetComponent<InputModuleBase>();
        base.OnStart();
    }

    protected override void Tick()
    {
        hasInputModule = inputModule != null;
        base.Tick();
    }
}
