using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject Player_GO;
    private UnitStats _unitStats;

    public GameObject Enemy_GO;
    private UnitStats _enemyStats;

    public Slider _playerHealthSlider;
    public TMP_Text _playerHealthText;
    public TMP_Text _playerLevelText;

    public Slider _enemyHealthSlider;
    public TMP_Text _enemyHealthText;
    public TMP_Text _enemyLevelText;


    private void Awake()
    {
        

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player_GO = GameObject.FindGameObjectWithTag("Player");
        _unitStats = Player_GO.GetComponent<UnitStats>();

        Enemy_GO = GameObject.FindGameObjectWithTag("Enemy");
        _enemyStats = Enemy_GO.GetComponent<UnitStats>();

        OnPlayerHealthChanged(_unitStats.Health, _unitStats.maxHp);
        OnPlayerLevelChanged(_unitStats.level);

        OnEnemyHealthChanged(_enemyStats.Health, _unitStats.maxHp);
        OnEnemyLevelChanged(_enemyStats.level);
    }

    private float CalculateSliderPercentage(float currentHp, float maxHealth)
    {
        return currentHp / maxHealth;
    }

    private void OnDisable()
    {
        _unitStats.playerHealthChanged.RemoveListener(OnPlayerHealthChanged);
        _unitStats.playerLevelChanged.RemoveListener(OnEnemyLevelChanged);

        _unitStats.enemyHealthChanged.RemoveListener(OnEnemyHealthChanged);
        _unitStats.enemyLevelChanged.RemoveListener(OnEnemyLevelChanged);
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        _playerHealthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        _playerHealthText.text = "Health " + newHealth + " / " + maxHealth;
    }

    private void OnPlayerLevelChanged(int newLevel)
    {
        _playerLevelText.text = "Lvl " + newLevel;
    }

    private void OnEnemyHealthChanged(int newHealth, int maxHealth)
    {
        _enemyHealthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        _enemyHealthText.text = "Health " + newHealth + " / " + maxHealth;
    }

    private void OnEnemyLevelChanged(int newLevel)
    {
        _enemyLevelText.text = "Lvl " + newLevel;
    }
}
