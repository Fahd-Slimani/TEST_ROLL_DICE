public enum Gameplay
{
    // Main gameplay mode
    Collect_6, // Score increases by 1 only when the dice roll is a 6

    // Extra gameplay mode: Exemple of an idea : Goal is to reach the highest score in a set time (exp 1 minute sessions)
    Collect_all, // Score increases with every dice roll. If the roll is 6, an extra point is added.
}

// Extra scoring options (optional)
public enum GameplayOptions
{
    None, // Default (no extra scoring)
    Collect_consecutives // Score increases by 1 for consecutive rolls.  
                         // If two 6s are rolled in a row, an extra point is added.
}