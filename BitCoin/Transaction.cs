using System;
using System.Collections.Generic;
using System.Text;

namespace BitCoin
{
    class Transaction
    {
        public Account From;
        public Account To;
        public int BitCoinAmount;

        public Transaction(Account from, Account to, int bitCoinAmount)
        {
            From = from;
            To = to;
            BitCoinAmount = bitCoinAmount;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            string str = From.GetName() + $" sends {BitCoinAmount} BitCoin to " + To.GetName();
            return str;
        }
    }
}
