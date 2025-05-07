using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card Game/Card")]
public class Card_SO : ScriptableObject
{
    public enum CardEffect
    {
        Damage,
        Heal,
        Buff,
        Debuff
    }

    public enum CardAnimation
    {
        MeleeAttack,
        SpellCast
    }

    public string _name;
    [TextArea]
    public string _description; // Ex: Attaque simple qui inflige {value} de dégâts
    public CardAnimation _animation;
    public CardEffect _effectType;
    public int _baseValue;

    public string GetDesc(int characterStat)
    {
        int _finalValue = _baseValue + characterStat;
        return _description.Replace("{value}", _finalValue.ToString());
    }
}
