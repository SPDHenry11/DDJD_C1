[System.Serializable]
/// <summary>
/// Stats object for saving the highscore
/// </summary>
public class Stats
{
    public float bestTime;

    public Stats(float time)
    {
        bestTime = time;
    }
}
