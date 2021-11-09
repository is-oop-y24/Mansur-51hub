namespace Banks.Services.Accounts
{
    public class Payment
    {
        public double MonthlyPayment { get; private set; } = 0;

        public void CalculateDailyPayment(double balance, double fixedInterest)
        {
            const int numberOfDaysInYear = 365;
            double fixedDailyInterest = fixedInterest / numberOfDaysInYear;
            const int percentage = 100;
            MonthlyPayment += balance * fixedDailyInterest / percentage;
        }

        public double GetMonthlyPayment()
        {
            double value = MonthlyPayment;
            MonthlyPayment = 0;
            return value;
        }
    }
}