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
    public Sprite bubbleHidden;

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
    private Animator _animator;
	void Awake ()
    {
        EventManager.StartListening("PlayAction", PlayAction);
        EventManager.StartListening("PlayerShoot", OnGettingShot);

        if(side == -1)
        {
            bubble.flipX = true;
        }

        _action = Actions.None;
        _animator = GetComponent<Animator>();
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
                _animator.SetBool("reload_dodge", false);
                _animator.SetTrigger("hit");
            }
            else
            {
                _animator.SetBool("reload_dodge", true);
                _animator.SetTrigger("reload");
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

            print(_action + " : " + side);

            StartCoroutine(ShowBubble(_action));

            _isDefending = false;
            _action = Actions.None;
        }
    }

    IEnumerator ShowBubble(Actions action)
    {
        Sprite sprite = null;
        switch (action)
        {
            case Actions.Shoot:
                sprite = bubbleShoot;
                break;
            case Actions.Defend:
                sprite = bubbleDefend;
                break;
            case Actions.Reload:
                sprite = bubbleReload;
                break;
            case Actions.None:
                sprite = null;
                break;
        }

        bubble.sprite = sprite;

        yield return new WaitForSeconds(0.4f);

        if(bubble.sprite != bubbleHidden)
        {
            bubble.sprite = null;
        }
    }

    void Update ()
    {
		// Read player input and assign to action
        if(Input.GetKeyDown(shotKey))
        {
            _action = Actions.Shoot;
            bubble.sprite = bubbleHidden;
        }
        else if(Input.GetKeyDown(reloadKey))
        {
            _action = Actions.Reload;
            bubble.sprite = bubbleHidden;
        }
        else if (Input.GetKeyDown(defendKey))
        {
            _action = Actions.Defend;
            bubble.sprite = bubbleHidden;
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

            _animator.SetBool("shoot_fail", false);
            _animator.SetTrigger("shoot");
        }
        else
        {
            _animator.SetBool("shoot_fail", true);
            _animator.SetTrigger("shoot");
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

        if(_bullets == 3)
        {
            _animator.SetBool("reload_fail", true);
        }
        else
        {
            _animator.SetBool("reload_fail", false);
        }

        _bullets = Mathf.Clamp(_bullets, 0, 3);

        _animator.SetTrigger("reload");

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
        EventManager.TriggerEvent("Lose", new { side = side });
        EventManager.TriggerEvent("Pause", new { });
        print("Lose " + side);
    }
}
