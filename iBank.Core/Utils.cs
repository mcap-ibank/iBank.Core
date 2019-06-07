using System;
using System.Globalization;

namespace iBank.Core
{
    public static class Utils
    {
        public static bool FilterText(char @char) => char.IsControl(@char) || char.IsLetter(@char) || @char == ' ' || @char == '\'' || @char == '.' || @char == '-';
        public static bool FilterDocumentSerialNumber(char @char) => char.IsControl(@char) || char.IsLetter(@char) || char.IsDigit(@char) || @char == ' ';
        public static bool FilterCardNumber(char @char) => char.IsControl(@char) || char.IsDigit(@char) || @char == ' ';

        public static DateTime GetDateTime(string value)
        {
            if (int.TryParse(value, out var oaDate))
                return DateTime.FromOADate(oaDate);
            if (DateTime.TryParseExact(value, "dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out var dateTimeExact))
                return dateTimeExact;
            if (DateTime.TryParse(value, out var dateTime))
                return dateTime;
            return DateTime.MinValue;
        }

        public static bool PassportIsValid(string documentSerialNumber, DateTime birthday, DateTime passport, TimeSpan? plusInterval = null)
        {
            if (documentSerialNumber.Length == 12 && documentSerialNumber[2] == ' ' && documentSerialNumber[5] == ' ')
            {
                if (Equals(birthday.Date, passport.Date))
                    return false;

                if (plusInterval == null)
                    plusInterval = TimeSpan.FromDays(7);
                return !(birthday.AddYears(20) < DateTime.Now.Add(plusInterval.Value) && birthday.AddYears(20) > passport);
            }
            else
                return true;
        }
    }
}