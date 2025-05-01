using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private UnitStats _enemyStats;
    private Animator animator;

    private SpriteRenderer _spriteRenderer;

    private GameObject Hero_GO;
    private Transform _heroTransform;
    private PlayerController _heroController;

    private float _speed = 7f;
    private float _offset = -1f;
    private float _spawnPoint;

    enum STATE
    {
        IDLE,
        FORWARD,
        ATTACK,
        BACKWARD,
        DEAD
    }

    private STATE _state = STATE.IDLE;

    private bool _isMoving = false;

    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    //Liste des attaques à faire
    private string[] AttackList = new string[4];
    private int _attackListIndex = 0;
    private bool _hasAttacked = false;

    

    //On viens chercher le GameManager pour savoir dans quel State nous sommes
    public GameObject GameManager_GO;
    private GameManager _gameManagerScript;

    //UIManager pour modifier l'ATH
    public GameObject UIManager_GO;
    private UIManager _uiManagerScript;




    private void Awake()
    {
        animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyStats = GetComponent<UnitStats>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Hero_GO = GameObject.FindWithTag("Player");
        _heroTransform = Hero_GO.GetComponent<Transform>();
        _heroController = Hero_GO.GetComponent<PlayerController>();

        GameManager_GO = GameObject.FindWithTag("GameManager");
        _gameManagerScript = GameManager_GO.GetComponent<GameManager>();

        UIManager_GO = GameObject.FindWithTag("UIManager");
        _uiManagerScript = UIManager_GO.GetComponent<UIManager>();

        _spawnPoint = transform.position.x;

        AttackList[0] = "MeleeAttack_1";
        AttackList[1] = "MeleeAttack_2";
        AttackList[2] = "Empty";
        AttackList[3] = "Empty";
    }

    // Update is called once per frame
    void Update()
    {

        if (_gameManagerScript._gameState == GameManager.STATE.ENEMYTURN && !_enemyStats.IsDead)
            _state = STATE.FORWARD;

        StateManager(_state);
        DeathCheck();
    }

    private void StateManager(STATE state)
    {
        switch (state)
        {
            case STATE.IDLE:
                _spriteRenderer.flipX = true;
                //On reset le compteur de la liste
                _attackListIndex = 0;
                break;
            case STATE.FORWARD:
                _gameManagerScript._gameState = GameManager.STATE.ENEMYATTACKING;
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
                case "Empty":
                    NextAttack();
                    break;
            }
        }
    }

    //Ici on passe à l'attaque suivante et donc on reset _hasAttacked
    private void NextAttack()
    {
        if (_attackListIndex >= 3)
        {
            AttackFinish();
        }
        else
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

    // On se rapproche de l'ennemi
    private void MoveTowards()
    {
        if (transform.position.x > _heroTransform.position.x - _offset)
        {
            IsMoving = true;
            transform.Translate(-1 * _speed * Time.deltaTime, 0, 0);
        }
        else
        {
            IsMoving = false;

            _state = STATE.ATTACK;
        }
    }

    //La fonctione pour revenir à notre point de départ
    private void MoveBackwards()
    {
        if (transform.position.x < _spawnPoint)
        {
            IsMoving = true;
            transform.Translate(1 * _speed * Time.deltaTime, 0, 0);
            _spriteRenderer.flipX = false;
        }
        else
        {
            IsMoving = false;
            _state = STATE.IDLE;
            _gameManagerScript._gameState = GameManager.STATE.PLAYERTURN;
        }
    }

    public void TakePhysicDamage(int Damage)
    {
        if (!_enemyStats.IsDead)
        {
            if (Damage - _enemyStats.physicArmor <= 0)
            {
                animator.SetTrigger(AnimationStrings.blockTrigger);
                _uiManagerScript.BlockDamage(gameObject);
            }
            else
            {
                _enemyStats.hp -= Damage - _enemyStats.physicArmor;
                animator.SetTrigger(AnimationStrings.hitTrigger);
                _uiManagerScript.TookDamage(gameObject, Damage);
            }
        }
    }

    public void TakeMagicDamage(int Damage)
    {
        if (!_enemyStats.IsDead)
        {
            if (Damage - _enemyStats.magicArmor <= 0)
            {
                animator.SetTrigger(AnimationStrings.blockTrigger);
                _uiManagerScript.BlockDamage(gameObject);
            }
            else
            {
                _enemyStats.hp -= Damage - _enemyStats.magicArmor;
                animator.SetTrigger(AnimationStrings.hitTrigger);
                _uiManagerScript.TookDamage(gameObject, Damage);
            }
        }
    }

    private void DealPhysicDamage()
    {
        _heroController.TakePhysicDamage(_enemyStats.physicDamage);
    }

    private void DealMagicDamage()
    {
        _heroController.TakeMagicDamage(_enemyStats.magicDamage);
    }

    private void DeathCheck()
    {
        if (_enemyStats.IsDead)
        {
            animator.SetBool(AnimationStrings.isDead, true);
            _state = STATE.DEAD;
            _gameManagerScript._gameState = GameManager.STATE.WIN;
        }
    }
}
