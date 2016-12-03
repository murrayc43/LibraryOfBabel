using UnityEngine;
using System.Collections;

public class Book : MonoBehaviour
{
    public void OnWillRenderObject()
    {
        ///print("OnWillRenderObject()");
    }

    public void OnBecameInvisible()
    {
        ///print("OnBecameInvisible()");
    }
}