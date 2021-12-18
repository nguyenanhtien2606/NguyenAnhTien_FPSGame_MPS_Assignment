using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float jumpPower = 10f;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip walkAudio;
    [SerializeField] AudioClip runAudio;

    float gravityScale = 5;

    float applySpeed;
    bool isRunning;

    bool isGrounded;
    RaycastHit hit;
    float distanceCheckGround = 1.5f;

    #region Enable Observer Running Anim
    public static event Action<bool> IsWaking;
    public static event Action<bool> IsRunning;
    #endregion

    public bool IsCanMove
    {
        get;
        set;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        IsCanMove = true;

        applySpeed = walkSpeed;
    }

    private void Update()
    {
        if (IsCanMove)
        {
            float h_input = Input.GetAxis("Horizontal");
            float v_input = Input.GetAxis("Vertical");

            //Get any value of horizontal || vertical input => Move it
            if (h_input != 0 || v_input != 0)
            {
                transform.Translate(new Vector3(h_input, 0, v_input) * applySpeed * Time.deltaTime);
            }

            //Check ground
            GroundCheck();

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }

            if (Input.GetButton("Sprint") && isGrounded)
            {
                applySpeed = runSpeed;
                isRunning = true;
            }
            else
            {
                applySpeed = walkSpeed;
                isRunning = false;
            }

            IsRunning?.Invoke(v_input > 0 && isRunning); //Hey I'm running - only run forward
            IsWaking?.Invoke((h_input != 0 || v_input != 0) && !isRunning); //Hey I'm walking

            PlayFootStep((h_input != 0 || v_input != 0));
        }
    }

    private void FixedUpdate()
    {
        //Change gravity scale => fall down faster
        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);
    }

    void GroundCheck()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        isGrounded = Physics.Raycast(ray, out hit, distanceCheckGround);
        //Debug.DrawRay(transform.position, Vector3.down * distanceCheckGround, Color.red);
    }

    void PlayFootStep(bool isPlay)
    {
        if (isGrounded && isPlay)
        {
            audioSource.clip = (isRunning) ? runAudio : walkAudio;
            
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying)
                audioSource.Pause();
        }
    }
}
