using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [SerializeField] private CheckPoint checkPoint;
    void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) checkPoint.Restart(); //For test purposes
    }

    public void SetCheckPoint(CheckPoint newCkeckPoint)
    {
        Destroy(checkPoint.gameObject);
        checkPoint = newCkeckPoint;
    }
    public void RestartFromCheckpoint()
    {
        //Do stuff before restarting (Fade out -> death message (BUSTED?))
        checkPoint.Restart();
    }
}
