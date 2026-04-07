using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    public float jumpHeight = 1f;

    [Tooltip("Speed penalty is a percentage of max speed lost")]
    public float speedPenalty = 20f;

    AudioSource audioPlayer;
    public AudioClip collisionAudio;
    Rigidbody rb;

    void Start()
    {
        audioPlayer = gameObject.AddComponent<AudioSource>();
        audioPlayer.spatialBlend = 0.8f;
        audioPlayer.volume = 0.8f;
        //audioPlayer = gameObject.AddComponent(typeof(AudioSource)) as AudioSource;

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collisionAudio != null)
        {
            if (rb.linearVelocity.magnitude > 2f)
            {
                audioPlayer.pitch = Random.Range(0.9f, 1.1f);
                audioPlayer.PlayOneShot(collisionAudio);
            }
        }
    }
}
