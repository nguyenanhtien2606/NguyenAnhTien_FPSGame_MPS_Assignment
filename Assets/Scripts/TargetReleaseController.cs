using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetReleaseController : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] Material lockMar;
    [SerializeField] Material releaseMar;

    [SerializeField] GameObject marker;

    [SerializeField] float timeNeedToRelease;

    [SerializeField] float timePlus = 10;


    #region observer
    public static event Action<bool,float> UpdateReleaseTargetProcess;
    #endregion

    bool isUncloking;
    float countTime;
    bool isReleased;

    public float ReleaseProcess
    {
        get;
        private set;
    }

    private void Start()
    {
        gameManager = GameManager.GLOBAL;
        countTime = timeNeedToRelease;
    }

    private void Update()
    {
        if (!isReleased && isUncloking && countTime > 0)
        {
            countTime -= Time.deltaTime;
            ReleaseProcess = countTime / timeNeedToRelease;
            
            if(countTime <= 0)
            {
                isReleased = true;
                marker.GetComponent<MeshRenderer>().material = releaseMar;

                gameManager.UpdateTargetRelease();
                gameManager.PlusTimeSurvival(timePlus);
            }

            UpdateReleaseTargetProcess?.Invoke(true, ReleaseProcess);
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isUncloking = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isUncloking = false;
            UpdateReleaseTargetProcess?.Invoke(false, ReleaseProcess);
        }
    }
}
