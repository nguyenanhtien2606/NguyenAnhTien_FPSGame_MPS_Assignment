using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    [Header("The Character")]
    [SerializeField] GameObject thePlayer;

    [Header("Sensitivity Setting")]
    [SerializeField] float X_sensitivity = 100f;
    [SerializeField] float Y_sensitivity = 100f;

    [Header("Looking Range Setting")]
    [SerializeField] float maximumX = 60f;
    [SerializeField] float minimumX = -60f;

    float rotationX;

    public static event Action<float> MouseRotate;

    public bool IsCanRotate
    {
        get;
        set;
    }

    private void Start()
    {
        IsCanRotate = true;

        //Hide and lock the cursor in center screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (IsCanRotate)
        {
            float mouseX = Input.GetAxis("Mouse X") * X_sensitivity * Time.deltaTime;

            rotationX += Input.GetAxis("Mouse Y") * Y_sensitivity * Time.deltaTime;
            rotationX = Mathf.Clamp(rotationX, minimumX, maximumX);

            //Look Up and down with mouse Y input
            transform.localRotation = Quaternion.Euler(-rotationX, 0, 0);

            //Rotate The Player with mouse X input
            thePlayer.transform.Rotate(Vector3.up * mouseX);
        }
    }
}
