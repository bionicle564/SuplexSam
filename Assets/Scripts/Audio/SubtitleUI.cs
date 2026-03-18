using TMPro;
using UnityEngine;

public class SubtitleUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI subtitleText;

    public static SubtitleUI instance;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        
    }

    public void SetSubtitle(string subtitle)
    {
        subtitleText.text = subtitle;
    }
}
