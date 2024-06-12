using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : MonoBehaviour
{
    //fro player getting hit
    public delegate void playerHitAction();
    public static event playerHitAction OnPlayerHit;

    //for player crossing finish line
    public delegate void playerFinishAction();
    public static event playerFinishAction OnFinishLine;
    
    //for player crossing start line
    public delegate void playerStartAction();
    public static event playerStartAction OnStartLine;

    //for missing time penalty flags
    public delegate void playerPenaltyAction();
    public static event playerPenaltyAction OnTimePenalty;

    public static void PlayerHit()
    {
        if (OnPlayerHit != null)
        {
            OnPlayerHit();
            Debug.Log("Hit Trigger Executed");
        }
    }

    public static void FinishLine()
    {
        if (OnFinishLine != null)
        {
            OnFinishLine();
            Debug.Log("Finish Trigger Executed");
        }
    }

    public static void StartLine()
    {
        if (OnStartLine != null)
        {
            OnStartLine();
            Debug.Log("Start Trigger Executed");
        }
    }

    public static void TimePenalty()
    {
        if (OnStartLine != null)
        {
            OnTimePenalty();
            Debug.Log("Penalty Trigger Executed");
        }
    }
}