using System.Globalization;

namespace FinancialApplication.Commons;

public static class CommonHelpers
{
    // Date converter
    public static DateTime ConvertToDate(string date)
    {
        var splitedDate = date.Split('/');

        DateTime convertedDate = new(int.Parse(splitedDate[0]), int.Parse(splitedDate[0]), int.Parse(splitedDate[0]));
        return convertedDate;
    }

    public static int GenerateRandomNumbers(int length)
    {
        Random random = new();
        int minValue = (int)Math.Pow(10, length - 1);
        int maxValue = (int)Math.Pow(10, length) - 1;
        int randomNumber = random.Next(minValue, maxValue);
        return randomNumber;
    }
}
