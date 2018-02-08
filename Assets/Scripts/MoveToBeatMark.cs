﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

public class MoveToBeatMark : MonoBehaviour {

    [Tooltip("1 = Left to right ; -1 = Right to Left")]
    public int side = 1;

    private float _beatDuration = 0f;
    private GameObject _beatMarker;
    private float _index = 0f;
    private bool _isMoving = false;
	
	void Start ()
    {
        _beatMarker = GameObject.FindGameObjectWithTag("BeatMarker");
        EventManager.StartListening("Beat", LaunchMovement);
    }

    void LaunchMovement(dynamic obj)
    {
        if(!_isMoving)
        {
            _beatDuration = obj.duration;
            StartCoroutine(MoveToPosition());
        }
    }

    public IEnumerator MoveToPosition()
    {
        _isMoving = true;

        GameObject[] markers;
        if (side == 1)
        {
            markers = GameObject.FindGameObjectsWithTag("P1_Action");
            markers = markers.OrderByDescending(e => e.transform.position.x).ToArray();
        }
        else
        {
            markers = GameObject.FindGameObjectsWithTag("P2_Action");
            markers = markers.OrderBy(e => e.transform.position.x).ToArray();
        }
        
        _index = markers.ToList().IndexOf(gameObject);

        Vector3 currentPos = transform.position;

        float timeToMove = _beatDuration * 4f * ((float) _index + 1f);
        Vector3 position = _beatMarker.transform.position;

        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
    }
}