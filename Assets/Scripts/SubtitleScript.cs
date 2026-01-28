using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleScript : MonoBehaviour
{
    Text text;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    public IEnumerator playSubtitle(string[] dialogue, float wait)
    {
        WaitForSeconds w = new WaitForSeconds(wait);
        foreach (string s in dialogue)
        {
            text.text = s;
            yield return w;
        }
    }
}
