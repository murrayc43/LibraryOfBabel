using UnityEngine;
using UnityEngine.UI;
using BigIntegerType;
using System.Text;

public class Algorithm : MonoBehaviour
{
    #region Variables
    private InputField bookText;
    private GameObject bookScreen, currentLocation;
    private Player player;
    private const string CHARACTERS = " 0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ`~!@#$%^&*()-_=+[{]}\\|;:'\",<.>/?"; //95 unique characters
    private const int PAGE_WIDTH = 137;
    private const int PAGE_HEIGHT = 28;
    private const int PAGE_LENGTH = PAGE_WIDTH * PAGE_HEIGHT;
    private const int AMOUNT_OF_PAGES = 1000;
    private const int BOOK_LENGTH = PAGE_LENGTH * AMOUNT_OF_PAGES;
	private const int BOOKS_PER_FLOOR = 70400;//76800; //70400
	private const int BOOKS_PER_SECTOR = 17600;//19200; //17600
    private const int BOOKS_PER_BOOKSHELF = 400;
    private const int MOD = 95;
    private int currentPage = 1;

    public BigInteger bookLocation;
    public string bookTitle;
    public BigInteger floor;
    public int sector, bookcase, shelf, book;
    #endregion

    #region Start
    /// <summary>
    /// This is run once at the beginning of the game.
    /// </summary>
    public void Start()
    {
		bookText = GameObject.FindGameObjectWithTag("BookText").GetComponent<InputField>();
        bookScreen = GameObject.FindGameObjectWithTag("BookScreen");
        bookScreen.SetActive(false);
        currentLocation = GameObject.FindGameObjectWithTag("CurrentLocation");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
            //GUIUtility.systemCopyBuffer = ttc;
        }
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

        str = PadRight(str);
        StringBuilder sb = new StringBuilder();
        
        //Do you want the entire book returned?
        if (entireBook)
        {
            for (int i = 0; i < BOOK_LENGTH; i++)
            {
                int charValue = CHARACTERS.IndexOf(str[i]);
                Random.InitState(i);
                int encryptedChar = (charValue + (int)(Random.value * MOD)) % MOD;
                sb.Append(CHARACTERS[encryptedChar]);
            }
        }
        //Do you want only the current page returned?
        else
        {
            int startIndex = (currentPage - 1) * PAGE_LENGTH;
            for (int i = startIndex; i < (startIndex + PAGE_LENGTH); i++)
            {
                int charValue = CHARACTERS.IndexOf(str[i]);
                Random.InitState(i);
                int encryptedChar = (charValue + (int)(Random.value * MOD)) % MOD;
                sb.Append(CHARACTERS[encryptedChar]);
            }
        }
        string encrypted = sb.ToString();
        if (!entireBook)
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

        str = (entireBook) ? PadRight(str) : PadDirect(str);
        StringBuilder sb = new StringBuilder();

        //Do you want the entire book returned?
        if (entireBook)
        {
            for (int i = 0; i < BOOK_LENGTH; i++)
            {
                int charValue = CHARACTERS.IndexOf(str[i]);
                Random.InitState(i);
                int decryptedChar = (int)Mod((charValue - (int)(Random.value * MOD)), MOD);
                sb.Append(CHARACTERS[decryptedChar]);
            }
        }
        //Do you only want the current page returned?
        else
        {
            int startIndex = (currentPage - 1) * PAGE_LENGTH;
            for (int i = startIndex; i < (startIndex + PAGE_LENGTH); i++)
            {
                int charValue = CHARACTERS.IndexOf(str[i]);
                Random.InitState(i);
                int decryptedChar = (int)Mod((charValue - (int)(Random.value * MOD)), MOD);
                sb.Append(CHARACTERS[decryptedChar]);
            }
        }
        string decrypted = sb.ToString();
        if (!entireBook)
            decrypted = FixStringForUI(decrypted);
        return decrypted;
    }
    #endregion

    #region Pad Left
    /// <summary>
    /// Pads a string with leading spaces.
    /// </summary>
    /// <param name="str">A string you would like to pad with extra spaces.</param>
    /// <returns>The string padded with leading spaces.</returns>
    public string PadLeft(string str)
    {
        return (new string(' ', BOOK_LENGTH - str.Length) + str);
    }
    #endregion

    #region Pad Right
    /// <summary>
    /// Pads a string with following spaces.
    /// </summary>
    /// <param name="str">A string you would like to pad with following spaces.</param>
    /// <returns>The string padded with following spaces.</returns>
    public string PadRight(string str)
    {
        return (str + new string(' ', BOOK_LENGTH - str.Length));
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

    #region Strip Left
    /// <summary>
    /// Strips a string of any leading spaces.
    /// </summary>
    /// <param name="str">The string you want to strip of spaces.</param>
    /// <returns>The string without leading spaces.</returns>
    public string StripLeft(string str)
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

    #region Strip Right
    public string StripRight(string str)
    {
        bool copy = false;
        for (int i = str.Length - 1; i < str.Length; i--)
        {
            if (!copy && str[i] != ' ' && str[i] != '\n')
                copy = true;
            if (copy)
                return (str.Substring(0, i + 1));
        }
        return "";
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

    #region Modulus
    /// <summary>
    /// A recreation of the true modulus operation. Use this function instead of '%' if you are trying to modulus negative numbers.
    /// </summary>
    /// <param name="a">The numerator of the modulus.</param>
    /// <param name="b">The denominator of the modulus.</param>
    /// <returns>The modulus of a % b.</returns>
    float Mod(float a, float b)
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
            value += CHARACTERS.IndexOf(number[i], 0) * (baseNum ^ j);
        return value;
    }
    #endregion

    #region Content To Location
    /// <summary>
    /// Finds the location of the book that has the first occurrance of the content passed in.
    /// </summary>
    /// <param name="content">The content of the book that you are looking for.</param>
    /// <returns>The location of the first occurrance book containing the content.</returns>
    public string ContentToLocation(string content)
    {
        //Find the title of the 0th index
        string zeroithTitle = Encrypt(" ", true);

        //Find a title with the content that we are looking for
        string textTitle = Decrypt(content, true);

        //Overlay the two titles, taking the left side from the zeroith and the right side from the text
        string completeTitle = "";
        completeTitle += textTitle.Substring(0, content.Length);
        completeTitle += zeroithTitle.Substring(content.Length);

        //Calculate the index of the first occurrence of the content
        string firstOccurrenceIndex = Decrypt(completeTitle, true);

        //Strip the index of any leading spaces for calculation
        firstOccurrenceIndex = StripRight(firstOccurrenceIndex);

        //Calculate where in the library the book resides
        BigInteger locationID = 0;
        for (int i = 0; i < firstOccurrenceIndex.Length; i++)
            locationID += BigInteger.Power(95, i) * CHARACTERS.IndexOf(firstOccurrenceIndex[i]);
        
        //Break down the locationID to find out exactly where the book is located
        BigInteger floor = locationID / BOOKS_PER_FLOOR;
        locationID -= floor * BOOKS_PER_FLOOR;
        BigInteger sector = locationID / BOOKS_PER_SECTOR;
        locationID -= sector * BOOKS_PER_SECTOR;
        BigInteger bookshelf = locationID / BOOKS_PER_BOOKSHELF;
        locationID -= bookshelf * BOOKS_PER_BOOKSHELF;
        BigInteger book = locationID;

		GUIUtility.systemCopyBuffer = floor.ToString();
        return ("Go to: Floor " + floor + ", Section " + sector + ", Bookcase " + bookshelf + ", Book " + book);
    }
    #endregion

    #region Location to Index
    /// <summary>
    /// Calculates the index of a specific location in the library.
    /// </summary>
    /// <param name="index">The book location as a number.</param>
    /// <returns>The index of the book number.</returns>
    public string LocationToIndex(BigInteger index)
    {
        //Find out the largest starting point to calculate down to string
        int startingPoint = 0;
        while (index > (BigInteger.Power(95, startingPoint) * 95))
            startingPoint++;

        //Convert the index to the title
        StringBuilder sb = new StringBuilder();
        while (startingPoint >= 0)
        {
            BigInteger largestValue = BigInteger.Power(95, startingPoint);
            BigInteger difference = index / largestValue;
            BigInteger waste = largestValue * difference;
            
            sb.Insert(0, CHARACTERS[int.Parse(difference.ToString())]);

            index -= waste;
            startingPoint--;
        }
        return PadRight(sb.ToString());
    }
    #endregion

	#region Find Word In Page
	/// <summary>
	/// Determines if a specified word is contained within the page being displayed.
	/// </summary>
	/// <param name="word">The specific word that you are looking for in the the page.</param>
	/// <returns>True or false whether the specific word was found to be contained within the page being displayed.</returns>
	public bool FindWordInPage(string word)
	{
		return (bookText.text.Contains(word)) ? true : false;
	}
	#endregion

    #region Generate Book
    /// <summary>
    /// Generate's the content of the book the player is currently looking at.
    /// </summary>
    public void GenerateBook()
    {
        bookLocation = (floor * BOOKS_PER_FLOOR) + (sector * BOOKS_PER_SECTOR) + (bookcase * BOOKS_PER_BOOKSHELF) + book;
        bookTitle = Encrypt(LocationToIndex(bookLocation), true);
        bookScreen.SetActive(true);
        currentLocation.SetActive(false);
        bookText.text = Encrypt(bookTitle, false);
		print(FindWordInPage ("fuck"));
		GUIUtility.systemCopyBuffer = bookText.text;
    }
    #endregion

    #region Next Page Button
    /// <summary>
    /// Controls the 'next page' button when viewing a book's content.
    /// </summary>
    public void nextPage()
    {
        currentPage++;
        bookText.text = Encrypt(bookTitle, false);
    }
    #endregion

    #region Previous Page Button
    /// <summary>
    /// Controls the 'previous page' button when viewing a book's content.
    /// </summary>
    public void previousPage()
    {
        currentPage--;
        bookText.text = Encrypt(bookTitle, false);
    }
    #endregion

    #region Exit Book Button
    /// <summary>
    /// Controls the 'exit book' button when viewing a book's content.
    /// </summary>
    public void exitBook()
    {
        currentPage = 1;
        bookText.text = "";
        bookScreen.SetActive(false);
        currentLocation.SetActive(true);
        player.mainCamera.cursorLock = CursorLockMode.Locked;
    }
    #endregion
}