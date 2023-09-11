using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CS_HomeWork_13._7
{
    /// <summary>
    /// Вспомогательный класс обработки событий и соответствующего создания уведомлений и записей в журнале
    /// </summary>
    public class MessageAnnouncedService
    {
        public delegate void MessageAnnouncedEventHandler( object source, EventArgs e );
        public event MessageAnnouncedEventHandler MessageAnnounced;
        public ObservableCollection<LogConteiner> LogConteinerList;
        public Employee current_employee;
        public  void MassegeLogPublishing(ObservableCollection<LogConteiner> oc,Employee current_employee, Account current_count, Client current_client, string action)
        {
            LogConteiner temp_logconteiner;
            string time = DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss");
            if (current_count == null) { temp_logconteiner = new LogConteiner(current_employee.FI, current_client.FIO, action, "-", time); }
            else temp_logconteiner = new LogConteiner(current_employee.FI, current_client.FIO, action, current_count.Number, time);
            for (int i = 0; i < oc.Count; i++) { oc[i].LogConteinerPrint();}
            oc.Add(temp_logconteiner);
        }
        /// <summary>
        /// Универсальный метод обработки событий на которые есть подписка этим методом.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="arg"></param>
        public void OnOperationDone(object source, MessegeArgs arg)
        {
            string message = $"Клиент: {arg.Client.FIO}\nОперация: {arg.Operation}\nРаботник: {this.current_employee.FI}";
            this.MassegeLogPublishing(this.LogConteinerList,this.current_employee,arg.CurrAccount,arg.Client,arg.Operation);
            MessageBox.Show(message); 
        }
        //public void OnAccountTopUp(object source, MessegeArgs arg) 
        //{

        //}
    }
    /// <summary>
    /// Класс для удобного создания и содержания записей журнала событий
    /// </summary>
    public class LogConteiner
    {
        public string OperationType { get; set; }
        public string Client { get; set; }
        public string AccountNumber { get; set; }
        public string Employee { get; set; }
        public string Time {get; set;}
        public LogConteiner(string employee, string client, string ot, string count, string time)
        {
            this.Employee = employee;
            this.Client = client;
            this.OperationType = ot;
            this.AccountNumber = count;
            this.Time = time;
        }
        public string LogConteinerPrint()
        {
            string e = $"{this.OperationType} | {this.Client} | {this.AccountNumber} | {this.Employee} | {this.Time}\n";
            Debug.WriteLine(e);
            return e ;
        }
    }
}   

