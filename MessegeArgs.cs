using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_HomeWork_13._7
{
    /// <summary>
    /// Вспомогательный класс для передачи дополнительной информации о событии и несколькими перегрузками подходящими соответствующему типу события
    /// </summary>
    public class MessegeArgs : EventArgs
    {
        public Client Client { get; set; }
        public Employee Employee { get; set; }
        public Account CurrAccount { get; set; }
        public string Operation { get; set; }
        

        public MessegeArgs(Client client, Employee employee, Account account, string operation) 
        { 
            this.Client = client; 
            this.Employee = employee;
            this.CurrAccount = account;
            this.Operation = operation;
            
           
        }
        public MessegeArgs(Client client, Employee employee, string operation)
        {
            this.Client = client;
            this.Employee = employee;
            this.Operation = operation;
        }
        public MessegeArgs(Client client, string operation)
        {
            this.Client = client;
            this.Operation = operation; 
        }
    }
}
