
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
        if (diceRoll == 6)
        {
            IncreaseScore(1);
            VisualFeedback(1);
        }
    }

    private void Collect_All(int diceRoll)
    {
        int gain = diceRoll;
        if (diceRoll == 6)
        {
            gain++;
            VisualFeedback((1));
        }
        IncreaseScore(gain);
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

