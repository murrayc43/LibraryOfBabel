using UnityEngine;
using System.Collections;
using BigIntegerType;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Text currentLocation;
    
    private float speed = 6f;
    private float jumpSpeed = 8f;
    private float gravity = 20f;
    private bool isJumping = false;
    private CharacterController controller;
    private Library library;
    private CameraController mainCamera;
    private Vector3 moveVector = Vector3.zero;

    #region Start
    /// <summary>
    /// This is run once at the beginning of the game.
    /// </summary>
    public void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = transform.FindChild("Camera").GetComponent<CameraController>();
        library = GameObject.FindGameObjectWithTag("Player").GetComponent<Library>();
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

    #region Fixed Update
    /// <summary>
    /// This is run once every physics frame.
    /// </summary>
    public void FixedUpdate()
    {
        currentLocation.text = "";

        Vector3 forward = mainCamera.transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position, forward, out hit, 5))
        {
            if (hit.transform.tag == "Book")
            {
                Indexer indexer = hit.transform.parent.parent.parent.GetComponent<Indexer>();
                string[] bookNameParts = hit.transform.name.Split(' ');
                int book = int.Parse(bookNameParts[1]);
                int shelf = book / 100;
            
                currentLocation.text = ("Floor: " + library.currentFloor + ", Sector: " + indexer.sector + ", Bookcase: " + indexer.bookcase + ", Shelf: " + shelf + ", Book: " + book);
                BigInteger bookIndex = (library.currentFloor * 76800) + (indexer.sector * 19200) + (indexer.bookcase * 400) + book;
                print("Book Index: " + bookIndex);
            }
        }
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