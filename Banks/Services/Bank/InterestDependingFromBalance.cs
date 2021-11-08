using Banks.Tools;

namespace Banks.Services.Bank
{
    public class InterestDependingFromBalance
    {
        public InterestDependingFromBalance(double minBalance, double maxBalance, double interest)
        {
            if (maxBalance.CompareTo(minBalance) < 0)
            {
                throw new BanksException("Maximal balance must be greater than Minimal");
            }

            MinBalance = minBalance;
            MaxBalance = maxBalance;
            Interest = interest;
        }

        public double MinBalance { get; }
        public double MaxBalance { get; }
        public double Interest { get; }
    }
}