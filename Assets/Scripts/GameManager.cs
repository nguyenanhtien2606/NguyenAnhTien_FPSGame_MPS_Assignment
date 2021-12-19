using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GLOBAL { get; private set; }

    [SerializeField] WeaponController weaponController;

    [Space]
    [SerializeField] UIController uiController;

    [Header("GamePlay")]
    [SerializeField] List<PlayRound> playRounds;
    [SerializeField] float limitTimeSurvirval = 300;

    float timeLimit;

    public bool IsPlaying
    {
        get;
        set;
    }

    public bool IsGameOver
    {
        get;
        set;
    }

    public WeaponController P_WeaponController
    {
        get { return weaponController; }
        set { weaponController = value; }
    }

    public UIController P_UIController
    {
        get { return uiController; }
    }

    private void Awake()
    {
        GLOBAL = this;
    }

    private void Start()
    {
        timeLimit = limitTimeSurvirval;

        IsPlaying = true;
    }

    private void Update()
    {
        if (timeLimit <= 0 || IsGameOver)
        {
            IsPlaying = false;
        }

        if (IsPlaying)
        {
            timeLimit -= Time.deltaTime;
            if (timeLimit <= 0)
                timeLimit = 0;

            P_UIController.UpdateLimitTimeSurvival(ConvertTime(timeLimit));
        }
    }

    string ConvertTime(float timeSecs)
    {
        TimeSpan t = TimeSpan.FromSeconds(timeSecs);

        return string.Format("{0:D2}:{1:D2}:{2:D3}",
                        t.Minutes,
                        t.Seconds,
                        t.Milliseconds);


    }
}

[System.Serializable]
public class PlayRound
{
    public int amountOfEnemy;
}