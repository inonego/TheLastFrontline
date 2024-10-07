using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class ShowFPS : MonoBehaviour
{
    private TextMeshProUGUI text;

    private float deltaTime;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

        float fps = 1.0f / deltaTime;

        text.text = Mathf.Ceil(fps).ToString();
    }
}