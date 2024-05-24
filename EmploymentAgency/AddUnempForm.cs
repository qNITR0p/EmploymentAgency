using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;
using EmploymentAgency;
using Microsoft.Office.Interop.Access.Dao;
using System.Xml.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;



namespace EmploymentAgency
{
    public partial class AddUnempForm : Form
    {
        private string photoPath;
        private DatabaseAccess db;
        private Programm programmForm;
        private OpenFileDialog openFileDialog;
        public AddUnempForm(DatabaseAccess db, Programm programmForm)
        {
            InitializeComponent();
            this.db = db;
            this.programmForm = programmForm;

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
        }


        private async void Okbutton_Click(object sender, EventArgs e)
        {
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
            DateTime data = DateTime.Now.Date;

            // Проверяем, что все поля заполнены
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(numberpass) || string.IsNullOrWhiteSpace(givepass)
              || string.IsNullOrWhiteSpace(adres) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(level)
              || string.IsNullOrWhiteSpace(nameeducation) || string.IsNullOrWhiteSpace(document) || string.IsNullOrWhiteSpace(specialization)
              || photo == null || datapass == DateTime.MinValue || age == 0 || experience == 0)
            {
                MessageBox.Show("Заполните все поля");
                return;
            }

            if (string.IsNullOrWhiteSpace(photoPath))
            {
                throw new ArgumentException("Путь к изображению не может быть пустым или null");
            }

            // Добавляем новую запись в базу данных
            db.AddUnemployed(name, age, numberpass, datapass, givepass, adres, phone, photoPath, level, nameeducation, document, specialization, experience, data);

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
                string imagesFolderPath = Path.Combine(Application.StartupPath, "Images");

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




        //События обработки нажатия клавиши Enter, для переключения фокуса на следующее поле
        private void Nametxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; 
                Agetxt.Focus();
                Agetxt.Select(0, Agetxt.Text.Length);
            }
        }
        private void Agetxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; 
                Numberpasstxt.Focus();
            }
        }
        private void Numberpasstxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Datapasstxt.Focus();
            }
        }
        private void Datapasstxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Givepasstxt.Focus();
            }
        }
        private void Givepasstxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Adrestxt.Focus();
            }
        }
        private void Adrestxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Phonetxt.Focus();
            }
        }
        private void Phonetxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Nameeducationtxt.Focus();
            }
        }
        private void Nameeducationtxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Documenttxt.Focus();
            }
        }
        private void Documenttxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Specializationtxt.Focus();
            }
        }
        private void Specializationtxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Experiencetxt.Focus();
                Experiencetxt.Select(0, Experiencetxt.Text.Length);
            }
        }
        private void Experiencetxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; 
                this.ActiveControl = null; 
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
