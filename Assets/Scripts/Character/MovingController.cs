using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingController : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] float speed = 10f;
    [SerializeField] float jumpPower = 10f;

    [Header("Animator")]
    [SerializeField] Animator anim;

    float gravityScale = 5;

    bool isGrounded;
    RaycastHit hit;
    float distanceCheckGround = 1.5f;

    public bool IsCanMove
    {
        get;
        set;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        IsCanMove = true;
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
                transform.Translate(new Vector3(h_input, 0, v_input) * speed * Time.deltaTime);
            }

            //Check ground
            Ray ray = new Ray(transform.position, Vector3.down);
            isGrounded = Physics.Raycast(ray, out hit, distanceCheckGround);
            //Debug.DrawRay(transform.position, Vector3.down * distanceCheckGround, Color.red);

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }

            anim.SetBool("IsRunning", v_input > 0 && isGrounded);
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);
    }


}
