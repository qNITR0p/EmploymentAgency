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
    public partial class AddVacForm : Form
    {
        private DatabaseAccess db;
        private Programm programmForm;
        public AddVacForm(DatabaseAccess db, Programm programmForm)
        {
            InitializeComponent();
            this.db = db;
            this.programmForm = programmForm;
        }

        private async void Okbutton_Click(object sender, EventArgs e)
        {
            // Получаем данные из текстовых полей
            string namevac = namevactxt.Text;
            string typevac = typevactxt.Text;
            string namejobgive = namejobgivetxt.Text;
            string adressjobgive = adressjobgivetxt.Text;
            string phonejobgive = phonejobgivetxt.Text;
            string money = moneytxt.Text;
            string specials = specialstxt.Text;

            // Проверяем, что все поля заполнены
            if (string.IsNullOrWhiteSpace(namevactxt.Text) || string.IsNullOrWhiteSpace(typevactxt.Text) || string.IsNullOrWhiteSpace(namejobgivetxt.Text)
                || string.IsNullOrWhiteSpace(adressjobgivetxt.Text) || string.IsNullOrWhiteSpace(phonejobgivetxt.Text) || string.IsNullOrWhiteSpace(moneytxt.Text)
                || string.IsNullOrWhiteSpace(specialstxt.Text))
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            // Проверяем, что поле moneytxt содержит только числовые значения
            decimal moneyDecimal;
            if (!decimal.TryParse(money, out moneyDecimal))
            {
                MessageBox.Show("Примерный размер зарплаты должен быть числом");
                return;
            }


            // Создаем новый экземпляр класса DatabaseAccess
            DatabaseAccess db = new DatabaseAccess();

            // Добавляем новую вакансию в базу данных асинхронно
            var addVacancyTask = db.AddVacancy(namevac, typevac, namejobgive, adressjobgive, phonejobgive, money, specials);

            // Задерживаем выполнение RefreshvacList() до тех пор, пока AddVacancy() не завершится
            await Task.Delay(TimeSpan.FromSeconds(1)); // замените 1 на количество секунд, которое вам нужно

            // Обновляем главную форму
            programmForm.RefreshvacList();
            programmForm.ClearFieldsvacList();

            // Закрываем форму
            this.Close();
        }

        private void Closebutton_Click(object sender, EventArgs e)
        {
            programmForm.RefreshvacList();
            this.Close();
        }
    }
}
