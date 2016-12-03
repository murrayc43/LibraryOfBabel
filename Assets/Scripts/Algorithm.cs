using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using BigIntegerType;

public class Algorithm : MonoBehaviour
{
    #region Variables
    public Text bookText;
    private string characters = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ`~!@#$%^&*()-_=+[{]}\\|;:'\",<.>/? "; //95 unique characters
    private const int PAGE_WIDTH = 102;
    private const int PAGE_HEIGHT = 25;
    private const int PAGE_LENGTH = 1960;//50;
    private const int BOOK_LENGTH = PAGE_LENGTH;// * 1000;
    private const int MOD = 95;
    private const int SEED = 42;
    private float currentTime, previousTime = 0f;
    #endregion

    #region Start
    /// <summary>
    /// This is run once at the beginning of the game.
    /// </summary>
    public void Start()
    {
        bookText = GameObject.FindGameObjectWithTag("BookText").GetComponent<Text>();
        Encrypt("I love you mom and dad!");
        Decrypt("17kzS|gU?D0O9}NBe. ;436BN@hG5D+U8XSjp ( } :]txF6n pTL'<G i{N;~e2<UV`JXR=8iS_p-g-F & -WCvmouBQ* rZ3I NR-!Nx/_549=tHV_g}ayl@o!");
    }
    #endregion

    #region Update
    /// <summary>
    /// Runs once every frame.
    /// </summary>
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            Encrypt(Random.Range(0, 1204).ToString());
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
        string newStr = "";
        for (int i = 0; i < BOOK_LENGTH - str.Length; i++)
            newStr += " ";
        return (newStr + str);
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
            if (!copy && str[i] != ' ')
                copy = true;
            if (copy)
                newStr += str[i];
        }
        return newStr;
    }
    #endregion

    #region PseudoRandom
    /// <summary>
    /// Creates a pseudorandom number based off of a passed in index of a character in a string and predefined seed.
    /// </summary>
    /// <param name="index">An integer representing the index of a character in a string.</param>
    /// <returns>A pseudorandom integer.</returns>
    public int PseudoRandom(int index)
    {
        return (index * (int)(haltonSequence(index, SEED) * 100));
    }
    #endregion

    #region Encrypt
    /// <summary>
    /// Creates an encrypted string of a predefined length based on any string passed in.
    /// </summary>
    /// <param name="str">A string that is to be encrypted.</param>
    /// <param name="starting_i">An integer representing where in the encryption you want to start at.</param>
    public void Encrypt(string str)
    {
        str = Pad(str);
        string encrypted = "";
        for (int i = 0; i < BOOK_LENGTH; i++)
        {
            int charValue = characters.IndexOf(str[i % str.Length]);
            int encryptedChar = (charValue + (PseudoRandom(i) % MOD)) % MOD;
            encrypted += characters[encryptedChar];
        }
        encrypted = Strip(encrypted);
        print(encrypted);

        GUIUtility.systemCopyBuffer = encrypted;
    }
    #endregion

    #region Decrypt
    /// <summary>
    /// Decrypts a string
    /// </summary>
    /// <param name="str">A string that you would like to decrypt.</param>
    public void Decrypt(string str)
    {
        str = Pad(str);
        string decrypted = "";
        for (int i = 0; i < BOOK_LENGTH; i++)
        {
            int charValue = characters.IndexOf(str[i % str.Length]);
            int decryptedChar = ((charValue - (PseudoRandom(i) % MOD)) + MOD) % MOD;
            decrypted += characters[decryptedChar];
        }
        decrypted = Strip(decrypted);
        print(decrypted);
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

    #region Halton Sequence
    /// <summary>
    /// Recreation of the Halton Sequence that generates a random number between 0 and 1.
    /// </summary>
    /// <param name="index">The prime number that is to be broken up.</param>
    /// <param name="radix">How many times you want to break up the prime number sequence.</param>
    /// <returns>A pseudo random number between 0 and 1 based off the passed in prime number and division count.</returns>
    private float haltonSequence(int index, int radix)
    {
        float result = 0;
        float f = 1;
        float i = index;
        while (i > 0)
        {
            f = f / radix;
            result = result + f * (i % radix);
            i = Mathf.Floor(i / radix);
        }
        return result;
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
}