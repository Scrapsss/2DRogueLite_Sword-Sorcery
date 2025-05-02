using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject HeroPrefab;
    public GameObject EnemyPrefab;

    public Transform HeroSpawn;
    public Transform EnemySpawn;

    public enum STATE
    {
        PLAYERTURN,
        PLAYERATTACKING,
        ENEMYTURN,
        ENEMYATTACKING,
        SHOP,
        WIN,
        LOOSE
    }

    public STATE _gameState;

    private void Awake()
    {
        Instantiate(HeroPrefab, HeroSpawn);
        Instantiate(EnemyPrefab, EnemySpawn);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StateManager(_gameState);
    }

    private void StateManager(STATE state)
    {

    }
}
