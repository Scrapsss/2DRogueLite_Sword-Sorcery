using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CardView : MonoBehaviour
{
    [SerializeField] private Image _cardImage;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cardImage = GetComponentsInChildren<Image>()[1];
        _name = GetComponentsInChildren<TMP_Text>()[0];
        _description = GetComponentsInChildren<TMP_Text>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
