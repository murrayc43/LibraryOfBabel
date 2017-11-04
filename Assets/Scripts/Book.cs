using UnityEngine;

public class Book : MonoBehaviour
{
    public void Start()
    {
        GetComponent<BoxCollider>().enabled = true;
        tag = "Book";
    }
}