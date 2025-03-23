using TMPro;
using UnityEngine;

public class AdditionalScoreTextBehaviour : MonoBehaviour, IScoreDisplay
{
    private Animator m_Animator;
    private TextMeshProUGUI m_Text;

    public int scoreToAdd;

    // Initialize components at the start
    void Start() => Initialize();

    public void Initialize()
    {
        // Get the Animator component if not already assigned
        if (!m_Animator)
            m_Animator = GetComponent<Animator>();

        // Get the Text component if not already assigned
        if (!m_Text)
            m_Text = GetComponent<TextMeshProUGUI>();
    }

    // Updates the displayed score and plays an animation
    public void SetAdditionalScore(int additionalScore)
    {
        scoreToAdd = additionalScore;
        UpdateScoreText();
        PlayScoreAnimation();
    }

    // Updates the text to show the additional score
    public void UpdateScoreText() => m_Text.text = "+" + scoreToAdd.ToString();

    // Plays the animation for displaying the score
    public void PlayScoreAnimation() => m_Animator?.Play("ShowSpecialScoreText");
}
