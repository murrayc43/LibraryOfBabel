using UnityEngine;
using BigIntegerType;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Variables
    private float speed = 6f;
    private float jumpSpeed = 8f;
    private float gravity = 20f;
    private bool isJumping = false;
    private CharacterController controller;
    private Library library;
    private Vector3 moveVector = Vector3.zero;
    private Algorithm algorithm;

    public Text currentLocation;
    public CameraController mainCamera;
    public GameObject TeleportInput;
    #endregion

    #region Start
    /// <summary>
    /// This is run once at the beginning of the game.
    /// </summary>
    public void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = transform.FindChild("Camera").GetComponent<CameraController>();
        library = GameObject.FindGameObjectWithTag("Player").GetComponent<Library>();
        algorithm = GameObject.FindGameObjectWithTag("Algorithm").GetComponent<Algorithm>();
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

        //DEBUGGING ONLY
        if (Input.GetKeyDown(KeyCode.T))
            TeleportInput.SetActive(true);
        if (Input.GetKeyDown(KeyCode.P))
            print("Current floor: " + library.currentFloor);
    }
    #endregion

    #region Fixed Update
    /// <summary>
    /// This is run once every physics frame.
    /// </summary>
    public void FixedUpdate()
    {
        if (mainCamera.cursorLock == CursorLockMode.Locked)
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

                    if (Input.GetMouseButton(0))
                    {
                        mainCamera.cursorLock = CursorLockMode.None;

                        algorithm.floor = library.currentFloor;
                        algorithm.sector = indexer.sector;
                        algorithm.bookcase = indexer.bookcase;
                        algorithm.shelf = shelf;
                        algorithm.book = book;

                        algorithm.generateBook();
                    }
                }
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