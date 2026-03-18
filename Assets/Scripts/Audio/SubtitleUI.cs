using System.Collections;
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

    public void SetSubtitle(string subtitle, float delay)
    {
        subtitleText.text = subtitle;

        StartCoroutine(ClearAfterSeconds(delay));
    }

    public void ClearSubtitle()
    {
        subtitleText.text = "";
    }

    private IEnumerator ClearAfterSeconds(float delay)
    {
        yield return new WaitForSeconds(delay);
        ClearSubtitle();
    }
}
