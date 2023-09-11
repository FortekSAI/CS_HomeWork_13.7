using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CS_HomeWork_13._7
{

    /// <summary>
    /// Контрвариантный интерфейс с методом обмена между счетами и пополнения счета
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAccountOperations<in T>
    {
        /// <summary>
        /// Метод обмена между счетами изначально принимающий класс Count в качестве аргумента, но благодаря контрвариативности интерфейса мы сможем использовать классы наследники
        /// </summary>
        /// <param name="summ"></param>
        /// <param name="from_account"></param>
        void Account_Exchange(double summ, T from_account, T to_account, Client client, Employee employee);
        void Account_Top_Up(double summ, T account, Client client, Employee employee);
        
        private static string Account_number_creation()
        {
            Regex regex = new Regex(@"\D");
            string prenumbe_1 = regex.Replace(DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss:fff"), "");
            Debug.WriteLine($"Преномер счета {prenumbe_1}");
            return prenumbe_1;
        }
    }
    public enum AccountType
    {
        Deposite,
        NonDeposite
    }
    /// <summary>
    /// Интерфейс функций определяющих существование счетов клиента
    /// </summary>
    public interface IAccountExist
    {
        
        void Account_Close(int index);
        void Account_Open(AccountType accountType);
    }
}
