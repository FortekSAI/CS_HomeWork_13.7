using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CS_HomeWork_13._7
{   
    /// <summary>
    /// Родительский класс для счетов клиентов реализующий интерфейс IExchange
    /// </summary>
    public class Account : INotifyPropertyChanged
    {
        
        public string Number { get; set; }
        private double Amount_A { get; set; }
        public string Type { get; set; }
        public double Amount
        {
            get
            {
                return Amount_A;
            }
            set
            {
                Amount_A = value;
                OnPropertyChanged("Amount");
                OnPropertyChanged("Info");
            }
        }
        public string Info { get { return $"{this.Type} {this.Number} Остаток: {this.Amount}"; } }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public Account(string number, double amount, string type)
        {
            Number = number;
            Amount = amount;
            Type = type;
        }
    }
    /// <summary>
    /// Класс наследник класса Count -  "Депозитный" 
    /// </summary>
    /// <typeparam name="N"></typeparam>
    /// <typeparam name="A"></typeparam>
    public class Deposit_C : Account//, IExchange<Count>, INotifyPropertyChanged
    {
        public Deposit_C(string number, double amount, string type = "Депозитный") : base(number, amount, type)
        {
            this.Type = type;
        }
    }
    /// <summary>
    /// Класс наследник класса Count -  "Недепозитный"
    /// </summary>
    /// <typeparam name="N"></typeparam>
    /// <typeparam name="A"></typeparam>
    public class Nondeposit_C : Account//, IExchange<Count>, INotifyPropertyChanged
    {
        public Nondeposit_C(string number, double amount, string type = "Недепозитный") : base(number, amount, type)
        {
            this.Type = type;
        }
    }
}
