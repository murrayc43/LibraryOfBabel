﻿using UnityEngine;

public class Indexer : MonoBehaviour
{
	#region Variables
    public int sector = 0;
    public int bookcase = 0;
	#endregion

	#region Start
    public void Start()
    {
        //Get bookcase
        string[] nameSplit = name.Split(' ');
        bookcase = int.Parse(nameSplit[1]);

        //Get sector
        nameSplit = transform.parent.name.Split(' ');
        sector = int.Parse(nameSplit[1]);
    }
	#endregion
}