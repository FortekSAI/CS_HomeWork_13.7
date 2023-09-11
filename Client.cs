
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CS_HomeWork_13._7
{
    public class ClientArgs : EventArgs
    {
        public Client Client { get; set; } 
    }
    /// <summary>
    /// Класс клиента имеющий поля ФИО и коллекцию счетов клиента параметризированную aбстрактным классом Count
    /// </summary>
    public class Client : INotifyPropertyChanged, IAccountExist
    {
        private bool alredy_created = false;
        private string first_name;
        private string last_name;
        private string patronymic;
        public string First_name { get { return first_name; } set { first_name = value; OnPropertyChanged("First_name"); OnPropertyChanged("FIO"); OnClientInfoChanged(); } }
        public string Last_name { get {return last_name;} set { last_name = value; OnPropertyChanged("Last_name"); OnPropertyChanged("FIO"); OnClientInfoChanged(); } }
        public string Patronymic { get { return patronymic; } set { patronymic = value; OnPropertyChanged("Patronymic"); OnPropertyChanged("FIO"); OnClientInfoChanged(); } }
        public string FIO { get { return $"{Last_name} {First_name} {Patronymic}"; } }
        public ObservableCollection<Account>? Accounts { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<MessegeArgs> ClientChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public void OnClientInfoChanged()
        {
            if (ClientChanged != null)
            {
                ClientChanged(this, new MessegeArgs(this, "Данные клиента изменены"));
            }
        }
        public string Print_Client()
        {
            string client_fio = $"{this.FIO}";
            Debug.WriteLine($"{client_fio} . ");
            return client_fio;
        }
        public Client(string first_name, string last_name, string patronymic)
        {
            this.First_name = first_name;
            this.Last_name = last_name;
            this.Patronymic = patronymic;
            this.Accounts = new ObservableCollection<Account>();
        }
        /// <summary>
        /// Метод создания номера счета на основе точного времени.
        /// </summary>
        /// <returns></returns>
        private string Account_number_creation()
        {
            Regex regex = new Regex(@"\D");
            string prenumbe_1 = regex.Replace(DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss:fff"), "");
            Debug.WriteLine($"Преномер счета {prenumbe_1}");
            return prenumbe_1;
        }

        public void Account_Close(int index)
        {
            this.Accounts?.RemoveAt(index);
        } 
        public void Account_Open(AccountType accountType)
        {
            if(accountType == AccountType.Deposite) 
            {
                this.Accounts?.Add(new Deposit_C(Account_number_creation(), 0.0));
                Debug.WriteLine($"Для клиента {this.FIO} cоздан депозитный счет");
            }
            else
            {
                this.Accounts?.Add(new Nondeposit_C(Account_number_creation(), 0.0));
                Debug.WriteLine($"Для клиента {this.FIO} cоздан недепозитный счет");
            }
        }
    }
}
