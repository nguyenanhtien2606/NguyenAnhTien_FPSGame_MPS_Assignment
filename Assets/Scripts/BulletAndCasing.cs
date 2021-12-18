using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAndCasing : MonoBehaviour
{
    public Rigidbody Rb
    {
        get;
        set;
    }

    public void Disable()
    {
        Rb.velocity = Vector3.zero;
        gameObject.SetActive(false);
        transform.localPosition = Vector3.zero;
    }
}
