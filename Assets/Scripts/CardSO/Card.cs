using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public bool _isDragging;
    public bool _canDrag;
    public bool _canPoint;
    public bool _played;
    public bool _isSelected;

    public GameObject _playTurnContainer;

    public Vector3 _selectPosition = new Vector3(0, 50, 0);

    Canvas _canvas;
    public CardManager _cardManager;

    public GameManager _gameManager;

    public Button _playButton;

    private void Start()
    {
        _canvas = GameObject.Find("GameCanvas").GetComponent<Canvas>();
        _cardManager = GameObject.Find("-- CardManager --").GetComponent<CardManager>();
        _cardManager._cardsLayoutGroup._cards.Add(gameObject);

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _playTurnContainer = GameObject.Find("PlayTurnCardsLayoutGroup");

        _playButton = GameObject.Find("PlayHand").GetComponent<Button>();
        _playButton.onClick.AddListener(PlayTurn);
    }

    private void Update()
    {
        if (_isSelected && !_isDragging)
            transform.position = transform.parent.position + _selectPosition;
        else if ( !_isSelected && !_isDragging)
            transform.position = transform.parent.position;

        if (_gameManager._gameState == GameManager.STATE.ENEMYATTACKING && transform.parent.parent == _playTurnContainer.transform)
            Destroy(transform.parent.gameObject);

        if (_gameManager._gameState == GameManager.STATE.PLAYERTURN)
        {
            _canDrag = true;
            _canPoint = true;
        } 
        else
        {
            _canDrag = false;
            _canPoint = false;
        }

    }

    private void PlayTurn()
    {
        if (_isSelected)
        {
            transform.parent.parent = _playTurnContainer.transform;
            _isSelected = false;
        }  
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_canDrag)
        {
            _cardManager.SelectedCard = gameObject;

            _isDragging = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_canDrag)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)_canvas.transform, Input.mousePosition, _canvas.worldCamera, out position);
            transform.position = _canvas.transform.TransformPoint(position);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = transform.parent.position;
        _isDragging = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_canPoint)
        {
            if (!_isSelected && !_isDragging && _cardManager._numberSelected < _cardManager._handSize)
            {
                _isSelected = true;
                _cardManager._numberSelected++;
            }
                
            else if (_isSelected && !_isDragging)
            {
                _isSelected = false;
                _cardManager._numberSelected--;
            }
        }
    }
}
