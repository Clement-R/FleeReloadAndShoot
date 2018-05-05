using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

public class MusicManager : MonoBehaviour {

    private BeatDetection _beatManager;
    private uint _songId;

    void Start()
    {
        EventManager.StartListening("Pause", PauseSong);
        EventManager.StartListening("Resume", ResumeSong);

        _beatManager = GetComponent<BeatDetection>();
        StartCoroutine(LaunchSong());
    }

    IEnumerator LaunchSong()
    {
        yield return null;
        yield return null;

        _songId = AkSoundEngine.PostEvent("interactive_music", gameObject, (uint)AkCallbackType.AK_MusicSyncBeat | (uint)AkCallbackType.AK_EndOfEvent | (uint)AkCallbackType.AK_EnableGetSourcePlayPosition, MainMusicCallback, this);
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

    private void PauseSong(dynamic obj)
    {
        AkSoundEngine.ExecuteActionOnEvent(_songId, AkActionOnEventType.AkActionOnEventType_Pause);
    }

    private void ResumeSong(dynamic obj)
    {
        AkSoundEngine.ExecuteActionOnEvent(_songId, AkActionOnEventType.AkActionOnEventType_Resume);
    }
}
