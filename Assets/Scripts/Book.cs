using BigIntegerType;
using UnityEngine;
using System.Collections;

public class Book : MonoBehaviour
{
    public void Start()
    {
        GetComponent<BoxCollider>().enabled = true;
        tag = "Book";
    }
}