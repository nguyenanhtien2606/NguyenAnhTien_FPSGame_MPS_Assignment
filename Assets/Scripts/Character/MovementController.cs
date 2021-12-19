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

    [Header("JoyStick")]
    [SerializeField] FixedJoystick fixedJoystick;

    float gravityScale = 5;

    float applySpeed;
    bool isRunning;

    bool isJumpTrigger;

    bool isGrounded;
    RaycastHit hit;
    float distanceCheckGround = 1.5f;

    #region Enable Observer Running Anim
    public static event Action<bool> IsWaking;
    public static event Action<bool> IsRunning;
    #endregion


#if UNITY_ANDROID || UNITY_IPHONE
    //Mobile Input
    bool mobile_IsSprint;
    bool mobile_IsJump;
    bool mobile_IsFire;
#endif


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
#if UNITY_STANDALONE || UNITY_EDITOR
            float h_input = Input.GetAxis("Horizontal");
            float v_input = Input.GetAxis("Vertical");
#elif UNITY_ANDROID || UNITY_IPHONE
            float h_input = fixedJoystick.Horizontal;
            float v_input = fixedJoystick.Vertical;
#endif

            //Get any value of horizontal || vertical input => Move it
            if (h_input != 0 || v_input != 0)
            {
                transform.Translate(new Vector3(h_input, 0, v_input) * applySpeed * Time.deltaTime);
            }

            //Check ground
            GroundCheck();

            if (isGrounded)
            {
#if UNITY_STANDALONE
                if (Input.GetButtonDown("Jump"))
                {
                    rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                }

                if (Input.GetButton("Sprint"))
                {
                    applySpeed = runSpeed;
                    isRunning = true;
                }
                else
                {
                    applySpeed = walkSpeed;
                    isRunning = false;
                }
#elif UNITY_ANDROID || UNITY_IPHONE || UNITY_EDITOR
                if (mobile_IsJump)
                {
                    rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                    isJumpTrigger = true;
                    mobile_IsJump = false;
                }

                if (mobile_IsSprint)
                {
                    applySpeed = runSpeed;
                    isRunning = true;
                }
                else
                {
                    applySpeed = walkSpeed;
                    isRunning = false;
                }
#endif
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

        if (isGrounded && isJumpTrigger)
        {
            GameManager.GLOBAL.P_UIController.SetActiveMobileJump(true);
            isJumpTrigger = false;
        }
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


#if UNITY_ANDROID || UNITY_IPHONE

    public void SetSprint(bool isSprint)
    {
        mobile_IsSprint = isSprint;
    }

    public void SetJump(bool isJump)
    {
        mobile_IsJump = isJump;
        GameManager.GLOBAL.P_UIController.SetActiveMobileJump(false);
    }
#endif
}
