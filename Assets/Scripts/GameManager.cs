using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Gameplay currentGameplay = Gameplay.Collect_6;
    public int Score { get; private set; }

    // UI texts 
    [SerializeField]
    private AdditionalScoreTextBehaviour additionalScoreText;

    void Start()
    {
        Score = 0;
        additionalScoreText = additionalScoreText ?? 
            FindFirstObjectByType<AdditionalScoreTextBehaviour>();

        Dice.Instance.OnDiceRolled += UpdateScore;
    }

    // Update the score based on the dice result
    private void UpdateScore(int diceResult) => Collect_Only_6(diceResult);

    private void Collect_Only_6(int diceRoll)
    {
        if (diceRoll == 6)
        {
            IncreaseScore(1);
            VisualFeedback(1);
        }
    }

    private void IncreaseScore(int newPoints)
    {
        ScoreTextBehaviour.Instance.IncreaseScore(newPoints);
        Score += newPoints;
    }

    private void VisualFeedback(int gain) => additionalScoreText.SetAdditionalScore(gain);  

    // Unsubscribe when this object is destroyed (to avoid memory leaks)
    void OnDestroy() => Dice.Instance.OnDiceRolled -= UpdateScore;

}

