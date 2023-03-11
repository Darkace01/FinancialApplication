using System.Globalization;

namespace FinancialApplication.Commons;

public static class CommonHelpers
{
    /// <summary>
    /// Convert string to date. Format: yyyy/MM/dd
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateTime ConvertToDate(string date)
    {
        var splitedDate = date.Split('/');

        DateTime convertedDate = new(int.Parse(splitedDate[2]), int.Parse(splitedDate[1]), int.Parse(splitedDate[0]));
        return convertedDate;
    }
    /// <summary>
    /// Generate random numbers
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static int GenerateRandomNumbers(int length)
    {
        Random random = new();
        int minValue = (int)Math.Pow(10, length - 1);
        int maxValue = (int)Math.Pow(10, length) - 1;
        int randomNumber = random.Next(minValue, maxValue);
        return randomNumber;
    }
}
