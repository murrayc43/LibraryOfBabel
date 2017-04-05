using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionTrigger : MonoBehaviour
{
    public GameObject section0;
    public GameObject section1;
    public GameObject section2;
    public GameObject section3;

    private void Start()
    {
        if (name == "NW Trigger")
            section0.SetActive(false);
        if (name == "NE Trigger")
            section1.SetActive(false);
        if (name == "SW Trigger")
            section2.SetActive(false);
        if (name == "SE Trigger")
            section3.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (name == "NW Trigger")
            section0.SetActive(true);
        if (name == "NE Trigger")
            section1.SetActive(true);
        if (name == "SW Trigger")
            section2.SetActive(true);
        if (name == "SE Trigger")
            section3.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (name == "NW Trigger")
            section0.SetActive(false);
        if (name == "NE Trigger")
            section1.SetActive(false);
        if (name == "SW Trigger")
            section2.SetActive(false);
        if (name == "SE Trigger")
            section3.SetActive(false);
    }
}