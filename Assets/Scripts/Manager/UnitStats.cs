using UnityEngine;
using UnityEngine.Events;

public class UnitStats : MonoBehaviour
{
    public UnityEvent<int, int> playerHealthChanged;
    public UnityEvent<int> playerLevelChanged;

    public UnityEvent<int, int> enemyHealthChanged;
    public UnityEvent<int> enemyLevelChanged;

    [Header("Statistique Principale")]
    public string name;
    public int level;
    [SerializeField] private int _hp;
    public int maxHp;
    private bool isDead = false;

    public int Health
    {
        get
        {
            return _hp;
        }
        set
        {
            _hp = value;


            if (_hp <= 0)
            {
                _hp = 0;
                isDead = true;
            }
        }
    }

    public bool IsDead { get { return isDead; } }

    [Header("Damage Stats")]
    public int physicDamage;
    public int magicDamage;
    public int critChance;
    public int critDamage;

    [Header("Armor Stats")]
    public int physicArmor;
    public int magicArmor;

    private void Awake()
    {
        maxHp = _hp;
    }

    private void Start()
    {
        
        playerHealthChanged?.Invoke(_hp, maxHp);
        playerLevelChanged?.Invoke(level);

        enemyHealthChanged?.Invoke(_hp, maxHp);
        enemyLevelChanged?.Invoke(level);
    }

    private void Update()
    {
        
    }
}
