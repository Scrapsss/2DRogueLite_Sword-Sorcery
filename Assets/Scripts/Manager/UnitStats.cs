using UnityEngine;

public class UnitStats : MonoBehaviour
{
    [Header("Statistique Principale")]
    public string name;
    public int level;
    public int hp;
    private int maxHp;
    private bool isDead = false;

    public bool IsDead { get { return isDead; } }

    [Header("Damage Stats")]
    public int physicDamage;
    public int magicDamage;
    public int critChance;
    public int critDamage;

    [Header("Armor Stats")]
    public int physicArmor;
    public int magicArmor;

    private void Update()
    {
        if (hp <= 0)
        {
            isDead = true;
        }
    }
}
