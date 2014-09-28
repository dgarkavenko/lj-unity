using System;
using UnityEngine;
using System.Collections;

public class LegDispatcher : MonoBehaviour
{


    public Action<int> OnEnterFrame;

    public void EnterFrame(int a)
    {
        if (OnEnterFrame != null) OnEnterFrame(a);
    }
}
