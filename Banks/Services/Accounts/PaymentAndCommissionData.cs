using System;

namespace Banks.Services.Accounts
{
    public class PaymentAndCommissionData
    {
        private DateTime _time;

        public PaymentAndCommissionData()
        {
            _time = DateTime.Now;
        }

        public bool ShouldCalculateDailyPayment()
        {
            if (_time.Day.Equals(DateTime.Now.Day)) return false;
            _time = DateTime.Now;
            return true;
        }

        public bool ShouldDoPayment()
        {
            if (_time.Month.Equals(DateTime.Now.Month)) return false;
            _time = DateTime.Now;
            return true;
        }
    }
}