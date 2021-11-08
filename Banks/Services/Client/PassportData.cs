using System;
using Banks.Tools;

namespace Banks.Services.Client
{
    public class PassportData
    {
        public PassportData(string passportSeries, string passportNumber)
        {
            const int seriesCorrectLength = 4;
            CheckPassportDataCorrection(passportSeries, seriesCorrectLength);
            const int numberCorrectLength = 6;
            CheckPassportDataCorrection(passportNumber, numberCorrectLength);
            PassportSeries = passportSeries;
            PassportNumber = passportNumber;
        }

        public string PassportSeries { get; }
        public string PassportNumber { get; }

        private static void CheckPassportDataCorrection(string data, int correctLength)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (!data.Length.Equals(correctLength))
            {
                throw new BanksException($"Incorrect data passport length. Correct length {correctLength}");
            }

            if (!int.TryParse(data, out _))
            {
                throw new BanksException($"Passport data must contain only digits");
            }
        }
    }
}