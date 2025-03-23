using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreTextBehaviour : Singleton<ScoreTextBehaviour>, IScoreDisplay
{
    public float animationDuration = 0.5f;
    private int currentValue = 0; // The current score displayed

    private TextMeshProUGUI m_Text;
    private Animator m_Animator;

    void Start()
    {
        Initialize();
        m_Text.text = currentValue.ToString(); // Set the initial score display
    }

    public void Initialize()
    {
        // Get the Animator component if not already assigned
        if (!m_Animator)
            m_Animator = GetComponent<Animator>();

        // Get the Text component if not already assigned
        if (!m_Text)
            m_Text = GetComponent<TextMeshProUGUI>();
    }

    // Plays the animation when the score is updated
    public void PlayScoreAnimation() => m_Animator?.Play("ScoreUpdated");

    // Updates the displayed score with animation
    public void UpdateScoreText()
    {
        PlayScoreAnimation();
        m_Text.text = currentValue.ToString();
    }

    // Increases the score and plays an animation
    public void IncreaseScore(int newValue)
    {
        StopAllCoroutines(); // Stop any ongoing animations
        StartCoroutine(AnimateValueIncrease(currentValue, currentValue + newValue, animationDuration));

        UpdateScoreText();
    }

    // Smoothly animates the score increase
    private IEnumerator AnimateValueIncrease(int startValue, int targetValue, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration; // Progress of the animation (0 to 1)
            int displayedValue = Mathf.RoundToInt(Mathf.Lerp(startValue, targetValue, t)); // Smooth transition
            m_Text.text = displayedValue.ToString();
            yield return null;
        }

        // Ensure the final value is correct
        currentValue = targetValue;
        m_Text.text = currentValue.ToString();
    }
}
