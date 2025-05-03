using NUnit.Framework;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

/// <summary>
/// Projet commencé le 1er mai 2025
/// 
/// Contrôle du personnage principal, toutes ses infos etc...
/// </summary>

public class PlayerController : MonoBehaviour
{
    private bool _isMoving = false;

    public bool IsMoving { get 
        { 
            return _isMoving; 
        } 
        private set 
        { 
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    Animator animator;

    //On viens chercher l'ennemi pour obtenir des informations
    private GameObject Enemy_GO;
    private Transform _enemyTransform;
    private EnemyController _enemyController;

    private float _offset = 1f;
    private float _spawnPoint;


    //Gestion de notre personnage
    private float _speed = 7f;
    private Quaternion _currentRotation;
    private SpriteRenderer _spriteRenderer;
    private Collider2D _playerCollider;

    private UnitStats _playerStats;

    //On viens chercher le GameManager pour savoir dans quel State nous sommes
    public GameObject GameManager_GO;
    private GameManager _gameManagerScript;

    //UIManager pour modifier l'ATH
    public GameObject UIManager_GO;
    private UIManager _uiManagerScript;

    public Button _playButton;


    //L'état du personnage
    enum STATE { 
        IDLE,
        FORWARD,
        ATTACK,
        BACKWARD,
        DEAD
    }

    //Liste des attaques à faire
    public List<string> AttackList = new List<string>();
    private int _attackListIndex = 0;
    private bool _hasAttacked = false;

    private STATE _state = STATE.IDLE;

    public CardManager _cardManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerStats = GetComponent<UnitStats>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Enemy_GO = GameObject.FindWithTag("Enemy");
        _enemyController = Enemy_GO.GetComponent<EnemyController>();
        _enemyTransform = Enemy_GO.GetComponent<Transform>();

        GameManager_GO = GameObject.FindWithTag("GameManager");
        _gameManagerScript = GameManager_GO.GetComponent<GameManager>();

        UIManager_GO = GameObject.FindWithTag("UIManager");
        _uiManagerScript = UIManager_GO.GetComponent<UIManager>();

        _playButton = GameObject.Find("PlayHand").GetComponent<Button>();
        _playButton.onClick.AddListener(PlayTurn);

        _cardManager = GameObject.Find("-- CardManager --").GetComponent<CardManager>();

        _spawnPoint = transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        StateManager(_state);
        DeathCheck();

        

    }

    //Gestion de l'état de notre personnage
    private void StateManager(STATE state)
    {
        switch( state )
        {
            case STATE.IDLE:
                _spriteRenderer.flipX = false;
                //On reset le compteur de la liste
                _attackListIndex = 0;
                AttackList.Clear();
                break;
            case STATE.FORWARD:
                MoveTowards();
                
                break;
            case STATE.ATTACK:
                Attack();
                break;
            case STATE.BACKWARD:
                MoveBackwards();
                
                break;
            case STATE.DEAD:
                break;
        }
    }

    private void PlayTurn()
    {
        if (_gameManagerScript._gameState == GameManager.STATE.PLAYERTURN && _cardManager._numberSelected > 0)
        {
            _state = STATE.FORWARD;
            _gameManagerScript._gameState = GameManager.STATE.PLAYERATTACKING;

        }

        ListCheck();
    }

    private void ListCheck()
    {
        for (int i = 0; i < AttackList.Count; i++)
        {
            print("Reçu : " + AttackList[i]);
        }
    }

    // On se rapproche de l'ennemi
    private void MoveTowards()
    {
        if (transform.position.x < _enemyTransform.position.x - _offset)
        {
            IsMoving = true;
            transform.Translate(1 * _speed * Time.deltaTime, 0, 0);
        } else
        {
            IsMoving = false;
            
            _state = STATE.ATTACK;
        }
    }

    //La fonctione pour revenir à notre point de départ
    private void MoveBackwards()
    {
        if (transform.position.x > _spawnPoint)
        {
            IsMoving = true;
            transform.Translate(-1 * _speed * Time.deltaTime, 0, 0);
            _spriteRenderer.flipX = true;
        }
        else
        {
            IsMoving = false;
            _state = STATE.IDLE;
            _gameManagerScript._gameState = GameManager.STATE.ENEMYTURN;
        }
    }

    //Alors, ici on viens gérer la liste d'attaque, on lis l'attaque actuelle de la liste et on active les animations correspondantes, on se sert du _hasAttacked pour garantir que les trigger ne s'activent qu'une fois
    private void Attack()
    {

        //C'est pour jouer temporairement ça, plus tard on ira chercher une liste de Card_SO et on fera ce que la carte dit
        if (!_hasAttacked)
        {
            switch (AttackList[_attackListIndex])
            {
                
                case "Attaque Simple":
                    animator.SetTrigger(AnimationStrings.attackTrigger);

                    _hasAttacked = true;
                    break;
                case "MeleeAttack_2":
                    animator.SetTrigger(AnimationStrings.attackTrigger);
                    _hasAttacked = true;
                    break;
                case "SpellCast_1":
                    animator.SetTrigger(AnimationStrings.spellCastTrigger);
                    _hasAttacked = true;
                    break;
                default:
                    NextAttack();
                    break;
            }
        } 
    }

    //Ici on passe à l'attaque suivante et donc on reset _hasAttacked
    private void NextAttack()
    {
        if ( _attackListIndex >= AttackList.Count -1 )
        {
            AttackFinish();
        } else
        {
            _attackListIndex++;
            _hasAttacked = false;
        }
    }

    //On retourne à notre point de départ
    private void AttackFinish()
    {
        _state = STATE.BACKWARD;
        _hasAttacked = false;
    }

    private void DealPhysicDamage()
    {
        _enemyController.TakePhysicDamage(_playerStats.physicDamage);
    }

    private void DealMagicDamage()
    {
        _enemyController.TakeMagicDamage(_playerStats.magicDamage);
    }

    public void TakePhysicDamage(int Damage)
    {
        if (!_playerStats.IsDead)
        {
            if (Damage - _playerStats.physicArmor <= 0)
            {
                animator.SetTrigger(AnimationStrings.blockTrigger);
                _uiManagerScript.BlockDamage(gameObject);
            }
            else
            {
                _playerStats.Health -= Damage - _playerStats.physicArmor;
                animator.SetTrigger(AnimationStrings.hitTrigger);
                _uiManagerScript.TookDamage( gameObject, Damage - _playerStats.physicArmor);

                _playerStats.playerHealthChanged.Invoke(_playerStats.Health, _playerStats.maxHp);
            }
        }
    }

    public void TakeMagicDamage(int Damage)
    {
        if (!_playerStats.IsDead)
        {
            if (Damage - _playerStats.magicArmor <= 0)
            {
                animator.SetTrigger(AnimationStrings.blockTrigger);
                _uiManagerScript.BlockDamage(gameObject);
            }
            else
            {
                _playerStats.Health -= Damage - _playerStats.magicArmor;
                animator.SetTrigger(AnimationStrings.hitTrigger);
                _uiManagerScript.TookDamage(gameObject, Damage - _playerStats.magicArmor);

                _playerStats.playerHealthChanged.Invoke(_playerStats.Health, _playerStats.maxHp);
            }
        }
    }

    private void DeathCheck()
    {
        if (_playerStats.IsDead)
        {
            animator.SetBool(AnimationStrings.isDead, true);
            _state = STATE.DEAD;
            _gameManagerScript._gameState = GameManager.STATE.LOOSE;
        }
    }
}
