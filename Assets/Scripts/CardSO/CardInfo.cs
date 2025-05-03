using UnityEngine;

public class CardInfo : MonoBehaviour
{
    [SerializeField] private Card_SO _cardSO;

    public Sprite _cardImage;
    public string _cardName;
    public string _cardDescriptionText;
    public CardRarity _cardRarity;
    [Header("Card Stats")]
    public CardEffect _effectType;
    public int _value;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cardImage = _cardSO._cardImage;
        _cardName = _cardSO._cardName;
        _cardDescriptionText = _cardSO._cardDescriptionText;
        _cardRarity = _cardSO._cardRarity;
        _effectType = _cardSO._effectType;
        _value = _cardSO._value;
    }
}
