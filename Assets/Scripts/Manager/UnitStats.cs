using UnityEngine;
using UnityEngine.Events;

public class UnitStats : MonoBehaviour
{
    public UnityEvent<int, int> playerHealthChanged;
    public UnityEvent<int, int> enemyHealthChanged;

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

    private void Start()
    {
        maxHp = _hp;
        playerHealthChanged?.Invoke(_hp, maxHp);
        enemyHealthChanged?.Invoke(_hp, maxHp);
    }

    private void Update()
    {
        
    }
}
