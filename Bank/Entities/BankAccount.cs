using System.Text;
using System.Transactions;

namespace Bank;

internal class BankAccount
{
    private static int s_accountNuberSeed = 1000000000;
    public string Number { get; }
    public string Owner { get; private set; }

    public decimal Balance 
    {
        get 
        {
            decimal balance = 0;
            foreach (var item in _allTransactions)
            {
                balance += item.Amount;
            }

            return balance;
        } 
    }
    

    private readonly List<Transaction> _allTransactions = new();

    public BankAccount(string name, decimal initialBalance)
    {
       
        Owner = name; // this.Owner = name
        MakeDeposit(initialBalance,DateTime.UtcNow, "Initial balance");
        Number = s_accountNuberSeed.ToString();
        s_accountNuberSeed++;
    }
    public void MakeDeposit(decimal amount, DateTime date, string note)
    {
        if (amount <= 0)
        { 
            throw new ArgumentOutOfRangeException(nameof(amount),"Amount of deposit must be positive");
        }

        var deposit = new Transaction(amount, date, note);
        _allTransactions.Add(deposit);
    }

    public void MakeWithdrawal(decimal amount, DateTime date, string note)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount of withdrawal must be positive");
        }

        if (Balance < amount)
        {
            throw new InvalidOperationException("Not sufficient rubls for this withdawal");
        }

        var withdrawal = new Transaction(-amount, date, note);
        _allTransactions.Add(withdrawal);
    }


}