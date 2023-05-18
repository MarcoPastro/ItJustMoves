using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResumeScene : Unit
{
    [DoNotSerialize]
    public ControlInput InputTrigger;
    [DoNotSerialize]
    public ControlOutput OutputTrigger;
    protected override void Definition()
    {
        InputTrigger = ControlInput("", InternalBoot);
        OutputTrigger = ControlOutput("");
    }
    private ControlOutput InternalBoot(Flow arg)
    {
        PlayerController.Instance.ResumeFromPause();
        return OutputTrigger;
    }
}
