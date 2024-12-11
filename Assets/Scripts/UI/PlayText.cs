using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayText : MonoBehaviour
{
    public TextMeshProUGUI TextUI;
    [TextArea(2, 5)]
    public string Text;

    public int TextPerSecond;

    private void OnEnable()
    {
        Play();
    }

    public void Play(string text)
    {
        Text = text;
        TextUI.text = "";
        
        Play();
    }

    public void Play()
    {
        StartCoroutine(PlayTextCoroutine());
    }

    IEnumerator PlayTextCoroutine()
    {
        for (int i = 0; i < Text.Length; i++)
        {
            TextUI.text += Text[i];
            yield return new WaitForSeconds(1f / TextPerSecond);
        }
    }

}
