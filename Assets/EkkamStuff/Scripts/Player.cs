using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Ekkam {
    public class Player : MonoBehaviour
    {

        public Transform orientation;
        public Transform cameraObj;
        public Transform playerObj;

        public Transform itemHolder;

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

        Inventory inventory;
        UIManager uiManager;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            inventory = FindObjectOfType<Inventory>();
            uiManager = FindObjectOfType<UIManager>();

            cameraObj = FindObjectOfType<Camera>().transform;
            itemHolder = GameObject.Find("ItemHolder").transform;

            gravity = -2 * jumpHeightApex / (jumpDuration * jumpDuration);
            initialJumpVelocity = Mathf.Abs(gravity) * jumpDuration;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            // horizontalInput = Input.GetAxis("Horizontal");
            // verticalInput = Input.GetAxis("Vertical");

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

            // Interact Prompts
            RaycastHit hit2;
            if (Physics.Raycast(cameraObj.position, cameraObj.forward * 2, out hit2, 2f))
            {
                switch (hit2.collider.tag)
                {
                    case "Lighter":
                        uiManager.ShowInteractPrompt("Pick up");
                        break;
                    case "Key":
                        uiManager.ShowInteractPrompt("Pick up");
                        break;
                    case "Fuel":
                        foreach (Item item in inventory.items)
                        {
                            if (item.tag == "Lighter")
                            {
                                uiManager.ShowInteractPrompt("Refuel lighter");
                            }
                        }
                        break;
                    case "Generator":
                        uiManager.ShowInteractPrompt("Fix generator");
                        break;
                    default:
                        break;
                }
            }
            else
            {
                uiManager.HideInteractPrompt();
            }
            Debug.DrawRay(cameraObj.position, cameraObj.forward * 2f, Color.green);

            // Cycle next or previous based on mouse scroll
            if (Input.mouseScrollDelta.y > 0)
            {
                inventory.CycleSlot(false);
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                inventory.CycleSlot(true);
            }

            if (Input.GetMouseButtonDown(0))
            {
                inventory.UseItem();
            }

        }

        void MovePlayer()
        {
            // Calculate movement direction
            // moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
            // moveDirection = cameraObj.forward * verticalInput + cameraObj.right * horizontalInput;
            moveDirection = new Vector3(cameraObj.forward.x * verticalInput, 0, cameraObj.forward.z * verticalInput) + new Vector3(cameraObj.right.x * horizontalInput, 0, cameraObj.right.z * horizontalInput);

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

        public void Interact()
        {
            RaycastHit hit;
            if (Physics.Raycast(cameraObj.position, cameraObj.forward * 2, out hit, 2f))
            {
                print(hit.collider.name);
                switch (hit.collider.tag)
                {
                    case "Lighter":
                        PickUp(hit.collider.GetComponent<Item>());
                        break;
                    case "Key":
                        PickUp(hit.collider.GetComponent<Item>());
                        break;
                    case "Fuel":
                        foreach (Item item in inventory.items)
                        {
                            if (item.tag == "Lighter")
                            {
                                item.GetComponent<Lighter>().Refuel();
                                Destroy(hit.collider.gameObject);
                            }
                        }
                        break;
                    case "Generator":
                        print("Fix generator");
                        // temporary ---
                        // find all doors with yellow locks and open them
                        foreach (Door door in FindObjectsOfType<Door>())
                        {
                            if (door.color == Door.doorColor.yellow)
                            {
                                door.Open();
                            }
                        }
                        // temporary ---
                        break;
                    default:
                        break;
                }
            }
        }

        public void InteractWithDoor()
        {
            RaycastHit hit;
            if (Physics.Raycast(cameraObj.position, cameraObj.forward * 2, out hit, 2f))
            {
                print(hit.collider.name);
                switch (hit.collider.tag)
                {
                    case "Door":
                        if (hit.collider.GetComponent<Door>().color == Door.doorColor.red)
                        {
                            if (HasValidKey(Door.doorColor.red))
                            {
                                print("has valid key");
                                hit.collider.GetComponent<Door>().Open();
                            }
                            else
                            {
                                print("no valid key");
                            }
                        }
                        else if (hit.collider.GetComponent<Door>().color == Door.doorColor.blue)
                        {
                            if (HasValidKey(Door.doorColor.blue))
                            {
                                print("has valid key");
                                hit.collider.GetComponent<Door>().Open();
                            }
                            else
                            {
                                print("no valid key");
                            }
                        }
                        break;
                    default:
                        print("no door");
                        break;
                }
            } 
        }

        public void PickUp(Item item)
        {
            inventory.AddItem(item);

            item.transform.SetParent(itemHolder);
            item.transform.localPosition = item.itemHolderPosition;
            item.transform.localRotation = Quaternion.identity;

            item.GetComponent<Collider>().enabled = false;
        }

        public bool HasValidKey(Door.doorColor color)
        {
            switch (color)
            {
                case Door.doorColor.red:
                    foreach (Item item in inventory.items)
                    {
                        if (item.tag == "Key" && item.GetComponent<Key>().color == Key.keyColor.red)
                        {
                            return true;
                        }
                    }
                    break;
                case Door.doorColor.blue:
                    foreach (Item item in inventory.items)
                    {
                        if (item.tag == "Key" && item.GetComponent<Key>().color == Key.keyColor.blue)
                        {
                            return true;
                        }
                    }
                    break;
                default:
                    break;
            }
            return false;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            horizontalInput = input.x;
            verticalInput = input.y;
        }

        public void OnCycleNext(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                inventory.CycleSlot(true);
            }
        }

        public void OnCyclePrevious(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                inventory.CycleSlot(false);
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Interact();
            }
        }
    }
}
