using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Observer : MonoBehaviour
{
    public static Action<int, Vector3> AddScore;// int = score, Vector3 = playerPos
    public static Action<bool> EndGame;//bool = isWin
    public static Action StartGame;
    public static Action ÑallNewPath;
    public static Action<SplinePath> SetNewPath;
    public static Action PlayerAnimJump;
    public static Action PlayerAnimDown;
    public static Action<float, float> ResetPositionAllYZ;
    public static Action<int> SetCountSplines;
    //call by button on scene
    public void CallStartGame()
    {
        StartGame();
    }
    private void OnDestroy()
    {
        AddScore = null;
        EndGame = null;
        StartGame = null;
        ÑallNewPath = null;
        SetNewPath = null;
        PlayerAnimJump = null;
        PlayerAnimDown = null;
        ResetPositionAllYZ = null;
        SetCountSplines = null;
    }
}
