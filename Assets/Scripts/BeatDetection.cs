﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

public class BeatDetection : MonoBehaviour
{
    public void BeatCallback()
    {
        EventManager.TriggerEvent("Beat", new { });
    }
}
