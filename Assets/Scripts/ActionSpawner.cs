using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

public class ActionSpawner : MonoBehaviour {

    public int side;
    public string actionTag;
    public GameObject action;

	void Start ()
    {
        EventManager.StartListening("SpawnAction", (dynamic obj) => { SpawnAction(obj.side, obj.beatDuration); });
	}

    void SpawnAction(int side, float beatDuration)
    {
        if(side == this.side)
        {
            GameObject newAction = Instantiate(action, transform.position, Quaternion.identity);
            newAction.tag = actionTag;

            MoveToBeatMark moveToComponent = newAction.GetComponent<MoveToBeatMark>();
            moveToComponent.side = this.side;
            moveToComponent.LaunchMovement(new { duration = beatDuration });
        }
    }
}
