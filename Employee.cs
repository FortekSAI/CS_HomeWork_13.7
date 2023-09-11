using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using Microsoft.VisualBasic;
using System.Diagnostics;

namespace CS_HomeWork_13._7
{
    /// <summary>
    /// Абстрактный класс работника определяющий общий функционал для работников.
    /// По логике операции с информаций клинтов и счетами провидит работник, поэтому все необходимые события происходят в классе Employee ( см. ниже)
    /// </summary>
    public abstract class Employee : IAccountOperations<Account>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FI { get { return $"{this.FirstName} {this.LastName}"; } }
        public event EventHandler<MessegeArgs> ClientInfoChanged;
        public event EventHandler<MessegeArgs> EmployeeEventAction;
        public event EventHandler<MessegeArgs> AccountExchenge;
        public event EventHandler<MessegeArgs> AccountOpened;
        public event EventHandler<MessegeArgs> AccountClosed;
        public event EventHandler<MessegeArgs> NewClient;
        /// <summary>
        /// Добавление нового клиента.
        /// </summary>
        /// <param name="ed"></param>
        /// <param name="oc"></param>
        public abstract void Create_New_Client(CreationPanel ed, ObservableCollection<Client> oc);
        public abstract void Create_New_Random_Client(Client cl, ObservableCollection<Client> oc);
        protected virtual void OnNewClientCreated(Client client, Employee employee)
        {
            if (client != null && employee != null) { NewClient(this, new MessegeArgs(client, employee, "Клиент добавлен")); }
        }
        protected virtual void OnClientInfoChanged(Client client, Employee employee)
        {
            if (client != null && employee != null) { ClientInfoChanged(this, new MessegeArgs(client,employee,"Данные клиента изменены")); }
        }
        /// <summary>
        /// Перевод между счетами.
        /// </summary>
        /// <param name="summ"></param>
        /// <param name="from_account"></param>
        /// <param name="to_account"></param>
        /// <param name="client"></param>
        /// <param name="employee"></param>
        public void Account_Exchange(double summ, Account from_account, Account to_account, Client client, Employee employee)
        {
            to_account.Amount += summ;
            from_account.Amount -= summ;
            OnAccountExchange(summ, from_account, to_account, client, employee);
        }
        private protected void OnAccountExchange(double summ, Account from_account, Account to_account, Client client, Employee employee)
        {
            if (client != null && employee != null) { AccountExchenge(this, new MessegeArgs(client, employee,to_account, $"Перевод на сумму {summ} со счета {from_account.Number}")); }
        }
        /// <summary>
        /// Пополнение счета.
        /// </summary>
        /// <param name="summ"></param>
        /// <param name="account"></param>
        /// <param name="client"></param>
        /// <param name="employee"></param>
        public void Account_Top_Up(double summ, Account account, Client client, Employee employee)
        {
            account.Amount += summ;
            OnAccountTopUp(summ,account,client,employee);
        }
        private void OnAccountTopUp(double summ, Account account, Client client, Employee employee)
        {
            if (client != null && employee != null) { EmployeeEventAction(this, new MessegeArgs(client, employee,account, $"Счет пополнен на {summ}")); }
        }
        /// <summary>
        /// Закрытие счёта.
        /// </summary>
        /// <param name="oc_ac"></param>
        /// <param name="index"></param>
        /// <param name="client"></param>
        /// <param name="employee"></param>
        public void Account_Close(Client client, int index)
        {
            Account temp_account_for_inf = client.Accounts[index];
            client.Accounts?.RemoveAt(index);
            OnAccountClose(client,this, temp_account_for_inf);
        }
        protected virtual void OnAccountClose(Client client, Employee employee, Account account)
        {
            if (client != null && employee != null) { AccountClosed(this, new MessegeArgs(client,employee,account,"Закрыт счёт")); }
        }
        /// <summary>
        /// Открытие счета.
        /// </summary>
        /// <param name="oc_ac"></param>
        /// <param name="index"></param>
        /// <param name="client"></param>
        /// <param name="employee"></param>
        /// <param name="accountType"></param>
        public void Account_Open(Client client,AccountType accountType)
        {
            client.Account_Open(accountType);
            Account new_account = client.Accounts[client.Accounts.Count - 1];
            Debug.WriteLine($"Создан новый аккаунт с номером {new_account.Number}");
            OnAccountOpen(client, this, new_account);
        }
        private void OnAccountOpen(Client client, Employee employee, Account account)
        {
            if (client != null && employee != null)
            {
                AccountOpened(this, new MessegeArgs(client, employee, account, "Cчёт открыт"));
            }
        }
        public Employee (string firstname, string lastname)
        {
            FirstName = firstname;
            LastName = lastname;
        }
    }
    /// <summary>
    /// Класс Менеджера производный от класса Работник. Для него отрыта возможность добавлять новых клиентов и изменять данные клиента клиента
    /// </summary>
    public class Manager : Employee
    {
        public static string Type { get { return "Менеджер"; } }
        public Manager(string firstname, string lastname) : base(firstname, lastname) { }
        public override void Create_New_Client(CreationPanel ed, ObservableCollection<Client> oc)
        {
            Client client = new Client(ed.FirstName_Box.Text, ed.LastName_Box.Text, ed.Patronymic_Box.Text);
            oc.Add(client);
            ed.FirstName_Box.Text = ed.LastName_Box.Text = ed.Patronymic_Box.Text = null;
            OnNewClientCreated(client,this);
        }
        public override void Create_New_Random_Client( Client client, ObservableCollection<Client> oc)
        {
            oc.Add(client);
            OnNewClientCreated(client, this);
        }
    }
        /// <summary>
        /// Класс Консультанта производный от класса Работник. Для него закрыта возможность добавлять новых клиентов и изменять данные клиента клиента 
        /// </summary>
        public class Consultant : Employee
    {
        public static string Type { get { return "Консультант"; } }
        public Consultant(string firstname, string lastname) : base(firstname, lastname) { }
        public override void Create_New_Client(CreationPanel ed, ObservableCollection<Client> oc)
        {
            MessageBox.Show("Для создания нового клиента необходимо обладать правами менеджера");
        }

        public override void Create_New_Random_Client(Client cl, ObservableCollection<Client> oc)
        {
            MessageBox.Show("Для создания нового клиента необходимо обладать правами менеджера");
        }
    }
}
