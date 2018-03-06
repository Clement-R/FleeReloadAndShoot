using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

public class PlayerBehavior : MonoBehaviour {

    [Header("Keys")]
    public KeyCode shotKey;
    public KeyCode reloadKey;
    public KeyCode defendKey;

    [Header("Bubble sprites")]
    public Sprite bubbleNone;
    public Sprite bubbleShoot;
    public Sprite bubbleReload;
    public Sprite bubbleDefend;

    [Header("UI gameobjects")]
    public GameObject[] bulletsUI = new GameObject[3];
    public GameObject[] roundsUI = new GameObject[3];
    public GameObject[] healthUI;
    public SpriteRenderer bubble;

    [Space(25)]
    public int side = 0;

    private int _health = 3;
    private int _bullets = 0;
    private int _rounds = 0;
    private bool _isDefending = false;
    private Actions _action;

	void Awake ()
    {
        EventManager.StartListening("PlayAction", PlayAction);
        EventManager.StartListening("PlayerShoot", OnGettingShot);

        if(side == -1)
        {
            bubble.flipX = true;
        }

        _action = Actions.None;
    }

    void OnGettingShot(dynamic obj)
    {
        // If it wasn't us, we take a bullet
        if(obj.side != side)
        {
            if(!_isDefending)
            {
                // Lose health
                TakeDamage();
            }
        }
    }

    void PlayAction(dynamic obj)
    {
        if (obj.side != side)
        {
            // Play action
            switch (_action)
            {
                case Actions.Shoot:
                    Shoot();
                    break;

                case Actions.Defend:
                    Defend();
                    break;

                case Actions.Reload:
                    Reload();
                    break;

                case Actions.None:
                    break;
            }

            _isDefending = false;
            _action = Actions.None;
        }
    }

    void ShowBubble()
    {
        // TODO
    }

    void Update ()
    {
		// Read player input and assign to action
        if(Input.GetKeyDown(shotKey))
        {
            _action = Actions.Shoot;
        }
        else if(Input.GetKeyDown(reloadKey))
        {
            _action = Actions.Reload;
        }
        else if (Input.GetKeyDown(defendKey))
        {
            _action = Actions.Defend;
        }

        if(_action != Actions.None)
        {
            print(_action + " : " + side);
        }
    }

    void Shoot()
    {
        if(_bullets > 0)
        {
            // Remove a bullet
            _bullets--;
            _bullets = Mathf.Clamp(_bullets, 0, 3);

            // Shoot action
            EventManager.TriggerEvent("PlayerShoot", new { side = side });
        }

        UpdateUI();
    }

    void Defend()
    {
        _isDefending = true;

        UpdateUI();
    }

    void Reload()
    {
        _bullets++;
        _bullets = Mathf.Clamp(_bullets, 0, 3);

        UpdateUI();
    }

    void TakeDamage()
    {
        _health--;

        UpdateUI();

        if (_health == 0)
        {
            print("Lose " + side);
            Lose();
        }
    }

    void UpdateUI()
    {
        // Health
        for (int i = 0; i < 4; i++)
        {
            healthUI[i].SetActive(false);
        }
        
        for (int i = 0; i < _health + 1; i++)
        {
            healthUI[i].SetActive(true);
        }

        // Bullets
        for (int i = 0; i < 3; i++)
        {
            bulletsUI[i].SetActive(false);
        }

        for (int i = 0; i < _bullets; i++)
        {
            bulletsUI[i].SetActive(true);
        }

        // Rounds
        for (int i = 0; i < 3; i++)
        {
            roundsUI[i].SetActive(false);
        }

        for (int i = 0; i < _rounds; i++)
        {
            roundsUI[i].SetActive(true);
        }
    }

    void Lose()
    {
        // TODO
    }
}
