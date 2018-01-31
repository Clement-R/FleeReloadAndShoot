using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

public class BeatDetection : MonoBehaviour
{
    public void BeatCallback(object cookie, AkCallbackType type, object callbakInfo)
    {
        EventManager.TriggerEvent("Beat", new { });
        Debug.Log("Beat");
    }
}
