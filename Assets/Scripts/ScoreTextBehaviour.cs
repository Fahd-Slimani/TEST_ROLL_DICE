using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreTextBehaviour : Singleton<ScoreTextBehaviour>, IScoreDisplay
{
    public float animationDuration = 0.5f; 
    private int currentValue = 0; // Tracks the displayed value


    private TextMeshProUGUI m_Text;
    private Animator m_Animator;

    void Start()
    {
        Initialize();

        m_Text.text = currentValue.ToString();
    }

    public void Initialize()
    {
        if (!m_Animator)
            m_Animator = GetComponent<Animator>();

        if (!m_Text)
            m_Text = GetComponent<TextMeshProUGUI>();
    }
    public void PlayScoreAnimation() => m_Animator?.Play("ScoreUpdated");

    public void UpdateScoreText()
    {
        PlayScoreAnimation();
        m_Text.text = currentValue.ToString();
    }
    public void IncreaseScore(int newValue)
    {
        StopAllCoroutines(); // Stop any ongoing animations
        StartCoroutine(AnimateValueIncrease(currentValue, currentValue + newValue, animationDuration));

        UpdateScoreText();
    }

    private IEnumerator AnimateValueIncrease(int startValue, int targetValue, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration; // Normalized time (0 to 1)
            int displayedValue = Mathf.RoundToInt(Mathf.Lerp(startValue, targetValue, t)); // Smooth transition
            m_Text.text = displayedValue.ToString();
            yield return null;
        }

        // Ensure the final value is set correctly
        currentValue = targetValue;
        m_Text.text = currentValue.ToString();
    }
}
