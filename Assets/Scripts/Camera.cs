using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour
{
    public CursorLockMode cursorLock;
    private float rotateSpeed = 2f;
    private Transform player;


    /****************************************************************************************************
     * Description:
     *      This is run once at the beginning of the game
     ****************************************************************************************************/
    public void Start()
    {
        player = transform.parent;
    }


    /****************************************************************************************************
     * Description:
     *      This is run once every frame
     ****************************************************************************************************/
    public void Update()
    {
        CursorState();
        if (cursorLock == CursorLockMode.Locked)
            Movement();
    }


    /****************************************************************************************************
     * Description:
     *      Controls the rotation of the camera and player
     * Syntax:
     *      Movement();
     ****************************************************************************************************/
    private void Movement()
    {
        float mouseInputX = Input.GetAxis("Mouse X");
        float mouseInputY = Input.GetAxis("Mouse Y");

        Vector3 lookXVector = new Vector3(0, mouseInputX, 0) * rotateSpeed;
        Vector3 lookYVector = new Vector3(-mouseInputY, 0, 0) * rotateSpeed;
        player.Rotate(lookXVector);
        transform.Rotate(lookYVector);
    }


    /****************************************************************************************************
     * Description:
     *      Hides or shows the cursor based on player actions
     * Syntax:
     *      CursorState();
     ****************************************************************************************************/
    private void CursorState()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
            Cursor.lockState = cursorLock = CursorLockMode.None;
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
            Cursor.lockState = cursorLock = CursorLockMode.Locked;

        Cursor.lockState = cursorLock;
        Cursor.visible = (CursorLockMode.Locked != cursorLock);
    }
}