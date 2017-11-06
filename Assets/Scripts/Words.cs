using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Words : MonoBehaviour {
	#region Variables
	private string[] allWords = {
		"Fuck",
		"Dragon"
	};
	#endregion

	#region Get Words
	public string[] GetWords()
	{
		return allWords;
	}
	#endregion

	#region Find Word In Page
	/// <summary>
	/// Determines if a specified word is contained within the page being displayed.
	/// </summary>
	/// <param name="word">The specific word that you are looking for in the the page.</param>
	/// <param name="text">The text that you would like to search through.</param>
	/// <returns>True or false whether the specific word was found to be contained within the page being displayed.</returns>
	public bool FindWordInPage(string word, string text)
	{
		return (text.Contains(word)) ? true : false;
	}
	#endregion

	#region Find Words In Page
	/// <summary>
	/// Determines if any word in a list of words is contained within the page being displayed.
	/// </summary>
	/// <param name="words">The list of words that you are looking for in the the page. (Default = allWords)</param>
	/// <param name="text">The text that you would like to search through.</param>
	/// <returns>True or false whether any word was found to be contained within the page being displayed.</returns>
	public bool FindWordsInPage(string[] words, string text)
	{
		foreach (string word in words)
		{
			if (text.Contains(word))
				return true;
		}
		return false;
	}
	#endregion
}