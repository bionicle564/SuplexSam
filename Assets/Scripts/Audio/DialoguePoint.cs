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
            VoiceManager.Instance.VoiceForceSam(clip, this.transform, 0.65f, 0f);
        }
    }
}
