using UnityEngine;
using BigIntegerType;
using UnityEngine.UI;

public class Library : MonoBehaviour
{
    private GameObject invisibleFloor;
    private Transform player;
    private int heightDifference = 10;
    private int amountOfFloorsToShow = 50;

    public GameObject libraryBasic;
    public GameObject libraryLowPoly;
    public Transform mainFloor;
    public BigInteger currentFloor = 0;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = -amountOfFloorsToShow; i <= amountOfFloorsToShow; i++)
        {
            if (i != 0)
                GameObject.Instantiate((i >= -3 && i <= 3 ? libraryBasic : libraryLowPoly), new Vector3(0, i * heightDifference, 0), Quaternion.identity).name = "Library " + i;
        }
    }

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

    public void Teleport()
    {
        Vector3 mainFloorOriginalPosition = new Vector3(0, 0.1f, 0);
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
    }
}