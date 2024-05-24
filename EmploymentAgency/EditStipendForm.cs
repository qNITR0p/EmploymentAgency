using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EmploymentAgency;

namespace EmploymentAgency
{
    public partial class EditStipendForm : Form
    {
        private int id;
        private DatabaseAccess db;
        private Programm programmForm;

        public EditStipendForm(DatabaseAccess db, Programm programmForm, int id)
        {
            InitializeComponent();
            this.db = db;
            this.programmForm = programmForm;
            this.id = id;
            stipendAmountTextBox.Minimum = 0;
            stipendAmountTextBox.Maximum = 1000000;
            stipendDatePicker.Minimum = 0;
            stipendDatePicker.Maximum = 31;

            // Заполняем текстовые поля текущими данными о пособии
            FillFields();
        }

        private void FillFields()
        {
            // Получаем данные о пособии
            DataTable dt = db.GetStipendsByUnemployedId(id);

            // Заполняем текстовые поля данными
            stipendAmountTextBox.Text = dt.Rows[0]["Сумма_пособия"].ToString();
            stipendDatePicker.Text = (dt.Rows[0]["Дата_выплаты_пособия"]).ToString();
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

            await db.UpdateStipend(id, stipendAmount, stipendDate);

            // Задерживаем выполнение RefreshStipendList() до тех пор, пока UpdateStipend() не завершится
            await Task.Delay(TimeSpan.FromSeconds(1));

            // Обновляем главную форму
            programmForm.RefreshStipendList();
            programmForm.ClearFieldsStipend();


            this.Close();
        }


        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
