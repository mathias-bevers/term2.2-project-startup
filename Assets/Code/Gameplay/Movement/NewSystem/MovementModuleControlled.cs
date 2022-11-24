using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementModuleControlled : MovementModule
{
    public InputModuleBase inputModule;

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
