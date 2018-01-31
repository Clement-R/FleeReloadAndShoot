using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

public class SpriteSwapOnBeat : MonoBehaviour {

    public Sprite beatSprite;

    private Sprite _baseSprite;
    private SpriteRenderer _sr;

    void Start ()
    {
        _sr = GetComponent<SpriteRenderer>();
        _baseSprite = _sr.sprite;
        EventManager.StartListening("Beat", (dynamic obj) => { StartCoroutine(SpriteSwap()); });
    }

    IEnumerator SpriteSwap()
    {
        _sr.sprite = beatSprite;
        yield return new WaitForSeconds(0.1f);
        _sr.sprite = _baseSprite;
    }
}
