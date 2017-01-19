using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using BigIntegerType;
using System.Text;

public class Algorithm : MonoBehaviour
{
    #region Variables
    public Text bookText;
    private string characters = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ`~!@#$%^&*()-_=+[{]}\\|;:'\",<.>/? "; //95 unique characters
    private const int PAGE_WIDTH = 137;
    private const int PAGE_HEIGHT = 28;
    private const int PAGE_LENGTH = PAGE_WIDTH * PAGE_HEIGHT;
    private const int AMOUNT_OF_PAGES = 10;
    private const int BOOK_LENGTH = PAGE_LENGTH * AMOUNT_OF_PAGES;
    private const int MOD = 95;
    private float currentTime, previousTime = 0f;
    private int currentPage = 0;
    #endregion

    #region Start
    /// <summary>
    /// This is run once at the beginning of the game.
    /// </summary>
    public void Start()
    {
        bookText = GameObject.FindGameObjectWithTag("BookText").GetComponent<Text>();
    }
    #endregion

    #region Update
    /// <summary>
    /// Runs once every frame.
    /// </summary>
    public void Update()
    {
        //TESTING PURPOSES ONLY.
        if (Input.GetKeyDown(KeyCode.C))
        {
            GUIUtility.systemCopyBuffer = bookText.text;
            Debug.Log("<color=green>Page text copied to clipboard!</color>");
        }
    }
    #endregion

    #region Pad
    /// <summary>
    /// Pads a string with leading spaces.
    /// </summary>
    /// <param name="str">A string you would like to pad with extra spaces.</param>
    /// <returns>The string padded with leading spaces.</returns>
    public string Pad(string str)
    {
        return (new string(' ', BOOK_LENGTH - str.Length) + str);
    }
    #endregion

    #region PadDirect
    /// <summary>
    /// Pads a substring both with leading and following spaces based on where the substring resides in the entire book.
    /// </summary>
    /// <param name="str">A substring you would like to pad with extra spaces both leading and following it.</param>
    /// <returns>The substring padded with leading and following spaces.</returns>
    public string PadDirect(string str)
    {
        string paddedStr = "";
        paddedStr += new string(' ', (currentPage - 1) * PAGE_LENGTH);
        paddedStr += str;
        paddedStr += new string(' ', BOOK_LENGTH - str.Length);
        return paddedStr;
    }
    #endregion

    #region Strip
    /// <summary>
    /// Strips a string of any leading spaces.
    /// </summary>
    /// <param name="str">The string you want to strip of spaces.</param>
    /// <returns>The string without leading spaces.</returns>
    public string Strip(string str)
    {
        string newStr = "";
        bool copy = false;
        for (int i = 0; i < str.Length; i++)
        {
            if (!copy && str[i] != ' ' && str[i] != '\n')
                copy = true;
            if (copy)
                newStr += str[i];
        }
        return newStr;
    }
    #endregion

    #region FixStringForUI
    /// <summary>
    /// Adds newline characters for every line so the Unity UI does not add its own new lines when a space character is encountered.
    /// </summary>
    /// <param name="str">The string you want to fix for Unity UI.</param>
    /// <returns>The fixed Unity UI string.</returns>
    public string FixStringForUI(string str)
    {
        for (int i = PAGE_WIDTH; i < PAGE_LENGTH; i += PAGE_WIDTH + 1)
            str = str.Insert(i, "\n");
        return str;
    }
    #endregion

    #region Encrypt
    /// <summary>
    /// Creates an encrypted string of a predefined length based on any string passed in.
    /// </summary>
    /// <param name="str">A string that is to be encrypted.</param>
    /// <param name="entireBook">Do you want to return the entire book instead of one page at a time?</param>
    /// <returns>The encrypted version of either the entire book encrypted or one page at a time.</returns>
    public string Encrypt(string str, bool entireBook)
    {
        if (currentPage > AMOUNT_OF_PAGES)
        {
            Debug.LogError("<color=red>Encrypting Issue:</color> Books only have " + AMOUNT_OF_PAGES + " pages. You are trying to encrypt a page greater than that.");
            return "";
        }
        else if (currentPage < 1)
        {
            Debug.LogError("<color=red>Encrypting Issue:</color> Books begin at page 1. You are trying to encrypt a page below page 1.");
            return "";
        }

        str = Pad(str);
        string encrypted = "";
        if (entireBook)
        {
            for (int i = 0; i < BOOK_LENGTH; i++)
            {
                int charValue = characters.IndexOf(str[i]);
                Random.InitState(i);
                int encryptedChar = (charValue + (int)(Random.value * MOD)) % MOD;
                encrypted += characters[encryptedChar];
            }
        }
        else
        {
            int startIndex = (currentPage - 1) * PAGE_LENGTH;
            for (int i = startIndex; i < (startIndex + PAGE_LENGTH); i++)
            {
                int charValue = characters.IndexOf(str[i]);
                Random.InitState(i);
                int encryptedChar = (charValue + (int)(Random.value * MOD)) % MOD;
                encrypted += characters[encryptedChar];
            }
        }
        encrypted = FixStringForUI(encrypted);
        return encrypted;
    }
    #endregion

    #region Decrypt
    /// <summary>
    /// Decrypts a string
    /// </summary>
    /// <param name="str">A string that you would like to decrypt.</param>
    /// <param name="entireBook">Are you wanting to decrypt the whole book or a substring in a specific spot of the book?</param>
    public string Decrypt(string str, bool entireBook)
    {
        if (currentPage > AMOUNT_OF_PAGES)
        {
            Debug.LogError("<color=red>Decrypting Issue:</color> Books only have " + AMOUNT_OF_PAGES + " pages. You are trying to decrypt a page greater than that.");
            return "";
        }
        else if (currentPage < 1)
        {
            Debug.LogError("<color=red>Decrypting Issue:</color> Books begin at page 1. You are trying to decrypt a page below page 1.");
            return "";
        }

        str = (entireBook) ? Pad(str) : PadDirect(str);
        string decrypted = "";
        int startIndex = (currentPage - 1) * PAGE_LENGTH;
        for (int i = startIndex; i < (startIndex + PAGE_LENGTH); i++)
        {
            int charValue = characters.IndexOf(str[i]);
            Random.InitState(i);
            int decryptedChar = (int)trueModulus((charValue - (int)(Random.value * MOD)), MOD);
            decrypted += characters[decryptedChar];
        }
        decrypted = FixStringForUI(decrypted);
        return decrypted;
    }
    #endregion

    #region String to Integer
    /// <summary>
    /// Converts a string to an integer.
    /// </summary>
    /// <param name="str">The string you want to convert to an integer.</param>
    /// <returns>An integer representation of the passed in string.</returns>
    private int StrToInt(string str)
    {
        int newInt = 0;
        for (int i = 0; i < str.Length; i++)
            newInt += int.Parse(str[i].ToString()) + (10 * i);
        return newInt;
    }
    #endregion

    #region True Modulus
    /// <summary>
    /// A recreation of the true modulus operation. Use this function instead of '%' if you are trying to modulus negative numbers.
    /// </summary>
    /// <param name="a">The numerator of the modulus.</param>
    /// <param name="b">The denominator of the modulus.</param>
    /// <returns>The modulus of a % b.</returns>
    float trueModulus(float a, float b)
    {
        return a - b * Mathf.Floor(a / b);
    }
    #endregion

    #region Convert Base 10 to Base 95
    /// <summary>
    /// Converts a digit in base 10 to a digit in base 95.
    /// </summary>
    /// <param name="num">A number in string format.</param>
    /// <returns>A string of the passed in number in Base 95.</returns>
    private string Base10To95(BigInteger num)
    {
        /*string number = "";
        int i = 0;
        while (num / (int)Mathf.Pow(95, i) > 94)
            i++;
        while (i >= 0)
        {
            if (characters[(num / (int)Mathf.Pow(95, i))] == '\\')
                number += '\\';
            else
                number += characters[(num / (int)Mathf.Pow(95, i))];
            num -= (num / (int)Mathf.Pow(95, i)) * (int)Mathf.Pow(95, i);
            i--;
        }
        return number;*/
        return num.ToString(95);
    }
    #endregion

    #region Convert Base 95 to Base 10
    /// <summary>
    /// Converts a digit in base 95 to a digit in base 10.
    /// </summary>
    /// <param name="number">A number in string format.</param>
    /// <returns>A BigInteger representing the number that was passed in as a string.</returns>
    private BigInteger Base95To10(string number)
    {
        BigInteger value = new BigInteger(0);
        BigInteger baseNum = new BigInteger(95);
        for (int i = 0, j = number.Length - 1; j >= 0; i++, j--)
            value += characters.IndexOf(number[i], 0) * (baseNum ^ j);
        return value;
    }
    #endregion

    #region FasterEncrypt
    /// <summary>
    /// Creates an encrypted string of a predefined length based on any string passed in.
    /// </summary>
    /// <param name="str">A string that is to be encrypted.</param>
    /// <param name="entireBook">Do you want to return the entire book instead of one page at a time?</param>
    /// <returns>The encrypted version of either the entire book encrypted or one page at a time.</returns>
    public string FasterEncrypt(string str, bool entireBook)
    {
        if (currentPage > AMOUNT_OF_PAGES)
        {
            Debug.LogError("<color=red>Encrypting Issue:</color> Books only have " + AMOUNT_OF_PAGES + " pages. You are trying to encrypt a page greater than that.");
            return "";
        }
        else if (currentPage < 1)
        {
            Debug.LogError("<color=red>Encrypting Issue:</color> Books begin at page 1. You are trying to encrypt a page below page 1.");
            return "";
        }

        str = Pad(str);
        StringBuilder sb = new StringBuilder();
        if (entireBook)
        {
            for (int i = 0; i < BOOK_LENGTH; i++)
            {
                int charValue = characters.IndexOf(str[i]);
                Random.InitState(i);
                int encryptedChar = (charValue + (int)(Random.value * MOD)) % MOD;
                sb.Append(characters[encryptedChar]);
            }
        }
        else
        {
            int startIndex = (currentPage - 1) * PAGE_LENGTH;
            for (int i = startIndex; i < (startIndex + PAGE_LENGTH); i++)
            {
                int charValue = characters.IndexOf(str[i]);
                Random.InitState(i);
                int encryptedChar = (charValue + (int)(Random.value * MOD)) % MOD;
                sb.Append(characters[encryptedChar]);
            }
        }
        string encrypted = sb.ToString();
        string decrypted = FasterDecrypt(encrypted, false);
        encrypted = FixStringForUI(encrypted);
        return decrypted;
    }
    #endregion

    #region FasterDecrypt
    /// <summary>
    /// Decrypts a string
    /// </summary>
    /// <param name="str">A string that you would like to decrypt.</param>
    /// <param name="entireBook">Are you wanting to decrypt the whole book or a substring in a specific spot of the book?</param>
    public string FasterDecrypt(string str, bool entireBook)
    {
        if (currentPage > AMOUNT_OF_PAGES)
        {
            Debug.LogError("<color=red>Decrypting Issue:</color> Books only have " + AMOUNT_OF_PAGES + " pages. You are trying to decrypt a page greater than that.");
            return "";
        }
        else if (currentPage < 1)
        {
            Debug.LogError("<color=red>Decrypting Issue:</color> Books begin at page 1. You are trying to decrypt a page below page 1.");
            return "";
        }

        str = (entireBook) ? Pad(str) : PadDirect(str);
        StringBuilder sb = new StringBuilder();
        int startIndex = (currentPage - 1) * PAGE_LENGTH;
        for (int i = startIndex; i < (startIndex + PAGE_LENGTH); i++)
        {
            int charValue = characters.IndexOf(str[i]);
            Random.InitState(i);
            int decryptedChar = (int)trueModulus((charValue - (int)(Random.value * MOD)), MOD);
            sb.Append(characters[decryptedChar]);
        }
        string decrypted = sb.ToString();
        decrypted = FixStringForUI(decrypted);
        return decrypted;
    }
    #endregion

    public void NextPage()
    {
        currentPage++;
        bookText.text = FasterEncrypt("Curtis Gregory Murray", false);
    }

    public void PreviousPage()

    {
        currentPage--;
        bookText.text = FasterEncrypt("Curtis Gregory Murray", false);
    }
}