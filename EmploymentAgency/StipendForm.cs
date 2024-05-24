using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EmploymentAgency;

namespace EmploymentAgency
{
    public partial class StipendForm : Form
    {
        private int id;
        private DatabaseAccess db;
        private Programm programmForm;

        public StipendForm(DatabaseAccess db, Programm programmForm, int id)
        {
            InitializeComponent();
            this.db = db;
            this.programmForm = programmForm;
            this.id = id;
            stipendAmountTextBox.Minimum = 0;
            stipendAmountTextBox.Maximum = 1000000;
            stipendDatePicker.Minimum = 0;
            stipendDatePicker.Maximum = 31;
            

        }

        private async void okButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(stipendAmountTextBox.Text) || string.IsNullOrWhiteSpace(stipendDatePicker.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля");
                return;
            }
            decimal stipendAmount = decimal.Parse(stipendAmountTextBox.Text);
            int stipendDate = int.Parse(stipendDatePicker.Text);

            db.AssignStipend(id, stipendAmount, stipendDate);

            // Задерживаем выполнение RefreshvacList() до тех пор, пока AddVacancy() не завершится
            await Task.Delay(TimeSpan.FromSeconds(1)); 

            programmForm.RefreshListView();

            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
