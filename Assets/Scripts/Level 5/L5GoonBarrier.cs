using System.Drawing;
using UnityEngine;

public class L5GoonBarrier : MonoBehaviour
{
    public GameObject explosionEffect;
    public Transform point;
    public GameObject barrier;
    public Animator animator;
    bool triggered;
    float timer = 0.5f;

    void Start()
    {
        
    }

    void Update()
    {
        if (timer <= 0f && barrier.activeInHierarchy)
        {
            GameObject kaboom = Instantiate(explosionEffect, point.position, point.rotation);
            kaboom.transform.localScale = new Vector3(4f, 4f, 4f);
            barrier.SetActive(false);
        }
        else if (triggered)
        {
            timer -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "HEAVY" && !triggered)
        {
            triggered = true;
            animator.Play("Valve_Spin_R");
        }
    }
}
