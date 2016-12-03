using UnityEngine;
using System.Collections;

public class Bookcase : MonoBehaviour
{
    public GameObject bookOne, bookTwo, bookThree, bookFour;
    public int wallShelf;
    private Vector3 currentPosition;
    private bool northSide;

    /*void Start()
    {
        //North Side
        if (wallShelf == 0 || wallShelf == 1)
        {
            northSide = true;
            for (int j = 0; j < 5; j++)
            {
                currentPosition = new Vector3(-0.08f - (j * 0.01f), 0.965f, -0.256f + (j * 0.147f));
                for (int i = 0; i < 25; i++)
                {
                    GameObject book = RandomBook();
                    currentPosition -= new Vector3(0, 0.0185f, 0);
                }

                currentPosition = new Vector3(-0.08f - (j * 0.01f), 0.47f, -0.256f + (j * 0.147f));
                for (int i = 0; i < 25; i++)
                {
                    GameObject book = RandomBook();
                    currentPosition -= new Vector3(0, 0.0185f, 0);
                }

                currentPosition = new Vector3(-0.08f - (j * 0.01f), -0.025f, -0.256f + (j * 0.147f));
                for (int i = 0; i < 25; i++)
                {
                    GameObject book = RandomBook();
                    currentPosition -= new Vector3(0, 0.0185f, 0);
                }

                currentPosition = new Vector3(-0.08f - (j * 0.01f), -0.52f, -0.256f + (j * 0.147f));
                for (int i = 0; i < 25; i++)
                {
                    GameObject book = RandomBook();
                    currentPosition -= new Vector3(0, 0.0185f, 0);
                }
            }
        }

        //South Side
        if (wallShelf == -1 || wallShelf == 0)
        {
            northSide = false;
            for (int j = 0; j < 5; j++)
            {
                currentPosition = new Vector3(0.078f + (j * 0.01f), 0.965f, -0.256f + (j * 0.147f));
                for (int i = 0; i < 25; i++)
                {
                    GameObject book = RandomBook();
                    currentPosition -= new Vector3(0, 0.0185f, 0);
                }

                currentPosition = new Vector3(0.078f + (j * 0.01f), 0.47f, -0.256f + (j * 0.147f));
                for (int i = 0; i < 25; i++)
                {
                    GameObject book = RandomBook();
                    currentPosition -= new Vector3(0, 0.0185f, 0);
                }

                currentPosition = new Vector3(0.078f + (j * 0.01f), -0.025f, -0.256f + (j * 0.147f));
                for (int i = 0; i < 25; i++)
                {
                    GameObject book = RandomBook();
                    currentPosition -= new Vector3(0, 0.0185f, 0);
                }

                currentPosition = new Vector3(0.078f + (j * 0.01f), -0.52f, -0.256f + (j * 0.147f));
                for (int i = 0; i < 25; i++)
                {
                    GameObject book = RandomBook();
                    currentPosition -= new Vector3(0, 0.0185f, 0);
                }
            }
        }
    }*/


    private GameObject RandomBook()
    {
        int randomBook = Random.Range(1, 5);
        switch (randomBook)
        {
            case 1:
                return SpawnBookOne();
            case 2:
                return SpawnBookTwo();
            case 3:
                return SpawnBookThree();
            case 4:
                return SpawnBookFour();
            default:
                return SpawnBookOne();
        }
    }

    private GameObject SpawnBookOne()
    {
        GameObject book = (GameObject)GameObject.Instantiate(bookOne);
        book.transform.parent = transform;
        book.transform.localScale = new Vector3(1, 1, 1);
        BookSetup(book);
        return book;
    }


    private GameObject SpawnBookTwo()
    {
        GameObject book = (GameObject)GameObject.Instantiate(bookTwo);
        book.transform.parent = transform;
        book.transform.localScale = new Vector3(1, 1, 1);
        BookSetup(book);
        return book;
    }


    private GameObject SpawnBookThree()
    {
        GameObject book = (GameObject)GameObject.Instantiate(bookThree);
        book.transform.parent = transform;
        book.transform.localScale = new Vector3(0.7f, 1, 0.7f);
        BookSetup(book);
        return book;
    }


    private GameObject SpawnBookFour()
    {
        GameObject book = (GameObject)GameObject.Instantiate(bookFour);
        book.transform.parent = transform;
        book.transform.localScale = new Vector3(1, 1, 1);
        BookSetup(book);
        return book;
    }


    private void BookSetup(GameObject book)
    {
        book.transform.localPosition = currentPosition;
        if (northSide)
            book.transform.localEulerAngles = new Vector3(0, -90, 90);
        else
            book.transform.localEulerAngles = new Vector3(180, -90, 90);
    }
}