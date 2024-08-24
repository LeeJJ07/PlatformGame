using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlManager : MonoBehaviour
{
    public float rotationSpeed = 720;
    public float jumpSpeed = 5;

    public float jumpButtonGracePeriod = 0.2f;

    public Transform cameraTransform;

    private CharacterController characterController;
    private Animator animator;

    private float ySpeed;
    private float originalStepOffset;

    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;

    private bool isJumping;
    private bool isGrounded;
    public float jumpHorizontalSpeed = 3;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        originalStepOffset = characterController.stepOffset;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);

        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            inputMagnitude *= 0.5f;
        }

        animator.SetFloat("Input Magnitude", inputMagnitude, 0.05f, Time.deltaTime);

        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
        }
        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }

        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.8f;

            animator.SetBool("isGround", true);
            isGrounded = true;
            animator.SetBool("isJumping", false);
            isJumping = false;
            animator.SetBool("isFalling", false);

            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = jumpSpeed;

                animator.SetBool("isJumping", true);
                isJumping = true;

                lastGroundedTime = null;
                jumpButtonPressedTime = null;
            }
        }
        else
        {
            characterController.stepOffset = 0;

            animator.SetBool("isGrounded", false);
            isGrounded = false;

            /* 낙하 애니메이션 전환 처리 */
            if ((isJumping && ySpeed < 0) || (ySpeed < -2))
            {
                animator.SetBool("isFalling", true);
            }
        }

        if (movementDirection != Vector3.zero)
        {
            animator.SetBool("isMoving", true);
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        if (isGrounded == false)
        {
            Vector3 velocity = movementDirection * inputMagnitude * jumpHorizontalSpeed;
            velocity.y = ySpeed;

            characterController.Move(velocity * Time.deltaTime);
        }
    }

    private void OnAnimatorMove()
    {
        Vector3 velocity = animator.deltaPosition;
        velocity.y = ySpeed * Time.deltaTime;

        characterController.Move(velocity);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}