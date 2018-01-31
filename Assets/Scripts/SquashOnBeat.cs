using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

public class SquashOnBeat : MonoBehaviour {

    public Vector3 squashScale;

    private Vector3 _baseScale;

	void Start ()
    {
        _baseScale = transform.localScale;
        EventManager.StartListening("Beat", (dynamic obj) => { StartCoroutine(Squash()); });
	}

	IEnumerator Squash()
    {
        transform.localScale = squashScale;
        yield return new WaitForSeconds(0.1f);
        transform.localScale = _baseScale;
    }
}
