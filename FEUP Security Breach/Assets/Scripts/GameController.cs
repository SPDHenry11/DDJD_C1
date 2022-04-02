using UnityEngine;

/// <summary>
/// Manages the overall variables that affect the game flow
/// </summary>
public class GameController : MonoBehaviour
{
    public static GameController instance;
    [HideInInspector] public static bool imunity=false;
    public CheckPoint checkPoint;
    void Awake()
    {
        instance = this;
        imunity=false;
    }
    public void SetCheckPoint(CheckPoint newCkeckPoint)
    {
        Destroy(checkPoint.gameObject);
        checkPoint = newCkeckPoint;
    }
}
