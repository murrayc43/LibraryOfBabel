using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Library : MonoBehaviour
{
    public GameObject library;
    public Transform mainFloor;
    private GameObject invisibleFloor;
    private Transform player;
    private int heightDifference = 10;
    public int currentFloor = 0;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = -10; i <= 10; i++)
        {
            if (i != 0)
                GameObject.Instantiate(library, new Vector3(0, i * heightDifference, 0), Quaternion.identity).name = "Library " + i;
        }
        invisibleFloor = GameObject.Find("Library -1");
        invisibleFloor.SetActive(false);
    }

    public void Update()
    {
        //if ((player.position.y / heightDifference) >= 0.816f)
        if ((player.position.y / heightDifference) >= 0.9f)
        {
            player.position = new Vector3(player.position.x, player.position.y - heightDifference, player.position.z);
            mainFloor.position -= new Vector3(0, heightDifference, 0);

            if (currentFloor <= 8)
            {
                invisibleFloor.SetActive(true);
                invisibleFloor = GameObject.Find("Library " + (-2 - currentFloor));
                invisibleFloor.SetActive(false);
            }
            currentFloor++;
        }
        //else if ((player.position.y / heightDifference) <= -0.185f)
        else if ((player.position.y / heightDifference) <= -0.101f)
        {
            player.position = new Vector3(player.position.x, player.position.y + heightDifference, player.position.z);
            mainFloor.position += new Vector3(0, heightDifference, 0);

            if (currentFloor <= 10)
            {
                invisibleFloor.SetActive(true);
                invisibleFloor = GameObject.Find("Library " + (0 - currentFloor));
                invisibleFloor.SetActive(false);
            }

            currentFloor--;
        }
    }
}