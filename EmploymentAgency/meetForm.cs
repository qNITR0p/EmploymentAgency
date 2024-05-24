using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmploymentAgency
{
    public partial class meetForm : System.Windows.Forms.Form
    {
        public static string userRole { get; set; }

        public meetForm()
        {
            InitializeComponent();
            comboBox.Items.Add("Специалист службы занятости");
            comboBox.Items.Add("Специалист по работе с работодателями");
            comboBox.Items.Add("Специалист по трудоустройству");
            comboBox.Items.Add("Администратор");
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            textBoxPassword.PasswordChar = '*';
        }

        Dictionary<string, string> rolesAndPasswords = new Dictionary<string, string>()
        {
            {"Специалист службы занятости", "1234"},
            {"Специалист по работе с работодателями", "2345"},
            {"Специалист по трудоустройству", "3456"},
            {"Администратор", "4567"}
        };

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (comboBox.SelectedItem == null || string.IsNullOrWhiteSpace(textBoxPassword.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedRole = comboBox.SelectedItem.ToString();
            string enteredPassword = textBoxPassword.Text;

            if (rolesAndPasswords.ContainsKey(selectedRole) && rolesAndPasswords[selectedRole] == enteredPassword)
            {
                // Пользователь успешно авторизован
                meetForm.userRole = selectedRole;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }

            else
            {
                MessageBox.Show("Введенный пароль не верен. Пожалуйста, попробуйте снова.", "Ошибка авторизации", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}