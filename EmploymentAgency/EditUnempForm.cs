using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using EmploymentAgency;
using Microsoft.Office.Interop.Access.Dao;
using System.Xml.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace EmploymentAgency
{
    public partial class EditUnempForm : Form
    {
        private int id;
        private DatabaseAccess db;
        private DataTable dt;
        private Programm programmForm;
        private OpenFileDialog openFileDialog;
        private string photoPath;
        public EditUnempForm(DatabaseAccess db, Programm programmForm, int id)
        {
            InitializeComponent();
            this.db = db;
            this.programmForm = programmForm;
            this.id = id;

            // Создаем новый экземпляр OpenFileDialog
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

            // Добавляем значения в ComboBox
            Leveltxt.Items.Add("Основное общее образование");
            Leveltxt.Items.Add("Среднее общее образование");
            Leveltxt.Items.Add("Среднее профессиональное образование");
            Leveltxt.Items.Add("Высшее образование");
            Leveltxt.DropDownStyle = ComboBoxStyle.DropDownList;

            //Ограничение значений
            Agetxt.Minimum = 0;
            Agetxt.Maximum = 150;
            Experiencetxt.Minimum = 0;
            Experiencetxt.Maximum = 100;

            //Ограничение кол-ва символов для полей ввода
            Nametxt.MaxLength = 35;
            Numberpasstxt.MaxLength = 35;
            Givepasstxt.MaxLength = 100;
            Givepasstxt.ScrollBars = ScrollBars.Vertical;
            Adrestxt.MaxLength = 100;
            Adrestxt.ScrollBars = ScrollBars.Vertical;
            Phonetxt.MaxLength = 20;
            Leveltxt.MaxLength = 35;
            Nameeducationtxt.MaxLength = 35;
            Documenttxt.MaxLength = 35;
            Specializationtxt.MaxLength = 35;

            // Заполняем текстовые поля текущими данными о безработном
            FillFields();
        }

        private void FillFields()
        {
            // Получаем данные о безработном
            dt = db.GetUnemployedById(id);

            // Заполняем текстовые поля данными
            Nametxt.Text = dt.Rows[0]["ФИО"].ToString();
            Agetxt.Text = dt.Rows[0]["Возраст"].ToString();
            Numberpasstxt.Text = dt.Rows[0]["Номер_паспорта"].ToString();
            Datapasstxt.Text = ((DateTime)dt.Rows[0]["Дата_выдачи_паспорта"]).ToShortDateString();
            Givepasstxt.Text = dt.Rows[0]["Кем_выдан_паспорт"].ToString();
            Adrestxt.Text = dt.Rows[0]["Адрес"].ToString();
            Phonetxt.Text = dt.Rows[0]["Телефон"].ToString();
            Leveltxt.Text = dt.Rows[0]["Уровень_образования"].ToString();
            Nameeducationtxt.Text = dt.Rows[0]["Название_оконченного_учебного_заведения"].ToString();
            Documenttxt.Text = dt.Rows[0]["Данные_документа_об_образовании"].ToString();
            Specializationtxt.Text = dt.Rows[0]["Специальность"].ToString();
            Experiencetxt.Text = dt.Rows[0]["Стаж_работы_по_специальности"].ToString();

            // Получаем путь к изображению
            string photoFileName = dt.Rows[0]["Фотография"].ToString();

            // Создаем полный путь к изображению
            string imagesFolderPath = Path.Combine(Application.StartupPath, "Images");
            string photoPath = Path.Combine(imagesFolderPath, photoFileName);

            // Загружаем изображение из файла в PictureBox
            if (File.Exists(photoPath))
            {
                pictureBox1.Image = Image.FromFile(photoPath);
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private async void Okbutton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы уверены, что хотите внести изменения?", "Подтверждение", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                return;
            }
            // Получаем данные 
            string name = Nametxt.Text;
            int age = Convert.ToInt32(Agetxt.Value);
            string numberpass = Numberpasstxt.Text;
            DateTime datapass = Datapasstxt.Value.Date;
            string givepass = Givepasstxt.Text;
            string adres = Adrestxt.Text;
            string phone = Phonetxt.Text;
            Image photo = pictureBox1.Image;
            string level = Leveltxt.Text;
            string nameeducation = Nameeducationtxt.Text;
            string document = Documenttxt.Text;
            string specialization = Specializationtxt.Text;
            int experience = Convert.ToInt32(Experiencetxt.Value);

            // Проверяем, что все поля заполнены
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(numberpass) || string.IsNullOrWhiteSpace(givepass)
              || string.IsNullOrWhiteSpace(adres) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(level)
              || string.IsNullOrWhiteSpace(nameeducation) || string.IsNullOrWhiteSpace(document) || string.IsNullOrWhiteSpace(specialization)
              || photo == null ||  age == 0 )
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            if (string.IsNullOrWhiteSpace(photoPath))
            {
                // Если пользователь не выбрал новое изображение, используем исходный путь к изображению
                photoPath = dt.Rows[0]["Фотография"].ToString();
            }

            // Обновляем существующую запись в базе данных
            db.UpdateUnemployed(id, name, age, numberpass, datapass, givepass, adres, phone, photoPath, level, nameeducation, document, specialization, experience);

            // Задерживаем выполнение RefreshvacList() до тех пор, пока AddVacancy() не завершится
            await Task.Delay(TimeSpan.FromSeconds(1)); 


            // Обновляем главную форму
            programmForm.RefreshListView();
            programmForm.ClearFields();

            // Закрываем форму
            this.Close();
        }

        private void Imagebutton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Создаем путь к папке "Images" в директории приложения
                string imagesFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

                // Создаем путь к файлу изображения
                string imageFilePath = Path.Combine(imagesFolderPath, Path.GetFileName(openFileDialog.FileName));

                // Копируем файл изображения в папку "Images"
                try
                {
                    File.Copy(openFileDialog.FileName, imageFilePath, true);
                }
                catch (IOException ex) when ((ex.HResult & 0x0000FFFF) == 32)
                {
                    Console.WriteLine("Нарушение доступа.");
                }

                // Загружаем изображение из файла в PictureBox
                pictureBox1.Image = Image.FromFile(imageFilePath);

                // Сохраняем только имя файла изображения в переменной класса
                photoPath = Path.GetFileName(imageFilePath);
            }
        }

        //Проверка ввода пользователя через регулярные выражения
        private void Nametxt_Validating(object sender, CancelEventArgs e)
        {
            string name = Nametxt.Text;
            if (!Regex.IsMatch(name, @"^[А-Я][а-я]*( [А-Я][а-я]*)*( [А-Я][а-я]*)*$"))
            {
                e.Cancel = true;
                MessageBox.Show("ФИО должно соответствовать 'Иванов Иван Иванович'");
                Nametxt.SelectAll();
            }
        }
        private void Agetxt_Validating(object sender, CancelEventArgs e)
        {
            string age = Agetxt.Text;
            if (!Regex.IsMatch(age, @"^[1-9][0-9]*$"))
            {
                e.Cancel = true;
                MessageBox.Show("Возраст не может быть равен 0");
                Agetxt.Select(0, Agetxt.Text.Length);
            }
        }
        private void Numberpasstxt_Validating(object sender, CancelEventArgs e)
        {
            string numberpass = Numberpasstxt.Text;
            if (!Regex.IsMatch(numberpass, @"^\d{4} \d{6}$"))
            {
                e.Cancel = true;
                MessageBox.Show("Номер паспорта должен соответствовать формату '1234 567891'");
                Numberpasstxt.SelectAll();
            }
            else
            {
                // Если валидация прошла успешно, установить фокус на Datapasstxt
                Datapasstxt.Focus();
            }
        }
        private void Phonetxt_Validating(object sender, CancelEventArgs e)
        {
            string phone = Phonetxt.Text;
            if (!Regex.IsMatch(phone, @"^[1-9][0-9]{10}$"))
            {
                e.Cancel = true;
                MessageBox.Show("Номер телефона должен начинаться с любого числа, соответствующего коду страны, и далее следует 10 символов");
                Phonetxt.SelectAll();
            }
        }

        private void Closebutton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
