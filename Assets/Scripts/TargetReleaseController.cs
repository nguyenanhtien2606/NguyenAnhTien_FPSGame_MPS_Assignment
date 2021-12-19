using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetReleaseController : MonoBehaviour
{
    [SerializeField] Material lockMar;
    [SerializeField] Material releaseMar;

    [SerializeField] GameObject marker;

    [SerializeField] float timeNeedToRelease;


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
