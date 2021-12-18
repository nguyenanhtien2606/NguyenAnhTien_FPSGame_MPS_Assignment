using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasingBehavior : BulletAndCasing
{
    private void Awake()
    {
        Rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        StartCoroutine(C_Disable());
    }

    IEnumerator C_Disable()
    {
        yield return new WaitForSeconds(3);
        Disable();
    }
}
