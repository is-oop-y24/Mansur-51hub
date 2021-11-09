using System.Collections.Generic;
using System.Linq;
using Banks.Tools;

namespace Banks.Services.Bank
{
    public class BankConditions
    {
        private readonly List<InterestDependingFromBalance> _interests;

        public BankConditions(List<InterestDependingFromBalance> interests, double fixedInterestOnTheBalance, double fixedCommission, double limitForSuspiciousAccounts)
        {
            _interests = interests;
            if (double.IsNegative(fixedInterestOnTheBalance))
            {
                throw new BanksException("Interest must be a non negative number");
            }

            if (double.IsNegative(fixedCommission))
            {
                throw new BanksException("Credit limit must be a non negative number");
            }

            FixedInterestOnTheBalance = fixedInterestOnTheBalance;
            FixedCommission = fixedCommission;
            LimitForSuspiciousAccounts = limitForSuspiciousAccounts;
        }

        public double FixedInterestOnTheBalance { get; private set; }
        public double FixedCommission { get; private set; }
        public double LimitForSuspiciousAccounts { get; }

        public double GetInterestForDepositAccount(double balance)
        {
            InterestDependingFromBalance interest = _interests.FirstOrDefault(p =>
                p.MinBalance.CompareTo(balance) < 0 && p.MaxBalance.CompareTo(balance) > 0);
            if (interest == null)
            {
                throw new BanksException($"Could not find a condition for a balance {balance} on deposit account");
            }

            return interest.Interest;
        }

        public void ChangeFixedInterestOnTheBalance(double interest)
        {
            if (double.IsNegative(interest))
            {
                throw new BanksException("Interest must be a non negative number");
            }

            FixedInterestOnTheBalance = interest;
        }

        public void ChangeFixedCommission(double fixedCommission)
        {
            if (double.IsNegative(fixedCommission))
            {
                throw new BanksException("Fixed commission must be a non negative number");
            }

            FixedCommission = fixedCommission;
        }
    }
}