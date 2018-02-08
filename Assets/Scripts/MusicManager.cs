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

        AkSoundEngine.PostEvent("interactive_music", gameObject, (uint)AkCallbackType.AK_MusicSyncBeat | (uint)AkCallbackType.AK_EndOfEvent | (uint)AkCallbackType.AK_EnableGetSourcePlayPosition, MainMusicCallback, this);
    }

    void MainMusicCallback(object cookie, AkCallbackType type, object callbakInfo)
    {
        switch (type)
        {
            case AkCallbackType.AK_EndOfEvent:
                // TODO : Handle end of music
                break;

            case AkCallbackType.AK_MusicSyncBeat:
                AkMusicSyncCallbackInfo info = (AkMusicSyncCallbackInfo)callbakInfo;
                _beatManager.BeatCallback(info.segmentInfo_fBeatDuration);
                break;
        }
    }
}
