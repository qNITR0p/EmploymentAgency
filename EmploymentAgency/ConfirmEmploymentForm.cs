using Microsoft.Office.Interop.Access.Dao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace EmploymentAgency
{
    public partial class ConfirmEmploymentForm : Form
    {
        private DatabaseAccess db;
        private int unemp_id;
        private Programm programmForm;


        public ConfirmEmploymentForm(DatabaseAccess db, Programm programmForm, int id)
        {
            InitializeComponent();
            this.programmForm = programmForm;
            this.unemp_id = id;
            vacList.Scrollable = true;
            this.db = db;

            // Заполняем ListView
            FillFields();
        }

        private void FillFields()
        {
            // Получаем данные о вакансиях
            DataTable dt = db.GetVacancies();

            //Получаем данные о безработном
            DataTable unmp = db.GetUnemployedById(unemp_id);

            // Если столбцы уже существуют, очищаем ListView
            if (vacList.Columns.Count > 0)
            {
                vacList.Items.Clear();
            }
            else
            {
                // Добавляем столбцы в ListView
                vacList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                vacList.View = View.Details;
                vacList.FullRowSelect = true;
                vacList.Columns.Add("ID", 100, HorizontalAlignment.Left);
                vacList.Columns.Add("Название вакансии", 200, HorizontalAlignment.Left);
                vacList.Columns[0].Width = (int)(vacList.Width * 0.3);
                vacList.Columns[1].Width = (int)(vacList.Width * 0.65);
            }


            Nametxt.Text = unmp.Rows[0]["ФИО"].ToString();

            // Получаем путь к изображению
            string photoFileName = unmp.Rows[0]["Фотография"].ToString();

            // Создаем полный путь к изображению
            string imagesFolderPath = Path.Combine(Application.StartupPath, "Images");
            string photoPath = Path.Combine(imagesFolderPath, photoFileName);

            // Загружаем изображение из файла в pictureBoxMain
            if (File.Exists(photoPath))
            {
                pictureBoxEmp.Image = Image.FromFile(photoPath);
                pictureBoxEmp.SizeMode = PictureBoxSizeMode.StretchImage;
            }


            // Добавляем данные в ListView
            foreach (DataRow row in dt.Rows)
            {
                int id = Convert.ToInt32(row["ID"]);
                if (!db.HasEmploymentForVacancy(id))
                {
                    ListViewItem item = new ListViewItem(row["ID"].ToString());
                    item.SubItems.Add(row["Название_вакантной_должности"].ToString());
                    vacList.Items.Add(item);
                }
            }
        }

        //Запрет на изменение размера столбцов
        static bool busy = false;
        private void vacList_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (busy) return;
            busy = true;
            if (e.ColumnIndex == 0)
                vacList.Columns[e.ColumnIndex].Width = (int)(vacList.Width * 0.3);
            else if (e.ColumnIndex == 1)
                vacList.Columns[e.ColumnIndex].Width = (int)(vacList.Width * 0.65);
            busy = false;
        }

        //Выбор строки в vacList для отображения данных
        private void vacList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (vacList.SelectedItems.Count > 0)
            {
                // Получаем выбранный элемент
                ListViewItem item = vacList.SelectedItems[0];

                // Получаем ID вакансии
                int id = int.Parse(item.Text);

                // Создаем новый экземпляр класса DatabaseAccess
                DatabaseAccess db = new DatabaseAccess();

                // Получаем данные о вакансии
                DataTable dt = db.GetVacanciesById(id);

                // Заполняем текстовые поля данными
                namevactxt.Text = dt.Rows[0]["Название_вакантной_должности"].ToString();
                typevactxt.Text = dt.Rows[0]["Тип_вакансии"].ToString();
                namejobgivetxt.Text = dt.Rows[0]["Название_работодателя"].ToString();
                adressjobgivetxt.Text = dt.Rows[0]["Адрес_работодателя"].ToString();
                phonejobgivetxt.Text = dt.Rows[0]["Телефон_работодателя"].ToString();
                moneytxt.Text = dt.Rows[0]["Примерный_размер_зарплаты"].ToString();
                specialstxt.Text = dt.Rows[0]["Особые_требования_к_работнику"].ToString();

            }
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void Confirmbutton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите подтвердить трудоустройство?", "Подтверждение", MessageBoxButtons.YesNo);

            if (result == DialogResult.No)
            {
                return;
            }

            if (vacList.SelectedItems.Count > 0)
            {
                ListViewItem item = vacList.SelectedItems[0];
                int id = int.Parse(item.Text);

                // Открываем форму для ввода данных пользователем
                EmploymentForm employmentForm = new EmploymentForm();
                employmentForm.ShowDialog();

                // Получаем данные от пользователя
                DateTime DataEmp = employmentForm.SelectedDate;
                string Name = employmentForm.Name;

                // Получаем данные 
                int vac_id = id;
                int unemp_ID = unemp_id;
                DateTime data = DateTime.Now.Date;

                // Добавляем новую запись в базу данных
                await db.ConfirmEmployed(vac_id, unemp_ID, DataEmp);
                await db.AddArchiveData(unemp_ID, data, Name);

                // Задерживаем выполнение RefreshvacList() до тех пор, пока AddVacancy() не завершится
                await Task.Delay(TimeSpan.FromSeconds(1));

                // Обновляем главную форму
                programmForm.RefreshListView();
                programmForm.ClearFields();

                // Закрываем форму
                this.Close();

            }
            else
            {
                MessageBox.Show("Выберите вакансию");
            }

        }


    }
}
