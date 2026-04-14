using UnityEngine;

public class DialoguePoint : MonoBehaviour
{
    public AudioClip clip;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            VoiceManager.Instance.VoiceForceSam(clip, this.transform, 0.65f, 0f);
        }
    }
}
