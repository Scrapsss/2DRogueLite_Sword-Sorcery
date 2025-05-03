using UnityEngine;


[CreateAssetMenu(fileName = "New Card", menuName = "Card")]

public class Card_SO : ScriptableObject
{
    public Sprite _cardImage;
    public string _cardName;
    public string _cardDescriptionText;
    public CardRarity _cardRarity;
    [Header("Card Stats")]
    public CardEffect _effectType;
    public int _value;



    public void UseCard()
    {

    }



}

public enum CardRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public enum CardEffect
{
    DealPhysicDamage,
    DealMagicDamage,
}
