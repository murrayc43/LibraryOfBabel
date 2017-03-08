using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class BookGeneration : MonoBehaviour
{
    public GameObject bookGameObject;

    private int amountOfBooksPerShelf = 100;
    private float distanceBetweenBooks = 0.0285f;
    private float increment = 0.0f;
    private bool generateBooks = false;

    public void Start()
    {
        //Generate the books if requested
        if (generateBooks)
        {
            Vector3 location = transform.position;

            for (int i = 0; i < amountOfBooksPerShelf; i++)
            {
                GameObject tempBook = Instantiate(bookGameObject, transform.position, Quaternion.identity, gameObject.transform);
                tempBook.transform.localPosition = tempBook.transform.right * increment;
                tempBook.transform.rotation = transform.rotation;
                increment -= distanceBetweenBooks;
                
                if (transform.parent.name == "Shelf 0")
                    tempBook.name = "Book " + (i);
                else if (transform.parent.name == "Shelf 1")
                    tempBook.name = "Book " + (i + 100);
                else if (transform.parent.name == "Shelf 2")
                    tempBook.name = "Book " + (i + 200);
                else if (transform.parent.name == "Shelf 3")
                    tempBook.name = "Book " + (i + 300);
            }
        }
    }
}