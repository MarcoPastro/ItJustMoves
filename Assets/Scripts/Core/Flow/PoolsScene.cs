using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolsScene : Unit
{
    [DoNotSerialize]
    public ControlInput InputTrigger;
    [DoNotSerialize]
    public ControlOutput OutputTrigger;

    [DoNotSerialize]
    public ValueInput SceneToLoad;
    protected override void Definition()
    {
        InputTrigger = ControlInput("", InternalBoot);
        OutputTrigger = ControlOutput("");
        SceneToLoad = ValueInput<string>("SceneToLoad", "");
    }
    private ControlOutput InternalBoot(Flow arg)
    {
        PoolingSystem.Instance.SetupSceneManagers(arg.GetValue<string>(SceneToLoad));
        return OutputTrigger;
    }
}
