using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GLOBAL { get; private set; }

    [SerializeField] WeaponController weaponController;

    [Space]
    [SerializeField] UIController uiController;

    [Header("GamePlay")]
    [SerializeField] List<TargetReleaseController> targetPoints;
    [SerializeField] float limitTimeSurvirval = 120;

    float timeLimitRemain;
    int targetPointRemain;

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
        if (GLOBAL == null)
            GLOBAL = this;

        Time.timeScale = 1;
    }

    private void Start()
    {
        timeLimitRemain = limitTimeSurvirval;
        targetPointRemain = 0;
        

        IsPlaying = true;

        //Update origin UI
        P_UIController.UpdateTargetPointRelease(string.Format("{0}/{1}", targetPointRemain, targetPoints.Count));
    }

    private void OnEnable()
    {
        PlayerController.IsDie += PlayerDied;
    }

    private void OnDisable()
    {
        PlayerController.IsDie -= PlayerDied;
    }

    private void Update()
    {
        if (timeLimitRemain <= 0 || IsGameOver)
        {
            IsPlaying = false;
        }

        if (IsPlaying)
        {
            timeLimitRemain -= Time.deltaTime;
            if (timeLimitRemain <= 0 && targetPointRemain < targetPoints.Count)
            {
                timeLimitRemain = 0;
                Lose();
            }

            P_UIController.UpdateLimitTimeSurvival(ConvertTime(timeLimitRemain));
        }
    }

    public void UpdateTargetRelease()
    {
        targetPointRemain += 1;
        P_UIController.UpdateTargetPointRelease(string.Format("{0}/{1}", targetPointRemain, targetPoints.Count));

        if (targetPointRemain == targetPoints.Count)
        {
            Win();
        }
    }

    public void PlusTimeSurvival(float timePlus)
    {
        timeLimitRemain += timePlus;
    }

    void Win()
    {
        Debug.Log("You Win!");
        IsPlaying = false;

        Time.timeScale = 0;
        P_UIController.DisplayResultPanel("You Win!");
    }

    void Lose()
    {
        Debug.Log("You Lose!");
        IsPlaying = false;

        Time.timeScale = 0;
        P_UIController.DisplayResultPanel("You Lose!");
    }

    void PlayerDied()
    {
        StartCoroutine(C_DelayPlayerDied());
    }

    IEnumerator C_DelayPlayerDied()
    {
        yield return new WaitForSeconds(1);
        Lose();
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Main");
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