using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptPlayer : MonoBehaviour
{
    [Serializable]
    public class KeyFrame : IComparable<KeyFrame>
    {
        [field: SerializeField] public float Start { get; set; }
        [field: SerializeField] public float End   { get; set; }

        [field: SerializeField] public string Text { get; set; }

        public int CompareTo(KeyFrame other)
        {
            return Start.CompareTo(other.Start);
        }

        public KeyFrame(float start, float end, string text)
        {
            Start = start; End = end; Text = text;
        }
    }
 
    public string CurrentText { get; private set; }

    public event Action<string> OnScriptTextChanged;

    public List<KeyFrame> KeyFrames = new();

    private int index = 0;

    public void AddKeyFrame(float start, float end, string text)
    {
        KeyFrames.Add(new ( start: start, end: end, text: text ));
    }

    private void Awake()
    {
        AddKeyFrame(start: 0f,  end: 5f,
        text: "The detonation device has been activated, but it's set to trigger in 5 minutes for safety.");

        AddKeyFrame(start: 5f,  end: 8f,
        text: "I have to hold off these monsters for the next 5 minutes.");
        
        AddKeyFrame(start: 8f,  end: 11f,
        text: "… My legs are shattered. I can barely move.");
        
        AddKeyFrame(start: 11f, end: 14f,
        text: "Damn it…");

        AddKeyFrame(start: 60f, end: 65f,
        text: "More enemies are closing in.");

        AddKeyFrame(start: 65f, end: 68f,
        text: "What are these things? What are they really?");

        AddKeyFrame(start: 68f, end: 71f,
        text: "What is their purpose for invading Earth over and over again?");
        
        AddKeyFrame(start: 120f, end: 125f,
        text: "This is an onslaught on a completely different scale.");

        AddKeyFrame(start: 125f, end: 128f,
        text: "This must be their final assault. If I can hold them off this time, victory will be ours.");

        KeyFrames.Sort((x,y) => x.CompareTo(y));
    }

    public void Update()
    {
        float time = GameManager.Instance.ElapsedTime;

        KeyFrame CurrentKeyFrame() => index < KeyFrames.Count ? KeyFrames[index] : null;

        if (CurrentKeyFrame() is not null && CurrentKeyFrame().Start <= time)
        {
            if (time <= CurrentKeyFrame().End)
            {
                ShowText(CurrentKeyFrame().Text);
            }
            else
            {
                index += 1;
            }
        }
        else
        {
            HideText();
        }
    }

    public void ShowText(string text)
    {
        if (CurrentText != text)
        {
            CurrentText = text;

            OnScriptTextChanged?.Invoke(text);
        }
    }

    public void HideText()
    {
        CurrentText = String.Empty;

        OnScriptTextChanged?.Invoke(String.Empty);
    }
}
