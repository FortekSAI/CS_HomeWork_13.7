using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CS_HomeWork_13._7
{
    /// <summary>
    /// Класс группировки элементов WPF для создания работников и клиентов
    /// </summary>
    public class CreationPanel
    {
        internal TextBox FirstName_Box { get; set; }
        internal TextBox LastName_Box { get; set; }
        internal TextBox Patronymic_Box { get; set; }
        internal Button CreateButton { get; set; }
        internal ComboBox Employee_type_ComboBox { get; set; }
        public CreationPanel(TextBox firstName, TextBox lastName, Button creationbutton)
        {
            this.FirstName_Box = firstName;
            this.LastName_Box = lastName;
            this.CreateButton = creationbutton;
        }
        public CreationPanel(TextBox firstName, TextBox lastName, TextBox patronymic, Button creationbutton) : this( firstName, lastName,creationbutton)
        {
            this.Patronymic_Box = patronymic;
        }
        public CreationPanel(TextBox firstName, TextBox lastName, ComboBox employee_choose_type , Button creationbutton) : this(firstName, lastName, creationbutton)
        {
            this.Employee_type_ComboBox = employee_choose_type;
        }
        internal void Check_Employee()
        {
            if (this.FirstName_Box.Text != "" && this.LastName_Box.Text != "" && this.Employee_type_ComboBox.SelectedItem != null) {this.CreateButton.IsEnabled = true;}
            else this.CreateButton.IsEnabled = false;
        }
        internal void Check_Client() 
        { 
            if(this.FirstName_Box.Text != "" && this.LastName_Box.Text != "" && this.Patronymic_Box.Text != "") { this.CreateButton.IsEnabled=true;}
            else this.CreateButton.IsEnabled = false;
        }
    }
}
