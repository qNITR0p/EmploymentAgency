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
    public partial class EditVacForm : Form
    {
        private int id;
        private DatabaseAccess db;
        private DataTable dt;
        private Programm programmForm;
        public EditVacForm(DatabaseAccess db, Programm programmForm, int id)
        {
            InitializeComponent();
            this.db = db;
            this.programmForm = programmForm;
            this.id = id;

            // Заполняем текстовые поля текущими данными о вакансии
            FillFields();
        }

        private void FillFields()
        {
            // Получаем данные о вакансии
            dt = db.GetVacanciesById(id);

            // Заполняем текстовые поля данными
            namevactxt.Text = dt.Rows[0]["Название_вакантной_должности"].ToString();
            typevactxt.Text = dt.Rows[0]["Тип_вакансии"].ToString();
            namejobgivetxt.Text = dt.Rows[0]["Название_работодателя"].ToString();
            adressjobgivetxt.Text = dt.Rows[0]["Адрес_работодателя"].ToString();
            phonejobgivetxt.Text = dt.Rows[0]["Телефон_работодателя"].ToString();
            moneytxt.Text = dt.Rows[0]["Примерный_размер_зарплаты"].ToString();
            specialstxt.Text = dt.Rows[0]["Особые_требования_к_работнику"].ToString();
        }

        private async void Okbutton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите внести изменения?", "Подтверждение", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                return;
            }
            // Получаем данные 
            string namevac = namevactxt.Text;
            string typevac = typevactxt.Text;
            string namejobgive = namejobgivetxt.Text;
            string adressjobgive = adressjobgivetxt.Text;
            string phonejobgive = phonejobgivetxt.Text;
            string money = moneytxt.Text;
            string specials = specialstxt.Text;

            // Обновляем существующую запись в базе данных
            await db.UpdateVacancy(id, namevac, typevac, namejobgive, adressjobgive, phonejobgive, money, specials);

            // Задерживаем выполнение RefreshvacList() до тех пор, пока AddVacancy() не завершится
            await Task.Delay(TimeSpan.FromSeconds(1));

            // Обновляем главную форму
            programmForm.RefreshvacList();
            programmForm.ClearFieldsvacList();

            // Закрываем форму
            this.Close();
        }

        private void Closebutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
