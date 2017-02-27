using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Library : MonoBehaviour
{
    public GameObject library;
    public Transform mainFloor;
    private GameObject invisibleFloor;
    private Transform player;
    [SerializeField]
    private int currentFloor = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        for (int i = -10; i < 10; i++)
            GameObject.Instantiate(library, new Vector3(0, i * 5, 0), Quaternion.identity).name = "Library " + i;
        invisibleFloor = GameObject.Find("Library -1");
        invisibleFloor.SetActive(false);
    }

    void Update()
    {
        if ((player.position.y / 5) >= 0.816f)
        {
            player.position = new Vector3(player.position.x, player.position.y - 5, player.position.z);
            mainFloor.position -= new Vector3(0, 5, 0);

            if (currentFloor <= 8)
            {
                invisibleFloor.SetActive(true);
                invisibleFloor = GameObject.Find("Library " + (-2 - currentFloor));
                invisibleFloor.SetActive(false);
            }
            currentFloor++;
        }
        else if ((player.position.y / 5) <= -0.185f)
        {
            player.position = new Vector3(player.position.x, player.position.y + 5, player.position.z);
            mainFloor.position += new Vector3(0, 5, 0);

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