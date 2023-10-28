using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ekkam {
    public class Player : MonoBehaviour
    {

        public Transform orientation;
        public Transform cameraObj;
        public Transform playerObj;

        public float rotationSpeed = 3f;

        float horizontalInput;
        float verticalInput;

        Vector3 moveDirection;

        public Rigidbody rb;
        public Animator anim;

        public float jumpHeightApex = 2f;
        public float jumpDuration = 1f;

        float currentJumpDuration;

        public float downwardsGravityMultiplier = 1f;

        public float speed = 1.0f;
        public float maxSpeed = 5.0f;
        public float groundDrag = 3;

        public bool isJumping = false;
        public bool hasLanded = true;
        public bool isGrounded;

        float gravity;
        float initialJumpVelocity;
        float jumpStartTime;

        public float groundDistance = 1f;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();

            cameraObj = FindObjectOfType<Camera>().transform;

            gravity = -2 * jumpHeightApex / (jumpDuration * jumpDuration);
            initialJumpVelocity = Mathf.Abs(gravity) * jumpDuration;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            playerObj.transform.rotation = Quaternion.Slerp(
                playerObj.transform.rotation,
                Quaternion.LookRotation(new Vector3(cameraObj.transform.forward.x, 0, cameraObj.transform.forward.z)),
                Time.deltaTime * rotationSpeed
            );
            
            if(moveDirection != Vector3.zero)
            {
                anim.SetBool("walking", true);
            }
            else
            {
                anim.SetBool("walking", false);
            }

            // Ground check
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), Vector3.down, out hit, groundDistance + 0.1f))
            {
                isGrounded = true;

                if (!hasLanded)
                {
                    hasLanded = true;
                }
            }
            else
            {
                isGrounded = false;
            }

            Debug.DrawRay(transform.position + new Vector3(0, 1, 0), Vector3.down * (groundDistance + 0.1f), Color.red);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isGrounded)
                {
                    StartJump(jumpHeightApex, jumpDuration);
                }
            }

            if (isGrounded && !isJumping)
            {
                anim.SetBool("isJumping", false);
            }

            if (isGrounded)
            {
                rb.drag = groundDrag;
            }
            else
            {
                rb.drag = 0;
            }

            // Limit velocity
            ControlSpeed();

        }

        void MovePlayer()
        {
            // Calculate movement direction
            // moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
            moveDirection = cameraObj.forward * verticalInput + cameraObj.right * horizontalInput;

            rb.AddForce(moveDirection * speed * 10f, ForceMode.Force);
        }

        void ControlSpeed()
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            // Limit velocity if needed
            if (flatVelocity.magnitude > maxSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * maxSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }

        void FixedUpdate()
        {

            // Move player
            MovePlayer();

            // Jumping
            if (isJumping)
            {
                rb.AddForce(Vector3.up * gravity, ForceMode.Acceleration);

                if (Time.time - jumpStartTime >= currentJumpDuration)
                {
                    isJumping = false;
                    hasLanded = false;
                }
            }
            else if (!isGrounded)
            {
                rb.AddForce(Vector3.down * -gravity * downwardsGravityMultiplier, ForceMode.Acceleration);
            }
        }

        void StartJump(float heightApex, float duration)
        {
            // Recalculate gravity and initial velocity
            gravity = -2 * heightApex / (duration * duration);
            initialJumpVelocity = Mathf.Abs(gravity) * duration;
            currentJumpDuration = duration;

            isJumping = true;
            anim.SetBool("isJumping", true);
            jumpStartTime = Time.time;
            rb.velocity = Vector3.up * initialJumpVelocity;
        }
    }
}
