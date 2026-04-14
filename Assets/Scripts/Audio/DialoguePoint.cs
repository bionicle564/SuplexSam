using UnityEngine;

public class DialoguePoint : MonoBehaviour
{
    public AudioClip clip;
    bool triggered = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !triggered)
        {
            triggered = true;
            VoiceManager.Instance.VoiceForceSam(clip, other.transform, 0.7f, 0f);
        }
    }
}
