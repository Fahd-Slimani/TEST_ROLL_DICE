public enum Gameplay
{
    // DEFAULT : Required gameplay
    Collect_6, // the score increases by 1 only if the dice's roll is 6

    // BONUS Potenial gamplay : IDEA = Reach the maximum score in a defined time (exp : 1 minute)
    Collect_all, // the score keep increasing by every dice's roll, if the roll is 6, an additional point is gained
}

// OPTIONAL/ADDITIONAL EXTRA SCORING
public enum GameplayOptions
{
    None, // DEFAULT
    Collect_consecutives // the score increases by 1 only if we have a consecutive dice rolls,
                         // in case of consecutive 6, an additional point is gained
}
