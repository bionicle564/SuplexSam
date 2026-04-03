using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public Image heartFill;

    public Animator anim;
    public AnimationClip hurtClip;

    TopDownRigidbodyController player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<TopDownRigidbodyController>();
    }

    void Update()
    {
        healthText.text = player.health.ToString();
        heartFill.fillAmount = (float)player.health / (float)player.maxHealth;
    }

    public void DamageAnimation()
    {
        anim.Play(hurtClip.name);
    }
}
