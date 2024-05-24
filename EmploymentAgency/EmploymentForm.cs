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
    public partial class EmploymentForm : Form
    {
        public DateTime SelectedDate { get; set; }
        public string Name { get; set; }

        public EmploymentForm()
        {
            InitializeComponent();
        }
        private void datePicker_ValueChanged(object sender, EventArgs e)
        {
            SelectedDate = datePicker.Value.Date;
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            Name = nameTextBox.Text;
        }

        private void Closebutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Okbutton_Click(object sender, EventArgs e)
        {
            // Проверяем, были ли заполнены все поля
            if (string.IsNullOrEmpty(nameTextBox.Text) || datePicker.Value == DateTime.MinValue)
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Если все поля заполнены, закрываем форму и устанавливаем результат диалога
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}
