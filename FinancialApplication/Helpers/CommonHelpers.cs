using System.Globalization;

namespace FinancialApplication.Helpers
{
    public static class CommonHelpers
    {
        // Date converter
        public static DateTime ConvertToDate(string date)
        {
            DateTime convertedDate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return convertedDate;
        }
    }
}
