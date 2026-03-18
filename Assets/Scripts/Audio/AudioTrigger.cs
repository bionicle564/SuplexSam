using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    public AudioObject clipToPlay;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vocals.instance.Say(clipToPlay);
        }
    }
}
