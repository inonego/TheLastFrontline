using UnityEngine;
using TMPro;

public class TitleGlow : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    private Material textMaterial;
    private float glowIntensity = 1f;
    private bool increasing = true;
    public float glowSpeed = 5f;  // 번쩍이는 속도 조절

    void Start()
    {
        // 텍스트의 재질(Material)을 가져옴
        textMaterial = titleText.fontMaterial;
    }

    void Update()
    {
        // 발광 효과가 서서히 커지고 작아지게 애니메이션
        if (increasing)
        {
            glowIntensity += Time.deltaTime * glowSpeed;
            if (glowIntensity > 2f) increasing = false;
        }
        else
        {
            glowIntensity -= Time.deltaTime * glowSpeed;
            if (glowIntensity < 1f) increasing = true;
        }

        // TextMeshPro의 Glow Intensity 설정
        textMaterial.SetFloat(ShaderUtilities.ID_GlowPower, glowIntensity);
    }
}
