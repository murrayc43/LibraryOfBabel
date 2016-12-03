using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float speed = 6f;
    public float jumpSpeed = 8f;
    public float gravity = 20f;
    private bool isJumping = false;
    private CharacterController controller;
    private Camera mainCamera;
    private Vector3 moveVector = Vector3.zero;


    #region Start
    /// <summary>
    /// This is run once at the beginning of the game.
    /// </summary>
    public void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = transform.FindChild("Camera").GetComponent<Camera>();
    }
    #endregion


    #region Update
    /// <summary>
    /// This is run once every frame.
    /// </summary>
    public void Update()
    {
        if (mainCamera.cursorLock == CursorLockMode.Locked)
            Movement();
    }
    #endregion


    #region Movement
    /// <summary>
    /// Controls the movement of the player.
    /// </summary>
    private void Movement()
    {
        if (controller.isGrounded)
        {
            moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveVector = transform.TransformDirection(moveVector);
            if (Input.GetButton("Run"))
                moveVector *= speed * 2;
            else
                moveVector *= speed;
            isJumping = false;
            if (Input.GetButton("Jump"))
            {
                moveVector.y = jumpSpeed;
                isJumping = true;
            }
        }
        else
        {
            Vector3 fallingVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime;
            fallingVector = transform.TransformDirection(fallingVector);
            fallingVector *= speed;
            moveVector.x *= 0.99f;
            moveVector.z *= 0.99f;
            moveVector += fallingVector;
        }
        moveVector.y -= gravity * Time.deltaTime;
        controller.Move(moveVector * Time.deltaTime);
        SlopeCorrection();
    }
    #endregion


    #region Slope Correction
    /// <summary>
    /// Corrects the players movement when walking down a slope (corrects the bouncing).
    /// </summary>
    private void SlopeCorrection()
    {
        if (isJumping)
            return;

        Vector3 ray = transform.TransformDirection(Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, ray, out hit))
        {
            if (hit.distance < controller.height)
                controller.Move(new Vector3(0, -hit.distance, 0));
        }
    }
    #endregion
}