using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ThirdPersonControllerScript : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] Transform cam;

    public float speed = 6f;
    [SerializeField] float turnSmoothTime = .1f;
    float turnSmoothVelocity;

    [SerializeField] float gravity = -9.81f;
    Vector3 velocity;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = .4f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float gravityScale = 5f;
    [SerializeField] float jumpHeight = 3f;
    bool isGrounded;

    PhotonView view;
    
    [SerializeField] GameObject[] destroyOnLoad;

    Animator anim;
    public AudioClip[] FootstepAudioClips;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
        if (!view.IsMine)
        {
            for (int i = 0; i < destroyOnLoad.Length; i++)
            {
                Destroy(destroyOnLoad[i]);
            }
        }
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        Gravity();

        if (view.IsMine)
        {
            Movement();
            Jumping();
        }
    }

    //Player's movement logic
    void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if (direction.magnitude >= .1f)
        {

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
        anim.SetFloat("run", direction.magnitude);
    }

    //Gravity logic
    void Gravity()
    {
        velocity.y += gravity * gravityScale * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    //Player's jumping logic
    void Jumping()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance,groundMask);
        anim.SetBool("isJumping", !isGrounded);
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

    }
    private void OnFootstep(AnimationEvent animationEvent)
    {
        print("Fooot step");
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(controller.center), .5f);
            }
        }
    }
}
