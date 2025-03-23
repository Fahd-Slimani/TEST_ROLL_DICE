
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Gameplay currentGameplay = Gameplay.Collect_6;
    public GameplayOptions gamePlayOption = GameplayOptions.None;

    public int Score { get; private set; }

    // UI texts 
    [SerializeField]
    private AdditionalScoreTextBehaviour additionalScoreText;

    void Start()
    {
        Score = 0;
        additionalScoreText = additionalScoreText ?? FindFirstObjectByType<AdditionalScoreTextBehaviour>();

        Dice.Instance.OnDiceRolled += UpdateScore;
    }

    // Update the score based on the dice result
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
            default: Collect_Only_6(diceResult);
                break;
        }
    }

    private void Collect_Only_6(int diceRoll)
    {
        int optionPoints = CheckOptions(diceRoll);

        if (diceRoll == 6)
        {
            IncreaseScore(1 + optionPoints);
            VisualFeedback(1 + optionPoints);
        }

        else // the option case of consecutive rolls different than 6
        {
            if (optionPoints > 0)
            {
                IncreaseScore(optionPoints);
                additionalScoreText.SetAdditionalScore(optionPoints);
            }
        }
    }

    private void Collect_All(int diceRoll)
    {
        int optionPoints = CheckOptions(diceRoll);

        if (diceRoll == 6)
        {
            diceRoll++;

            VisualFeedback((1 + optionPoints));
            IncreaseScore(diceRoll + optionPoints);
        }
        else // the option case of consecutive rolls different than 6
        {
            if (optionPoints > 0)
            {
                IncreaseScore(optionPoints);
                additionalScoreText.SetAdditionalScore(optionPoints);
            }
        }
    }

    private int CheckOptions(int diceRoll)
    {
        int optionPoints = 0;
        if (gamePlayOption == GameplayOptions.Collect_consecutives)
            optionPoints = Collect_Consecutives(diceRoll);

        return optionPoints;
    }

    // OPTION : returns 1 if consecutive, 2 if consecutive 6 and 0 if none
    private int Collect_Consecutives(int diceRoll)
    {
        int score = 0;
            if (diceRoll == Dice.Instance.lastDiceRoll)
            {
            // Debug.Log("CONSECUTIVE")
                score = 1;

                if (diceRoll == 6)
                    score++;
            }
        return score;
    }

    private void IncreaseScore(int newPoints)
    {
        ScoreTextBehaviour.Instance.IncreaseScore(newPoints);
        Score += newPoints;
    }

    private void VisualFeedback(int gain)
    {
        Dice.Instance.GetComponent<ShakingBehaviour>().ShakeDice();
        additionalScoreText.SetAdditionalScore(gain);    
    }

    // Unsubscribe when this object is destroyed (to avoid memory leaks)
    void OnDestroy() => Dice.Instance.OnDiceRolled -= UpdateScore;

}

