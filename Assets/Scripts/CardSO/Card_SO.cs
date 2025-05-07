using Unity.VisualScripting;
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

    public Sprite _sprite;
    public string _name;
    [TextArea]
    public string _description; // Ex: Attaque simple qui inflige {value} de dégâts
    public CardAnimation _animation;
    public CardEffect _effectType;
    public int _baseValue;

    //Ici on transforme la description avec la valeur de dégat de notre personnage
    public string GetDesc(int characterStat)
    {
        int _finalValue = _baseValue + characterStat;
        return _description.Replace("{value}", _finalValue.ToString());
    }

}
