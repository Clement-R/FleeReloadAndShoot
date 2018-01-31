using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

public class MusicManager : MonoBehaviour {

    private BeatDetection _beatManager;

    void Start()
    {
        _beatManager = GetComponent<BeatDetection>();
        StartCoroutine(LaunchSong());
    }

    IEnumerator LaunchSong()
    {
        yield return null;
        yield return null;

        AkSoundEngine.PostEvent("interactive_music", gameObject, (uint)AkCallbackType.AK_MusicSyncBeat, _beatManager.BeatCallback, this);
    }
}
