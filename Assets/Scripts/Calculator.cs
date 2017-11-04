/****************************************************************************************************
 * Author: Curtis Murray
 * Notes: When converting from a string number to an integer array number, the numbers are stored
 *        backwards in the integer array, so the least significant digit is first
 *        Example: The number 401.36 --> int[6, 3, 1, 0, 4]
 *        
 *        sum, difference, product, quotient
 ****************************************************************************************************/

using UnityEngine;
using System.Collections;

public class Calculator : MonoBehaviour
{
    private string num1 = "";
    private string num2 = "";
    private int max = 0;
    private int decimalLocation = -1;

    /****************************************************************************************************
     * Description:
     *      Adds two string numbers together
     * Syntax:
     *      calculator.Add(string num1, string num2);
     * Parameters:
     *      num1 = The first number you want to add
     *      num2 = The second number you want to add
     * Returns:
     *      A string representing the two parameter values added together
     ****************************************************************************************************/
    public string Add(string numberOne, string numberTwo)
    {
        //Record the passed in parameters into local memory
        Record(numberOne, numberTwo);

        //Convert the numbers from string to integer arrays
        int[] n1 = ConvertStringToIntArray(num1);
        int[] n2 = ConvertStringToIntArray(num2);

        //Perform the addition to the two integer arrays
        int carry = 0;
        int[] sum = new int[max + 1];
        for (int i = 0; i < max; i++)
        {
            sum[i] = (n1[i] + n2[i] + carry) % 10;
            carry = ((n1[i] + n2[i] + carry) >= 10) ? 1 : 0;
        }
        sum[max] = carry;

        //Convert the sum back into a string
        string result = ConvertIntArrayToString(sum);

        //Make sure the resulting number isn't ending with a decimal point
        result = ValidateNumber(result);

        //Strip the resulting number of any redundant 0's before returning it
        return StripZeros(result);
    }


    /****************************************************************************************************
     * Description:
     *      Subtracts two string numbers from each other
     * Syntax:
     *      calculator.Subtract(string num1, string num2);
     * Parameters:
     *      num1 = The first number you want to subtract
     *      num2 = The second number you want to subtract
     * Returns:
     *      A string representing the two parameter values subtracted from each other
     ****************************************************************************************************/
    public string Subtract(string numberOne, string numberTwo)
    {
        //Record the passed in parameters into local memory
        Record(numberOne, numberTwo);

        //Convert the numbers from string to integer arrays
        int[] n1 = ConvertStringToIntArray(num1);
        int[] n2 = ConvertStringToIntArray(num2);

        //Perform the subtraction of the two integer arrays
        int[] difference = new int[max + 1];
        for (int i = 0; i < max; i++)
        {
            if(n1[i] - n2[i] < 0)
            {
                int j = i;
                bool exitLoop = false;
                do
                {
                    j++;
                    if (n1[j] == 0)
                        n1[j] = 9;
                    else
                    {
                        n1[j] -= 1;
                        exitLoop = true;
                    }
                } while (!exitLoop);
                n1[i] += 10;
            }
            difference[i] = n1[i] - n2[i];
        }

        //Convert the difference back into a string
        string result = ConvertIntArrayToString(difference);

        //Make sure the resulting number isn't ending with a decimal point
        result = ValidateNumber(result);

        //Strip the resulting number of any redundant 0's before returning it
        return StripZeros(result);
    }


    /*public string Multiply(string numberOne, string numberTwo)
    {
        //Record the passed in parameters into local memory
        Record(numberOne, numberTwo);

        //Convert the numbers from string to integer arrays
        int[] n1 = ConvertStringToIntArray(num1);
        int[] n2 = ConvertStringToIntArray(num2);

        //Perform the multiplication of the two integer arrays
        

        //Convert the sum back into a string
        string result = ConvertIntArrayToString(difference);

        //Make sure the resulting number isn't ending with a decimal point
        result = ValidateNumber(result);

        //Strip the resulting number of any redundant 0's before returning it
        return StripZeros(result);
    }*/


    /****************************************************************************************************
     * Description:
     *      Returns a subset of the passed in array
     * Syntax:
     *      int[] subset = SplitArray(int indexStart, int indexEnd, int[] array);
     * Parameters:
     *      indexStart = The starting index
     *      indexEnd = The ending index
     *      array = The array you want a subset of
     * Returns:
     *      The subset of the passed in array
     ****************************************************************************************************/
    private int[] SplitArray(int indexStart, int indexEnd, int[] array)
    {
        int[] newArray = new int[indexEnd - indexStart + 1];
        for (int i = 0; i + indexStart <= indexEnd; i++)
            newArray[i] = array[i + indexStart];
        return newArray;
    }


    /****************************************************************************************************
     * Description:
     *      Converts an integer array number into a string number
     * Syntax:
     *      string number = ConvertIntArrayToString(int[] num);
     * Parameters:
     *      num = The integer array number you want to convert
     * Returns:
     *      The string number passed in as an integer array
     ****************************************************************************************************/
    private string ConvertIntArrayToString(int[] num)
    {
        string result = "";
        for (int i = max; i >= 0; i--)
        {
            if (i == max - decimalLocation - 1)
                result += ".";
            result += num[i].ToString();
        }
        return result;
    }


    /****************************************************************************************************
     * Description:
     *      Converts a string number into an integer array number
     * Syntax:
     *      int[] number = ConvertStringToIntArray(string num);
     * Parameters:
     *      num = The string number you want to convert
     * Returns:
     *      The integer number passed in as an string
     ****************************************************************************************************/
    private int[] ConvertStringToIntArray(string num)
    {
        int[] number = new int[num.Length];
        for (int i = 0; i < num.Length; i++)
            number[i] = num[num.Length - 1 - i] - 48;
        return number;
    }


    /****************************************************************************************************
     * Description:
     *      Removes any irrelevant 0's from a number string
     * Syntax:
     *      string = Strip(string num);
     * Parameters:
     *      num = The number you want to strip of irrelevant 0's
     * Returns:
     *      The same number as num without irrelevant 0's
     ****************************************************************************************************/
    private string StripZeros(string num)
    {
        string stripped = "";
        int offsetleading = 0, offsetTrailing = 0;

        //Check the beginning of the number for leading 0's
        for (int i = 0; i < num.Length; i++)
        {
            if (num[i] == '0')
                offsetleading++;
            else
                break;
        }

        //Check the end of the number for trailing 0's
        for (int i = num.Length - 1; i >= 0; i--)
        {
            if (num[i] == '0')
                offsetTrailing++;
            else
                break;
        }

        for (int i = 0; i < num.Length - offsetleading - offsetTrailing; i++)
            stripped += num[i + offsetleading];

        stripped = ValidateNumber(stripped);
        return stripped;
    }


    /****************************************************************************************************
     * Description:
     *      Removes the decimal from a number
     * Syntax:
     *      string = StripDecimal(string num);
     * Parameters:
     *      num = The number you want to remove the decimal from
     * Returns:
     *      The new number without the decimal
     ****************************************************************************************************/
    private string StripDecimal(string num)
    {
        string newNum = "";
        for (int i = 0; i < num.Length; i++)
        {
            if (num[i] != '.')
                newNum += num[i];
        }
        return newNum;
    }


    /****************************************************************************************************
     * Description:
     *      Aligns the decimal points of two numbers
     * Syntax:
     *      string = Strip(string num);
     * Parameters:
     *      num = The number you want to strip of leading 0's
     * Returns:
     *      The same number as num without any leading 0's
     ****************************************************************************************************/
    private string[] AlignDecimals(string num1, string num2)
    {
        string newNum1 = "", newNum2 = "";
        int num1Pos = -1, num2Pos = -1;

        //Determine where the decimal points are for both numbers
        num1Pos = LocateDecimal(num1);
        num2Pos = LocateDecimal(num2);

        //Align the decimal points of both numbers by padding the front with 0's
        while (num1Pos != num2Pos)
        {
            if (num1Pos < num2Pos)
            {
                newNum1 += "0";
                num1Pos++;
            }
            else
            {
                newNum2 += "0";
                num2Pos++;
            }
        }
        newNum1 += num1;
        newNum2 += num2;
        
        //Make sure both numbers have decimal points
        bool hasDecimal = false;
        for (int i = 0; i < newNum1.Length; i++)
        {
            if (newNum1[i] == '.')
                hasDecimal = true; ;
        }
        if (!hasDecimal)
            newNum1 += ".0";

        hasDecimal = false;
        for (int i = 0; i < newNum2.Length; i++)
        {
            if (newNum2[i] == '.')
                hasDecimal = true; ;
        }
        if (!hasDecimal)
            newNum2 += ".0";

        //Make both numbers the same size by padding the end with 0's
        while (newNum1.Length != newNum2.Length)
        {
            if (newNum1.Length < newNum2.Length)
                newNum1 += "0";
            else
                newNum2 += "0";
        }

        string[] values = new string[2];
        values[0] = newNum1;
        values[1] = newNum2;
        return values;
    }


    /****************************************************************************************************
     * Description:
     *      Determines the location of the decimal point in a number
     * Syntax:
     *      int = LocateDecimal(string num);
     * Parameters:
     *      num = The number you want to find the decimal point in
     * Returns:
     *      The index of the decimal point in the string
     ****************************************************************************************************/
    private int LocateDecimal(string num)
    {
        for (int i = 0; i < num.Length; i++)
        {
            if (num[i] == '.')
                return i;
        }
        return num.Length;
    }


    /****************************************************************************************************
     * Description:
     *      Validates a number to make sure it isn't ending with a decimal
     * Syntax:
     *      string = ValidateNumber(string num);
     * Parameters:
     *      num = The number you want to vaplidate
     * Returns:
     *      The validated number
     ****************************************************************************************************/
    private string ValidateNumber(string num)
    {
        if (num[num.Length - 1] == '.')
            num += '0';
        return num;
    }


    /****************************************************************************************************
     * Description:
     *      Records the numbers passed in and calculates their stats into local memory
     * Syntax:
     *      Record(string numberOne, string numberTwo);
     * Parameters:
     *      numberOne = The first number to record
     *      numberTwo = The second number to record
     ****************************************************************************************************/
    private void Record(string numberOne, string numberTwo)
    {
        string[] numbers = AlignDecimals(numberOne, numberTwo);
        decimalLocation = LocateDecimal(numbers[0]);
        num1 = StripDecimal(numbers[0]);
        num2 = StripDecimal(numbers[1]);
        max = num1.Length;
    }
}