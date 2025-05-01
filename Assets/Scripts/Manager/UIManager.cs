using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject _damageTextPrefab;
    public GameObject _healthTextPrefab;

    public Canvas _gameCanvas;
    public Vector3 _offset = new Vector3(0, 50, 0);

    private void Awake()
    {
        _gameCanvas = FindFirstObjectByType<Canvas>();
    }

    public void TookDamage(GameObject character, int damage)
    {
        //On r�cup�re la position de notre personnage et on la transform en position HUD
        Vector3 _spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        //On instancie et r�cup�re directement un composant 
        TMP_Text _innerText = Instantiate(_damageTextPrefab, _spawnPosition + _offset, Quaternion.identity, _gameCanvas.transform)
            .GetComponent<TMP_Text>();

        //On change son texte interne
        _innerText.text = damage.ToString();
    }

    public void BlockDamage(GameObject character)
    {
        //On r�cup�re la position de notre personnage et on la transform en position HUD
        Vector3 _spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        //On instancie et r�cup�re directement un composant 
        TMP_Text _innerText = Instantiate(_damageTextPrefab, _spawnPosition + _offset, Quaternion.identity, _gameCanvas.transform)
            .GetComponent<TMP_Text>();

        //On change son texte interne
        _innerText.text = "Block";
        _innerText.color = Color.white;
    }

    public void Healed(GameObject character, int healthRestored)
    {
        //On r�cup�re la position de notre personnage et on la transform en position HUD
        Vector3 _spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        //On instancie et r�cup�re directement un composant 
        TMP_Text _innerText = Instantiate(_healthTextPrefab, _spawnPosition + _offset, Quaternion.identity, _gameCanvas.transform)
            .GetComponent<TMP_Text>();

        //On change son texte interne
        _innerText.text = healthRestored.ToString();
    }
}
