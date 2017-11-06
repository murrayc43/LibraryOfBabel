using UnityEngine;
using BigIntegerType;
using UnityEngine.UI;

public class Library : MonoBehaviour
{
	#region Variables
    private GameObject invisibleFloor;
    private Transform player;
    private Algorithm algorithmScript;
    private Text lookupInput;
    private Text lookupLocation;
    private int heightDifference = 10;
    private int amountOfFloorsToShow = 25;

    public GameObject libraryLimited;
    public GameObject libraryStairsOnly;
    public Transform mainFloor;
    public BigInteger currentFloor = 0;
	#endregion

	#region Start
    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        algorithmScript = GameObject.FindGameObjectWithTag("Algorithm").GetComponent<Algorithm>();
        lookupInput = GameObject.FindGameObjectWithTag("LookupInput").GetComponent<Text>();
        lookupInput.transform.parent.gameObject.SetActive(false);
        lookupLocation = GameObject.FindGameObjectWithTag("LookupLocation").GetComponent<Text>();

        for (int i = -amountOfFloorsToShow; i <= amountOfFloorsToShow; i++)
            if (i != 0)
                GameObject.Instantiate((i > -3 && i < 3 ? libraryLimited : libraryStairsOnly), new Vector3(0, i * heightDifference, 0), Quaternion.identity).name = "Library " + i;
    }
	#endregion

	#region Update
    public void Update()
    {
        if ((player.position.y / heightDifference) >= 0.9f)
        {
            //Reset the player back to the actual first floor
            player.position = new Vector3(player.position.x, player.position.y - heightDifference, player.position.z);
            currentFloor++;

            //Hide the main floor if the player is too far away to see it, otherwise display it
            if (currentFloor > amountOfFloorsToShow)
                mainFloor.gameObject.SetActive(false);
            else
            {
                mainFloor.gameObject.SetActive(true);
                mainFloor.position -= new Vector3(0, heightDifference, 0);
            }
        }
        else if ((player.position.y / heightDifference) <= -0.101f)
        {
            //Reset the player back to the actual first floor
            player.position = new Vector3(player.position.x, player.position.y + heightDifference, player.position.z);
            currentFloor--;

            //Hide the main floor if the player is too far away to see it, otherwise display it
            if (currentFloor > amountOfFloorsToShow)
                mainFloor.gameObject.SetActive(false);
            else
            {
                mainFloor.gameObject.SetActive(true);
                mainFloor.position += new Vector3(0, heightDifference, 0);
            }
        }
    }
	#endregion

	#region Teleport
    public void Teleport()
    {
        Vector3 mainFloorOriginalPosition = new Vector3(102.5f, 0.1f, 0);
        GameObject teleportText = GameObject.FindGameObjectWithTag("TeleportInput");
        BigInteger teleportToFloor = new BigInteger(teleportText.GetComponent<Text>().text);

        if (teleportToFloor > amountOfFloorsToShow)
        {
            mainFloor.position = (new Vector3(0, -heightDifference, 0) * amountOfFloorsToShow) + mainFloorOriginalPosition;
            mainFloor.gameObject.SetActive(false);
        }
        else
        {
            mainFloor.position = (new Vector3(0, -heightDifference, 0) * int.Parse(teleportToFloor.ToString())) + mainFloorOriginalPosition;
            mainFloor.gameObject.SetActive(true);
        }
        
        currentFloor = teleportToFloor;
        teleportText.GetComponent<Text>().text = "";
        teleportText.transform.parent.gameObject.SetActive(false);
        player.GetComponent<Player>().mainCamera.cursorLock = CursorLockMode.Locked;
    }
	#endregion

	#region Lookup
    public void Lookup()
    {
        lookupLocation.text = algorithmScript.ContentToLocation(lookupInput.text);
        lookupInput.GetComponent<Text>().text = "";
        lookupInput.transform.parent.gameObject.SetActive(false);
        player.GetComponent<Player>().mainCamera.cursorLock = CursorLockMode.Locked;
    }
	#endregion
}