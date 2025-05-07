using UnityEngine;

/// <summary>
/// On instancie les cartes dans le jeu
/// </summary>

public class CardManager : MonoBehaviour
{

    public GameObject CardPrefab;
    private GameObject CardZone;

    public int HandSize;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CardZone = GameObject.Find("CardZone");

        DrawCard(HandSize);
    }

    public void DrawCard(int handSize)
    {

    }
}
