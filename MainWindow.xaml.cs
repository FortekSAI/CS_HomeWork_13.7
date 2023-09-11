using Newtonsoft.Json;
//using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
//using System.Diagnostics.Metrics;
//using System.ComponentModel;
using System.IO;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
//using System.Windows.Media.Animation;

namespace CS_HomeWork_13._7
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        delegate TOut SaveColl<TOut,TIn1,TIn2>(TIn1 Arg1, TIn2 Arg2);// просто проверка работы делегата
        Action<ObservableCollection<Client>, string> save_c = Save_Collection_JsonFile;
        Action<ObservableCollection<LogConteiner>, string> save_o = Save_Collection_JsonFile;
        ObservableCollection<Client> clients_list;
        string clients_list_json = "Clients_list";
        string operations_log_json = "Operations_log";
        MessageAnnouncedService message_event = new MessageAnnouncedService();
        bool account_type_choose;
        bool enter_exit_button_bool;
        int selected_index;
        Employee? current_employee;
        Client? selected_client;
        CreationPanel clnt_crt_pnl;
        CreationPanel empl_crt_pnl;
        public MainWindow()
        {
            clients_list = Check_Collection_JsonFile(clients_list, clients_list_json);
            message_event.LogConteinerList = Check_Collection_JsonFile(message_event.LogConteinerList, operations_log_json);
            //operations_log = Check_Collection_JsonFile(operations_log, operations_log_json);
            InitializeComponent();
            Exchange_ComboBox_client_to.ItemsSource = Exchange_ComboBox_client_from.ItemsSource = Clients_list_DataGrid.ItemsSource = clients_list;
            Operations_Log_DataGrid.ItemsSource = message_event.LogConteinerList;
            Exchange_text_account_to.Visibility = Exchange_ComboBox_account_to.Visibility = Exchange_text_account_from.Visibility = 
            Exchange_ComboBox_account_from.Visibility = Main_Grid.Visibility = Visibility.Collapsed;
            Employee_Enter_Button.IsEnabled = enter_exit_button_bool = Exchange_accept_button.IsEnabled = Exchange_summ_textbox.IsEnabled =
            Top_Up_Summ_TextBox.IsEnabled = Top_Up_Button.IsEnabled =  false;
            Clients_list_DataGrid.IsReadOnly = true;
            clnt_crt_pnl = new CreationPanel(FirstName_Box, LastName_Box, Patronimic_Box, Add_client_button);
            empl_crt_pnl = new CreationPanel(Employee_FirstName_TextBox, Employee_SecondName_TextBox, Employee_Type_Choose_ComboBox, Employee_Enter_Button);
            string temp = "Выберите клиента";
            Current_client_textblock.Text = temp;
            if (selected_client != null)
            {
                Accounts_list.ItemsSource = selected_client.Accounts;
            }
        }

        #region Методы MainWindow
        /// <summary>
        /// Метод проверки наличия файла с определенным названием в формате json нужной коллекции.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="file_Name"></param>
        private static ObservableCollection<T> Check_Collection_JsonFile<T>(ObservableCollection<T> obj, string file_Name)
        {
            if (File.Exists($"{file_Name}.json") && File.ReadAllText($"{file_Name}.json") != "")
            {
                string temp_path = File.ReadAllText($"{file_Name}.json");
                obj = JsonConvert.DeserializeObject<ObservableCollection<T>>(temp_path);
                Debug.WriteLine($"Файл десериализован\n{temp_path}");
            }
            else
            {
                obj = new ObservableCollection<T>();
                Debug.WriteLine("Создана новая коллекция");
            }
            return obj;
        }
        /// <summary>
        /// Метод сохранения определенной коллекции в формата json с указанием названия файла.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="file_Name"></param>
        private static void Save_Collection_JsonFile<T>(ObservableCollection<T> obj, string file_Name)
        {
            string temp_json = JsonConvert.SerializeObject(obj);
            File.WriteAllText($"{file_Name}.json", temp_json);
        }
        /// <summary>
        /// Метод рандомайзер для быстрого создания клиента любым работником
        /// </summary>
        /// <returns></returns>
        static Client Create_rnd_client()
        {

            List<string> FN = new List<string>();
            List<string> LN = new List<string>();
            List<string> PatN = new List<string>();
            // Лист листов для заполнения через цикл
            List<List<string>> Clients_inf = new List<List<string>>() { FN, LN, PatN };
            // В каждый лист через цыкл наполняем набором текстовых значений из соответствующих тесктовых файлов txt
            for (int i = 0; i < Clients_inf.Count; i++)
            {
                string[] txt_file_names = { "FN", "LN", "PatN" };
                StreamReader stream = new StreamReader($"{txt_file_names[i]}.txt");
                string? line;
                while ((line = stream.ReadLine()) != null)
                {
                    Clients_inf[i].Add(line);
                }
                stream.Close();
            }
            Random rand = new Random();
            //Создаем нового клиента через рандомный выбор значений полей из листов созданных ранее
            Client rndm_clietn = new Client(FN[rand.Next(0, Clients_inf[0].Count)],
                                       LN[rand.Next(0, Clients_inf[1].Count)],
                                       PatN[rand.Next(0, Clients_inf[2].Count)]);

            Debug.WriteLine("Рандомно создан: ");
            rndm_clietn.Print_Client();
            
            return rndm_clietn;
        }
        
        /// <summary>
        /// Метод проверки коректности комбобоксов с выбором клиентов и их счетов при переводе средств между счетами
        /// </summary>
        public void Elements_wpf_status_checker()
        {
            if (Exchange_ComboBox_client_to.SelectedIndex > -1)
            {
                Exchange_text_account_to.Visibility = Exchange_ComboBox_account_to.Visibility = Visibility.Visible;
            }
            else Exchange_text_account_to.Visibility = Exchange_ComboBox_account_to.Visibility = Visibility.Collapsed;
            if (Exchange_ComboBox_client_from.SelectedIndex > -1)
            {
                Exchange_text_account_from.Visibility = Exchange_ComboBox_account_from.Visibility = Visibility.Visible;
            }
            else Exchange_text_account_from.Visibility = Exchange_ComboBox_account_from.Visibility = Visibility.Collapsed;
            if (Exchange_ComboBox_account_from.SelectedIndex > -1 && Exchange_ComboBox_account_to.SelectedIndex > -1 && Exchange_ComboBox_client_from.SelectedIndex > -1 && Exchange_ComboBox_client_to.SelectedIndex > -1)
            {
                Exchange_summ_textbox.IsEnabled = true;
            }
            else Exchange_summ_textbox.IsEnabled = false;

        }
        #endregion
        #region События элементов WPF 
        private void Create_new_rnd_client_Click(object sender, RoutedEventArgs e)
        {
            current_employee?.Create_New_Random_Client(Create_rnd_client(), clients_list);
        }

        private void Choose_account_type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Choose_account_type.SelectedItem == Deposit)
            {
                account_type_choose = true;
            }
            if (Choose_account_type.SelectedItem == Nondeposit)
            {
                account_type_choose = false;
            }
            Account_type_choose_button.IsEnabled = true;
        }

        private void Clients_list_DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selected_client_index = Clients_list_DataGrid.SelectedIndex;
            Debug.WriteLine("___________________VVV__________________");
            Debug.WriteLine($"Выбран элемент с индексом: {selected_client_index.ToString()}");
            if (clients_list[selected_client_index].Accounts != null)
            {
                Debug.WriteLine($"Количество счетов: {clients_list?[selected_client_index].Accounts?.Count}");
                foreach (Account count in clients_list[selected_client_index].Accounts)
                {
                    Debug.WriteLine($"{count.Info}\n");
                }
                Debug.WriteLine("___________________OOO__________________");
            }
            selected_index = Clients_list_DataGrid.SelectedIndex;
            selected_client = clients_list[selected_index];
            selected_client.ClientChanged += message_event.OnOperationDone;
            Current_client_textblock.Text =Top_Up_Client_Text.Text = selected_client.FIO;
            Account_close_button.IsEnabled = false;
            Accounts_list.ItemsSource = Top_Up_Clients_Counts_ConboBox.ItemsSource = clients_list[selected_client_index].Accounts;
        }

        private void Accounts_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selected_client?.Accounts?.Count > 0)
            {
                selected_index = Clients_list_DataGrid.SelectedIndex;
                selected_client = clients_list[selected_index];
                int selected_account_index = Accounts_list.SelectedIndex;
                if (selected_account_index > -1)
                {
                    Selected_account_textblock.Text = selected_client?.Accounts?[selected_account_index].Info;
                    Account_close_button.IsEnabled = true;
                }
            }
        }
        private void Account_close_button_Click(object sender, RoutedEventArgs e)
        {
            current_employee?.Account_Close(selected_client, Accounts_list.SelectedIndex);
            Debug.WriteLine("Счёт закрыт");
            Account_close_button.IsEnabled = false;
        }
        private void Accounts_list_LostFocus(object sender, RoutedEventArgs e)
        {
            Selected_account_textblock.Text = "";
        }
        private void Exchange_summ_textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string temp = Exchange_summ_textbox.Text;
            bool parse_result = double.TryParse(temp, out double dsumm);
            if (parse_result) { Exchange_accept_button.IsEnabled = true; }
            else Exchange_accept_button.IsEnabled = false;
        }
        private void Exchange_accept_button_Click(object sender, RoutedEventArgs e)
        {
            
            double temp_summ = Convert.ToDouble(Exchange_summ_textbox.Text);
            if (temp_summ > clients_list?[Exchange_ComboBox_client_from.SelectedIndex].Accounts?[Exchange_ComboBox_account_from.SelectedIndex].Amount)
            {
                Exchange_warning_textblock.Text = "Указанная сумма больше суммы на счете отправителя";
            }
            else
            {
                current_employee?.Account_Exchange(temp_summ, clients_list[Exchange_ComboBox_client_from.SelectedIndex].Accounts[Exchange_ComboBox_account_from.SelectedIndex], 
                    clients_list?[Exchange_ComboBox_client_to.SelectedIndex].Accounts?[Exchange_ComboBox_account_to.SelectedIndex], 
                    clients_list?[Exchange_ComboBox_client_to.SelectedIndex],current_employee);
                Elements_wpf_status_checker();
                Exchange_warning_textblock.Text = "";
                Debug.WriteLine("Перевод произведён");
            }
        }

        private void Exchange_ComboBox_client_to_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Exchange_ComboBox_account_to.Visibility = Exchange_text_account_to.Visibility = Visibility.Visible;
            Exchange_ComboBox_account_to.ItemsSource = clients_list[Exchange_ComboBox_client_to.SelectedIndex].Accounts;
            Elements_wpf_status_checker();
        }

        private void Exchange_ComboBox_client_from_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Exchange_ComboBox_account_from.Visibility = Exchange_text_account_from.Visibility = Visibility.Visible;
            Exchange_ComboBox_account_from.ItemsSource = clients_list[Exchange_ComboBox_client_from.SelectedIndex].Accounts;
            Elements_wpf_status_checker();
        }

        private void Exchange_ComboBox_account_to_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Elements_wpf_status_checker();
        }

        private void Exchange_ComboBox_account_from_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Elements_wpf_status_checker();
        }



        
        private void Window_Closed_1(object sender, EventArgs e)
        {
            save_c(clients_list, clients_list_json);
            save_o(message_event.LogConteinerList, operations_log_json);
            //Save_Collection_JsonFile(clients_list, clients_list_json);
            //Save_Collection_JsonFile(operations_log, operations_log_json);
        }

        private void Account_type_choose_button_Click(object sender, RoutedEventArgs e)
        {
            AccountType accountType;
            if (account_type_choose)
            {
                accountType = AccountType.Deposite;
                current_employee.Account_Open(selected_client, accountType);
            }
            else
            {
                accountType = AccountType.NonDeposite;
                current_employee.Account_Open(selected_client, accountType);
            }

        }
        private void Employee_Enter_Button_Click(object sender, RoutedEventArgs e)
        {
            if (enter_exit_button_bool == false)
            {
                if (Employee_Type_Choose_ComboBox.SelectedItem == Manager)
                {
                    current_employee = new Manager(Employee_FirstName_TextBox.Text, Employee_SecondName_TextBox.Text);
                    NewClient_Creation_Tab.Visibility = Account_close_button.Visibility = Visibility.Visible;
                    Clients_list_DataGrid.IsReadOnly = false;
                }
                if (Employee_Type_Choose_ComboBox.SelectedItem == Consultant)
                {
                    current_employee = new Consultant(Employee_FirstName_TextBox.Text, Employee_SecondName_TextBox.Text);
                    NewClient_Creation_Tab.Visibility = Account_close_button.Visibility = Visibility.Collapsed;
                    Clients_list_DataGrid.IsReadOnly = true;
                }
                Main_Grid.Visibility = Visibility.Visible;
                Employee_Type_Choose_ComboBox.IsEnabled = Employee_FirstName_TextBox.IsEnabled = Employee_SecondName_TextBox.IsEnabled = false;
                Employee_FirstName_TextBox.Text = current_employee?.FirstName;
                Employee_SecondName_TextBox.Text = current_employee?.LastName;
                message_event.current_employee = current_employee;
                #region Подписки на события
                current_employee.NewClient += message_event.OnOperationDone;
                current_employee.ClientInfoChanged += message_event.OnOperationDone;
                current_employee.EmployeeEventAction += message_event.OnOperationDone;
                current_employee.AccountExchenge += message_event.OnOperationDone;
                current_employee.AccountOpened += message_event.OnOperationDone;
                current_employee.AccountClosed += message_event.OnOperationDone;
                #endregion
                Enter_Text.Text = "Вы вошли как:";
                Employee_Enter_Button.Content = "Выйти";
                
            }
            if (enter_exit_button_bool == true)
            {
                Main_Grid.Visibility = Visibility.Collapsed;
                Employee_Type_Choose_ComboBox.IsEnabled = Employee_FirstName_TextBox.IsEnabled = Employee_SecondName_TextBox.IsEnabled = true;
                Employee_FirstName_TextBox.Text = "";
                Employee_SecondName_TextBox.Text = "";
                Enter_Text.Text = "Войти как:";
                Employee_Enter_Button.Content = "Войти";
            }
            enter_exit_button_bool = !enter_exit_button_bool;

        }


        //private void Check_New_Client_Boxes()
        //{
        //    if (FirstName_Box.Text != "" && LastName_Box.Text != "" && Patronimic_Box.Text != "") { Add_client_button.IsEnabled = true; }
        //    else Add_client_button.IsEnabled = false;
        //}

        private void Employee_FirstName_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            empl_crt_pnl.Check_Employee();
        }

        private void Employee_SecondName_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            empl_crt_pnl.Check_Employee();
        }
        private void Employee_Type_Choose_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            empl_crt_pnl.Check_Employee();
        }

        private void Add_client_button_Click(object sender, RoutedEventArgs e)
        {
            current_employee?.Create_New_Client(clnt_crt_pnl, clients_list);
        }

        private void FirstName_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            clnt_crt_pnl.Check_Client();
        }

        private void LastName_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            clnt_crt_pnl.Check_Client();
        }

        private void Patronimic_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            clnt_crt_pnl.Check_Client();
        }

        private void Top_Up_Clients_Counts_ConboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Top_Up_Clients_Counts_ConboBox.SelectedIndex > -1) { Top_Up_Summ_TextBox.IsEnabled = true; }
            else Top_Up_Summ_TextBox.IsEnabled = false;
        }

        private void Top_Up_Summ_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string temp = Top_Up_Summ_TextBox.Text;
            bool parse_result = double.TryParse(temp, out double dsumm);
            if (parse_result) { Top_Up_Button.IsEnabled = true; }
            else Top_Up_Button.IsEnabled = false;
        }

        private void Top_Up_Button_Click(object sender, RoutedEventArgs e)
        {
            current_employee.Account_Top_Up(Convert.ToDouble(Top_Up_Summ_TextBox.Text),
                                            clients_list[Clients_list_DataGrid.SelectedIndex].Accounts?[Top_Up_Clients_Counts_ConboBox.SelectedIndex],
                                            clients_list[Clients_list_DataGrid.SelectedIndex], current_employee);
        }
        #endregion


    }
    //public enum ActionType
    //{
    //    AccountOpen,
    //    AccountClosed,
    //    AccountTopedUp,
    //    TransactionBeetwenAccounts,
    //    ClientsInfoChanged
    //}
}