using UnityEngine;

// Manages the core gameplay mechanics and score tracking
public class GameManager : Singleton<GameManager>
{
    // Defines the current gameplay mode
    public Gameplay currentGameplay = Gameplay.Collect_6;
    public GameplayOptions gamePlayOption = GameplayOptions.None;

    public int Score { get; private set; }

    // UI element for displaying additional score feedback
    [SerializeField]
    private AdditionalScoreTextBehaviour additionalScoreText;

    void Start()
    {
        Score = 0;
        // Ensure additionalScoreText is assigned (fallback if not set in inspector)
        additionalScoreText = additionalScoreText ?? FindFirstObjectByType<AdditionalScoreTextBehaviour>();

        // Subscribe to dice roll event to update the score accordingly (Observer Pattern)
        Dice.Instance.OnDiceRolled += UpdateScore;
    }

    // Determines the score update logic based on the current gameplay mode
    private void UpdateScore(int diceResult)
    {
        switch (currentGameplay)
        {
            case Gameplay.Collect_6:
                Collect_Only_6(diceResult);
                break;
            case Gameplay.Collect_all:
                Collect_All(diceResult);
                break;
            default:
                Collect_Only_6(diceResult);
                break;
        }
    }

    // Handles score increase when only rolling a 6 counts
    private void Collect_Only_6(int diceRoll)
    {
        int optionPoints = CheckOptions(diceRoll);

        if (diceRoll == 6)
        {
            // Award points for rolling a 6, including option bonuses
            IncreaseScore(1 + optionPoints);
            VisualFeedback(1 + optionPoints);
        }
        else
        {
            // Handle additional points for special consecutive roll conditions
            if (optionPoints > 0)
            {
                IncreaseScore(optionPoints);
                additionalScoreText.SetAdditionalScore(optionPoints);
            }
        }
    }

    // Handles score increase when every dice roll counts
    private void Collect_All(int diceRoll)
    {
        int optionPoints = CheckOptions(diceRoll);

        if (diceRoll == 6)
        {
            diceRoll++; // Bonus increment for rolling a 6

            VisualFeedback(1 + optionPoints);
            IncreaseScore(diceRoll + optionPoints);
        }
        else
        {
            // Handle additional points for special consecutive roll conditions
            if (optionPoints > 0)
            {
                IncreaseScore(optionPoints);
                additionalScoreText.SetAdditionalScore(optionPoints);
            }
        }
    }

    // Checks if special gameplay options apply to modify score calculation
    private int CheckOptions(int diceRoll)
    {
        int optionPoints = 0;
        if (gamePlayOption == GameplayOptions.Collect_consecutives)
            optionPoints = Collect_Consecutives(diceRoll);

        return optionPoints;
    }

    // Awards extra points for consecutive rolls, with a bonus for consecutive sixes
    private int Collect_Consecutives(int diceRoll)
    {
        int score = 0;
        if (diceRoll == Dice.Instance.lastDiceRoll)
        {
            score = 1; // Base bonus for any consecutive roll

            if (diceRoll == 6)
                score++; // Additional bonus for consecutive sixes
        }
        return score;
    }

    // Updates the score UI and keeps track of the total score
    private void IncreaseScore(int newPoints)
    {
        ScoreTextBehaviour.Instance.IncreaseScore(newPoints);
        Score += newPoints;
    }

    // Provides visual feedback when points are gained
    private void VisualFeedback(int gain)
    {
        Dice.Instance.GetComponent<ShakingBehaviour>().ShakeDice();
        additionalScoreText.SetAdditionalScore(gain);
    }

    // Ensures proper cleanup by unsubscribing from the dice roll event
    void OnDestroy() => Dice.Instance.OnDiceRolled -= UpdateScore;
}
