using TMPro;
using UnityEngine;

public class AdditionalScoreTextBehaviour : MonoBehaviour, IScoreDisplay
{
    private Animator m_Animator;
    private TextMeshProUGUI m_Text;

    public int scoreToAdd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() => Initialize();

    public void Initialize()
    {
        if (!m_Animator)
            m_Animator = GetComponent<Animator>();

        if (!m_Text)
            m_Text = GetComponent<TextMeshProUGUI>();
    }

    public void SetAdditionalScore(int additionalScore)
    {
        scoreToAdd = additionalScore;
        UpdateScoreText();
        PlayScoreAnimation();
    }

    public void UpdateScoreText() => m_Text.text = "+" + scoreToAdd.ToString();

    public void PlayScoreAnimation() => m_Animator?.Play("ShowSpecialScoreText");
}
