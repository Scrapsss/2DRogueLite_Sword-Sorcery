using UnityEngine;

public class CardManager : MonoBehaviour
{
    [HideInInspector] public GameObject SelectedCard;
    [HideInInspector] public GameObject HoveringMenu;

    public CardsLayoutGroup _cardsLayoutGroup;
    private float _DrawCooldown = 0.1f;
    private float _timer = 0f;
    public Vector3 _offset = new Vector3(0, -50, 0);

    public GameObject _CardTrackerPrefab;
    public GameObject _CardPrefab;
    private GameObject _spawn;
    public int _handSize = 4;
    public int _numberSelected = 0;

    public GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _spawn = GameObject.Find("DeckButton");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Update()
    {
        if (_gameManager._gameState == GameManager.STATE.PLAYERTURN)
        {
            _timer += Time.deltaTime;
            if (_timer > _DrawCooldown)
            {
                AddCard();
                _timer = 0f;
            }
        }

        if (_gameManager._gameState == GameManager.STATE.PLAYERATTACKING)
        {
            _numberSelected = 0;
        }
    }

    public void AddCard()
    {
        if( _cardsLayoutGroup.transform.childCount < 6 )
        {
            GameObject CardTrackerObject = Instantiate(_CardTrackerPrefab, _cardsLayoutGroup.transform);
            CardsFace CardObject = Instantiate(_CardPrefab, _spawn.transform).GetComponentInChildren<CardsFace>();
            CardObject._target = CardTrackerObject.GetComponentInChildren<Card>().gameObject;
        }
    }
}
