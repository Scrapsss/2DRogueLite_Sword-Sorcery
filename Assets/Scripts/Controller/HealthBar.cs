using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject Player_GO;
    private UnitStats _playerStats;

    public GameObject Enemy_GO;
    private UnitStats _enemyStats;

    public GameObject _playerHealthBar;
    public GameObject _enemyHealthBar;

    public Slider _playerHealthSlider;
    public TMP_Text _playerHealthText;

    public Slider _enemyHealthSlider;
    public TMP_Text _enemyHealthText;


    private void Awake()
    {
        

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player_GO = GameObject.FindGameObjectWithTag("Player");
        _playerStats = Player_GO.GetComponent<UnitStats>();

        Enemy_GO = GameObject.FindGameObjectWithTag("Enemy");
        _enemyStats = Enemy_GO.GetComponent<UnitStats>();

        _playerHealthSlider = _playerHealthBar.GetComponent<Slider>();
        _playerHealthText = _playerHealthBar.GetComponentInChildren<TMP_Text>();

        _enemyHealthSlider = _enemyHealthBar.GetComponent<Slider>();
        _enemyHealthText = _enemyHealthBar.GetComponentInChildren<TMP_Text>();

        _playerStats.playerHealthChanged.AddListener(OnPlayerHealthChanged);
        _enemyStats.enemyHealthChanged.AddListener(OnEnemyHealthChanged);
    }

    private float CalculateSliderPercentage(float currentHp, float maxHealth)
    {
        return currentHp / maxHealth;
    }

    private void OnDisable()
    {
        _playerStats.playerHealthChanged.RemoveListener(OnPlayerHealthChanged);
        _playerStats.enemyHealthChanged.RemoveListener(OnEnemyHealthChanged);
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        _playerHealthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        _playerHealthText.text = "Health " + newHealth + " / " + maxHealth;
    }

    private void OnEnemyHealthChanged(int newHealth, int maxHealth)
    {
        _enemyHealthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        _enemyHealthText.text = "Health " + newHealth + " / " + maxHealth;
    }
}
