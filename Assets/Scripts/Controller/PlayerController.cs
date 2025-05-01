using NUnit.Framework;
using System;
using UnityEngine;
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


    //L'état du personnage
    enum STATE { 
        IDLE,
        FORWARD,
        ATTACK,
        BACKWARD,
        DEAD
    }

    //Liste des attaques à faire
    private string[] AttackList = new string[4];
    private int _attackListIndex = 0;
    private bool _hasAttacked = false;

    private STATE _state = STATE.IDLE;

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

        _spawnPoint = transform.position.x;

        AttackList[0] = "MeleeAttack_1";
        AttackList[1] = "MeleeAttack_2";
        AttackList[2] = "SpellCast_1";
        AttackList[3] = "SpellCast_1";

    }

    // Update is called once per frame
    void Update()
    {
        StateManager(_state);
        DeathCheck();

        if (Input.GetKeyDown(KeyCode.Space) && _gameManagerScript._gameState == GameManager.STATE.PLAYERTURN)
        {
            _state = STATE.FORWARD;
            _gameManagerScript._gameState = GameManager.STATE.PLAYERATTACKING;
            
        }

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
        if (!_hasAttacked)
        {
            switch (AttackList[_attackListIndex])
            {
                case "MeleeAttack_1":
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
            }
        } 
    }

    //Ici on passe à l'attaque suivante et donc on reset _hasAttacked
    private void NextAttack()
    {
        if ( _attackListIndex >=3 )
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
            }
            else
            {
                _playerStats.hp -= Damage - _playerStats.physicArmor;
                animator.SetTrigger(AnimationStrings.hitTrigger);
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
            }
            else
            {
                _playerStats.hp -= Damage - _playerStats.magicArmor;
                animator.SetTrigger(AnimationStrings.hitTrigger);
            }
        }
    }

    private void DeathCheck()
    {
        if (_playerStats.IsDead)
        {
            animator.SetBool(AnimationStrings.isDead, true);
        }
    }
}
