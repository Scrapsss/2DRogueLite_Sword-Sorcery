using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

public class CardsLayoutGroup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CardManager _cardManager;
    public GameObject _selectedCard;
    public List<GameObject> _cards = new List<GameObject>();
    public GameManager _gameManager;
    public bool _canSwap;
    private int _numberDone = 0;

    public PlayerController _playerController;

    public Button _playButton;
    private Vector3 _naturalPosition;
    public Vector3 _playPosition = new Vector3(0, 50, 0);

    private void Start()
    {
        _naturalPosition = transform.position;

        _playButton = GameObject.Find("PlayHand").GetComponent<Button>();
        _playButton.onClick.AddListener(PlayTurn);

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (_cardManager.SelectedCard)
            _selectedCard = _cardManager.SelectedCard;
        else
            _selectedCard = null;


        if (_selectedCard && _canSwap)
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                if(_selectedCard.transform.position.x > _cards[i].transform.position.x)
                {
                    if(_selectedCard.transform.parent.GetSiblingIndex() < _cards[i].transform.parent.GetSiblingIndex())
                    {
                        SwapCards(_selectedCard, _cards[i]);
                        break;
                    }
                }

                if (_selectedCard.transform.position.x < _cards[i].transform.position.x)
                {
                    if (_selectedCard.transform.parent.GetSiblingIndex() > _cards[i].transform.parent.GetSiblingIndex())
                    {
                        SwapCards(_selectedCard, _cards[i]);
                        break;
                    }
                }
            }
        }

        if (_gameManager._gameState == GameManager.STATE.PLAYERTURN)
        {
            _canSwap = true;
            transform.position = _naturalPosition;
            _cards.RemoveAll(card => card == null);
        } 
        else
        {
            _canSwap = false;
        }


    }

    private void PlayTurn()
    {
        transform.position -= _playPosition * 5;
        _numberDone = 0;

        for (int i = 0; i < _cards.Count; i++)
        {
            Card Card = _cards[i].GetComponent<Card>();

            if (_cards[i] == Card._isSelected)
            {
                CardInfo cardInfo = Card.GetComponent<CardInfo>();
                _playerController.AttackList.Add(cardInfo._cardName);
                _numberDone++;
            }
        }

        for (int i = 4; i > _numberDone; i--)
        {
            _playerController.AttackList.Add("Empty");
        }
    }

    public void SwapCards(GameObject _currentCard, GameObject _targetCard)
    {
        Transform _currentCardParent = _currentCard.transform.parent;
        Transform _targetCardParent = _targetCard.transform.parent;

        _currentCard.transform.SetParent(_targetCardParent);
        _targetCard.transform.SetParent(_currentCardParent);

        if (!_currentCard.transform.GetComponent<Card>()._isDragging)
        {
            _currentCard.transform.localPosition = Vector2.zero;
        }

        _targetCard.transform.localPosition = Vector2.zero;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _cardManager.HoveringMenu = gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _cardManager.HoveringMenu = null;
    }
}
