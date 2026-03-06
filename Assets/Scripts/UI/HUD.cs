using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public TextMeshPro healthText;

    TopDownRigidbodyController player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<TopDownRigidbodyController>();
    }

    void Update()
    {
        healthText.text = "HP: " + player.health.ToString();
    }
}
