using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace EmploymentAgency
{
    public partial class Programm : System.Windows.Forms.Form
    {
        private DatabaseAccess db;
        public event Action RefreshListViewEvent;
        public event Action RefreshvacListEvent;
        public event Action RefreshStipendListEvent;
        public event Action RefreshArchiveListEvent;
        string role = meetForm.userRole;



        public Programm()
        {
            InitializeComponent();
            mainPanel.Visible = true;
            unempPanel.Visible = false;
            vacPanel.Visible = false;
            stipendPanel.Visible = false;
            archivePanel.Visible = false;
            this.Text = "Биржа труда : Главная";
            db = new DatabaseAccess();
            unempList.Scrollable = true;
            vacList.Scrollable = true;
            stipendList.Scrollable = true;
            archiveList.Scrollable = true;
            if (role == "Специалист службы занятости")
            {
                unempButton.Enabled = true;
                vacancyButton.Enabled = true;
                sttButton.Enabled = true;
                archiveButton.Enabled = true;
                Addunepmbutton.Enabled = true;
                Changebutton.Enabled = false;
                Addvacbutton.Enabled = false;
                archiveDeletebutton.Enabled = false;
                Confirmbutton.Enabled = false;
                Changevacbutton.Enabled = false;
                Deletevacbutton.Enabled = false;
                Deletebutton.Enabled = false;
                Stipendbutton.Enabled = false;
                DeleteStipend.Enabled = false;
                ChangeStipend.Enabled = false;
            }
            if (role == "Специалист по работе с работодателями")
            {
                unempButton.Enabled = true;
                vacancyButton.Enabled = true;
                sttButton.Enabled = true;
                archiveButton.Enabled = true;
                Addunepmbutton.Enabled = false;
                Changebutton.Enabled = false;
                Addvacbutton.Enabled = true;
                archiveDeletebutton.Enabled = false;
                Confirmbutton.Enabled = false;
                Changevacbutton.Enabled = false;
                Deletevacbutton.Enabled = false;
                Deletebutton.Enabled = false;
                Stipendbutton.Enabled = false;
                DeleteStipend.Enabled = false;
                ChangeStipend.Enabled = false;
            }
            if (role == "Специалист по трудоустройству")
            {
                unempButton.Enabled = true;
                vacancyButton.Enabled = true;
                sttButton.Enabled = true;
                archiveButton.Enabled = true;
                Addunepmbutton.Enabled = false;
                Changebutton.Enabled = false;
                Addvacbutton.Enabled = false;
                archiveDeletebutton.Enabled = false;
                Confirmbutton.Enabled = true;
                Changevacbutton.Enabled = false;
                Deletevacbutton.Enabled = false;
                Deletebutton.Enabled = false;
                Stipendbutton.Enabled = true;
                DeleteStipend.Enabled = true;
                ChangeStipend.Enabled = true;
            }
            if (role == "Администратор")
            {
                unempButton.Enabled = true;
                vacancyButton.Enabled = true;
                sttButton.Enabled = true;
                Addunepmbutton.Enabled = true;
                archiveButton.Enabled = true;
                Changebutton.Enabled = true;
                Addvacbutton.Enabled = true;
                archiveDeletebutton.Enabled = true;
                Confirmbutton.Enabled = true;
                Changevacbutton.Enabled = true;
                Deletevacbutton.Enabled = true;
                Deletebutton.Enabled = true;
                Stipendbutton.Enabled = true;
                DeleteStipend.Enabled = true;
                ChangeStipend.Enabled = true;
            }

        }



        //Нажатие на кнопку Безработные
        private void unempButton_Click(object sender, EventArgs e)
        {
            mainPanel.Visible = false;
            unempPanel.Visible = true;
            vacPanel.Visible = false;
            stipendPanel.Visible = false;
            archivePanel.Visible = false;
            this.Text = "Биржа труда : Безработные";


            // Создаем новый экземпляр класса DatabaseAccess
            DatabaseAccess db = new DatabaseAccess();

            // Получаем данные о безработных
            DataTable dt = db.GetUnemployed();

            // Если столбцы уже существуют, очищаем ListView
            if (unempList.Columns.Count > 0)
            {
                unempList.Items.Clear();
            }
            else
            {
                // Добавляем столбцы в ListView
                unempList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                unempList.View = View.Details;
                unempList.FullRowSelect = true;
                unempList.Columns.Add("ID", 100, HorizontalAlignment.Left);
                unempList.Columns.Add("ФИО", 200, HorizontalAlignment.Left);
                unempList.Columns[0].Width = (int)(unempList.Width * 0.3);
                unempList.Columns[1].Width = (int)(unempList.Width * 0.60);
            }

            // Добавляем данные в ListView
            foreach (DataRow row in dt.Rows)
            {
                int id = Convert.ToInt32(row["ID"]);
                if (!db.HasStipendForUnemployed(id) && !db.HasEmploymentForUnemployed(id))
                {
                    ListViewItem item = new ListViewItem(row["ID"].ToString());
                    item.SubItems.Add(row["ФИО"].ToString());
                    unempList.Items.Add(item);
                }
            }
        }

        //Нажатие на кнопку Вакансии
        private void vacancyButton_Click(object sender, EventArgs e)
        {
            mainPanel.Visible = false;
            unempPanel.Visible = false;
            vacPanel.Visible = true;
            stipendPanel.Visible = false;
            archivePanel.Visible = false;
            this.Text = "Биржа труда : Вакансии";


            // Создаем новый экземпляр класса DatabaseAccess
            DatabaseAccess db = new DatabaseAccess();

            // Получаем данные о вакансиях
            DataTable dt = db.GetVacancies();

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

        //Нажатие на кнопку Пособие по безработице
        private void sttButton_Click(object sender, EventArgs e)
        {
            mainPanel.Visible = false;
            unempPanel.Visible = false;
            vacPanel.Visible = false;
            stipendPanel.Visible = true;
            archivePanel.Visible = false;
            this.Text = "Биржа труда : Пособия по безработице";


            // Создаем новый экземпляр класса DatabaseAccess
            DatabaseAccess db = new DatabaseAccess();

            // Получаем данные о безработных
            DataTable dt = db.GetUnemployed();

            // Если столбцы уже существуют, очищаем ListView
            if (stipendList.Columns.Count > 0)
            {
                stipendList.Items.Clear();
            }
            else
            {
                // Добавляем столбцы в ListView
                stipendList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                stipendList.View = View.Details;
                stipendList.FullRowSelect = true;
                stipendList.Columns.Add("ID", 100, HorizontalAlignment.Left);
                stipendList.Columns.Add("ФИО", 200, HorizontalAlignment.Left);
                stipendList.Columns[0].Width = (int)(stipendList.Width * 0.3);
                stipendList.Columns[1].Width = (int)(stipendList.Width * 0.60);
            }

            // Добавляем данные в ListView
            foreach (DataRow row in dt.Rows)
            {
                int id = Convert.ToInt32(row["ID"]);
                if (db.HasStipendForUnemployed(id))
                {
                    ListViewItem item = new ListViewItem(row["ID"].ToString());
                    item.SubItems.Add(row["ФИО"].ToString());
                    stipendList.Items.Add(item);
                }
            }
        }

        //Нажатие на кнопку Архив
        private async void archiveButton_Click(object sender, EventArgs e)
        {
            mainPanel.Visible = false;
            unempPanel.Visible = false;
            vacPanel.Visible = false;
            stipendPanel.Visible = false;
            archivePanel.Visible = true;
            this.Text = "Биржа труда : Архив";


            // Создаем новый экземпляр класса DatabaseAccess
            DatabaseAccess db = new DatabaseAccess();

            // Получаем данные для архива
            DataTable dt = await db.GetArchivedData();

            // Если столбцы уже существуют, очищаем ListView
            if (archiveList.Columns.Count > 0)
            {
                archiveList.Items.Clear();
            }
            else
            {
                // Добавляем столбцы в ListView
                archiveList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                archiveList.View = View.Details;
                archiveList.FullRowSelect = true;
                archiveList.Columns.Add("ID", 100, HorizontalAlignment.Left);
                archiveList.Columns.Add("ФИО", 200, HorizontalAlignment.Left);
                archiveList.Columns[0].Width = (int)(archiveList.Width * 0.3);
                archiveList.Columns[1].Width = (int)(archiveList.Width * 0.60);
            }

            // Добавляем данные в ListView
            foreach (DataRow row in dt.Rows)
            {
                ListViewItem item = new ListViewItem(row["ID"].ToString());
                item.SubItems.Add(row["ФИО"].ToString());
                archiveList.Items.Add(item);
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
        private void unempList_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (busy) return;
            busy = true;
            if (e.ColumnIndex == 0)
                unempList.Columns[e.ColumnIndex].Width = (int)(unempList.Width * 0.3);
            else if (e.ColumnIndex == 1)
                unempList.Columns[e.ColumnIndex].Width = (int)(unempList.Width * 0.60);
            busy = false;

        }
        private void stipendList_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (busy) return;
            busy = true;
            if (e.ColumnIndex == 0)
                stipendList.Columns[e.ColumnIndex].Width = (int)(stipendList.Width * 0.3);
            else if (e.ColumnIndex == 1)
                stipendList.Columns[e.ColumnIndex].Width = (int)(stipendList.Width * 0.60);
            busy = false;
        }
        private void archiveList_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (busy) return;
            busy = true;
            if (e.ColumnIndex == 0)
                archiveList.Columns[e.ColumnIndex].Width = (int)(archiveList.Width * 0.3);
            else if (e.ColumnIndex == 1)
                archiveList.Columns[e.ColumnIndex].Width = (int)(archiveList.Width * 0.60);
            busy = false;
        }

        //Обновление unempList
        internal void RefreshListView()
        {
            // Используем уже существующий экземпляр класса DatabaseAccess
            DataTable dt = db.GetUnemployed();

            // Очищаем ListView
            unempList.Items.Clear();

            // Начинаем обновление ListView
            unempList.BeginUpdate();

            // Добавляем новые данные в ListView
            foreach (DataRow row in dt.Rows)
            {
                int id = Convert.ToInt32(row["ID"]);
                if (!db.HasStipendForUnemployed(id) && !db.HasEmploymentForUnemployed(id))
                {
                    ListViewItem item = new ListViewItem(row["ID"].ToString());
                    item.SubItems.Add(row["ФИО"].ToString());
                    unempList.Items.Add(item);
                }
            }

            // Завершаем обновление ListView
            unempList.EndUpdate();

            // Вызываем событие
            RefreshListViewEvent?.Invoke();
        }

        //Обновление vacList
        internal void RefreshvacList()
        {
            // Используем уже существующий экземпляр класса DatabaseAccess
            DataTable dt = db.GetVacancies();

            // Очищаем ListView
            vacList.Items.Clear();

            // Начинаем обновление ListView
            vacList.BeginUpdate();

            // Добавляем новые данные в ListView
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

            // Завершаем обновление ListView
            vacList.EndUpdate();

            // Вызываем событие
            RefreshvacListEvent?.Invoke();
        }

        //Обновление stipendList
        internal void RefreshStipendList()
        {
            // Используем уже существующий экземпляр класса DatabaseAccess
            DataTable dt = db.GetUnemployed();

            // Очищаем ListView
            stipendList.Items.Clear();

            // Начинаем обновление ListView
            stipendList.BeginUpdate();

            // Добавляем новые данные в ListView
            foreach (DataRow row in dt.Rows)
            {
                int id = Convert.ToInt32(row["ID"]);
                if (db.HasStipendForUnemployed(id))
                {
                    ListViewItem item = new ListViewItem(row["ID"].ToString());
                    item.SubItems.Add(row["ФИО"].ToString());
                    stipendList.Items.Add(item);
                }
            }

            // Завершаем обновление ListView
            stipendList.EndUpdate();

            // Вызываем событие
            RefreshStipendListEvent?.Invoke();
        }

        //Обновление archiveList
        internal async void RefreshArchiveList()
        {
            // Используем уже существующий экземпляр класса DatabaseAccess
            DataTable dt = await db.GetArchivedData();

            // Очищаем текущий список
            archiveList.Items.Clear();

            // Начинаем обновление ListView
            archiveList.BeginUpdate();

            // Добавляем обновленные данные в список
            foreach (DataRow row in dt.Rows)
            {
                ListViewItem item = new ListViewItem(row["ID"].ToString());
                item.SubItems.Add(row["ФИО"].ToString());
                archiveList.Items.Add(item);
            }

            // Завершаем обновление ListView
            archiveList.EndUpdate();

            // Вызываем событие
            RefreshArchiveListEvent?.Invoke();
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

        //Выбор строки в unempList для отображения данных
        private void unempList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (unempList.SelectedItems.Count > 0)
            {
                // Получаем выбранный элемент
                ListViewItem item = unempList.SelectedItems[0];

                // Получаем ID безработного
                int id = int.Parse(item.Text);

                // Создаем новый экземпляр класса DatabaseAccess
                DatabaseAccess db = new DatabaseAccess();

                // Получаем данные о безработном
                DataTable dt = db.GetUnemployedById(id);

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
                Datatxt.Text = ((DateTime)dt.Rows[0]["Дата_постановки_на_учет"]).ToString("dd.MM.yyyy");

                // Получаем путь к изображению
                string photoFileName = dt.Rows[0]["Фотография"].ToString();

                // Создаем полный путь к изображению
                string imagesFolderPath = Path.Combine(Application.StartupPath, "Images");
                string photoPath = Path.Combine(imagesFolderPath, photoFileName);

                // Загружаем изображение из файла в pictureBoxMain
                if (File.Exists(photoPath))
                {
                    pictureBoxMain.Image = Image.FromFile(photoPath);
                    pictureBoxMain.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }

        //Выбор строки в stipendList для отображения данных
        private void stipendList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (stipendList.SelectedItems.Count > 0)
            {
                // Получаем выбранный элемент
                ListViewItem item = stipendList.SelectedItems[0];

                // Получаем ID безработного
                int id = int.Parse(item.Text);

                // Получаем данные о безработном
                DataTable dt = db.GetUnemployedById(id);

                // Получаем данные о пособиях
                DataTable stipends = db.GetStipendsByUnemployedId(id);

                // Заполняем текстовые поля данными
                stNametxt.Text = dt.Rows[0]["ФИО"].ToString();
                stAgetxt.Text = dt.Rows[0]["Возраст"].ToString();
                stNumberpasstxt.Text = dt.Rows[0]["Номер_паспорта"].ToString();
                stDatapasstxt.Text = ((DateTime)dt.Rows[0]["Дата_выдачи_паспорта"]).ToShortDateString();
                stGivepasstxt.Text = dt.Rows[0]["Кем_выдан_паспорт"].ToString();
                stAdrestxt.Text = dt.Rows[0]["Адрес"].ToString();
                stPhonetxt.Text = dt.Rows[0]["Телефон"].ToString();
                stLeveltxt.Text = dt.Rows[0]["Уровень_образования"].ToString();
                stNameeducationtxt.Text = dt.Rows[0]["Название_оконченного_учебного_заведения"].ToString();
                stDocumenttxt.Text = dt.Rows[0]["Данные_документа_об_образовании"].ToString();
                stSpecializationtxt.Text = dt.Rows[0]["Специальность"].ToString();
                stExperiencetxt.Text = dt.Rows[0]["Стаж_работы_по_специальности"].ToString();
                stDatatxt.Text = ((DateTime)dt.Rows[0]["Дата_постановки_на_учет"]).ToString("dd.MM.yyyy");

                stStipendtxt.Text = stipends.Rows[0]["Сумма_пособия"].ToString();
                stDatastipendtxt.Text = (stipends.Rows[0]["Дата_выплаты_пособия"]).ToString();

                // Получаем путь к изображению
                string photoFileName = dt.Rows[0]["Фотография"].ToString();

                // Создаем полный путь к изображению
                string imagesFolderPath = Path.Combine(Application.StartupPath, "Images");
                string photoPath = Path.Combine(imagesFolderPath, photoFileName);

                // Загружаем изображение из файла в pictureBoxMain
                if (File.Exists(photoPath))
                {
                    pictureBoxStipend.Image = Image.FromFile(photoPath);
                    pictureBoxStipend.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }

        //Выбор строки в archiveList для отображения данных
        private void archiveList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (archiveList.SelectedItems.Count > 0)
            {
                // Получаем выбранный элемент
                ListViewItem item = archiveList.SelectedItems[0];

                // Получаем ID архивной записи
                int id = int.Parse(item.Text);

                // Создаем новый экземпляр класса DatabaseAccess
                DatabaseAccess db = new DatabaseAccess();

                // Получаем данные о безработном
                DataTable dt = db.GetUnemployedById(id);

                // Получаем ID_вакансии из таблицы Трудоустройство
                int vacancyId = db.GetVacancyIdByEmploymentId(id);

                // Получаем данные о вакансии
                DataTable vac = db.GetVacanciesById(vacancyId);


                // Заполняем текстовые поля данными
                arnamevactxt.Text = vac.Rows[0]["Название_вакантной_должности"].ToString();
                artypevactxt.Text = vac.Rows[0]["Тип_вакансии"].ToString();
                arnamejobgivetxt.Text = vac.Rows[0]["Название_работодателя"].ToString();
                aradressjobgivetxt.Text = vac.Rows[0]["Адрес_работодателя"].ToString();
                arphonejobgivetxt.Text = vac.Rows[0]["Телефон_работодателя"].ToString();
                armoneytxt.Text = vac.Rows[0]["Примерный_размер_зарплаты"].ToString();
                arspecialstxt.Text = vac.Rows[0]["Особые_требования_к_работнику"].ToString();

                arNametxt.Text = dt.Rows[0]["ФИО"].ToString();
                arAgetxt.Text = dt.Rows[0]["Возраст"].ToString();
                arNumberpasstxt.Text = dt.Rows[0]["Номер_паспорта"].ToString();
                arDatapasstxt.Text = ((DateTime)dt.Rows[0]["Дата_выдачи_паспорта"]).ToShortDateString();
                arGivepasstxt.Text = dt.Rows[0]["Кем_выдан_паспорт"].ToString();
                arAdrestxt.Text = dt.Rows[0]["Адрес"].ToString();
                arPhonetxt.Text = dt.Rows[0]["Телефон"].ToString();
                arLeveltxt.Text = dt.Rows[0]["Уровень_образования"].ToString();
                arNameeducationtxt.Text = dt.Rows[0]["Название_оконченного_учебного_заведения"].ToString();
                arDocumenttxt.Text = dt.Rows[0]["Данные_документа_об_образовании"].ToString();
                arSpecializationtxt.Text = dt.Rows[0]["Специальность"].ToString();
                arExperiencetxt.Text = dt.Rows[0]["Стаж_работы_по_специальности"].ToString();
                arDatatxt.Text = ((DateTime)dt.Rows[0]["Дата_постановки_на_учет"]).ToString("dd.MM.yyyy");
            }
        }

        //Отчистка полей в unempList
        internal void ClearFields()
        {
            // Очищаем текстовые поля
            Nametxt.Text = "";
            Agetxt.Text = "";
            Numberpasstxt.Text = "";
            Datapasstxt.Text = "";
            Givepasstxt.Text = "";
            Adrestxt.Text = "";
            Phonetxt.Text = "";
            Leveltxt.Text = "";
            Nameeducationtxt.Text = "";
            Documenttxt.Text = "";
            Specializationtxt.Text = "";
            Experiencetxt.Text = "";
            Datatxt.Text = "";

            // Очищаем изображение
            pictureBoxMain.Image = null;
        }

        //Отчистка полей в stipendList
        internal void ClearFieldsStipend()
        {
            // Очищаем текстовые поля
            stNametxt.Text = "";
            stAgetxt.Text = "";
            stNumberpasstxt.Text = "";
            stDatapasstxt.Text = "";
            stGivepasstxt.Text = "";
            stAdrestxt.Text = "";
            stPhonetxt.Text = "";
            stLeveltxt.Text = "";
            stNameeducationtxt.Text = "";
            stDocumenttxt.Text = "";
            stSpecializationtxt.Text = "";
            stExperiencetxt.Text = "";
            stDatatxt.Text = "";
            stStipendtxt.Text = "";
            stDatastipendtxt.Text = "";


            // Очищаем изображение
            pictureBoxStipend.Image = null;
        }

        //Отчистка полей в vacList
        internal void ClearFieldsvacList()
        {
            // Очищаем текстовые поля
            namevactxt.Text = "";
            typevactxt.Text = "";
            namejobgivetxt.Text = "";
            adressjobgivetxt.Text = "";
            phonejobgivetxt.Text = "";
            moneytxt.Text = "";
            specialstxt.Text = "";
        }

        //Отчистка полей в archiveList
        internal void ClearFieldsarchiveList()
        {
            // Очищаем текстовые поля
            arnamevactxt.Text = "";
            artypevactxt.Text = "";
            arnamejobgivetxt.Text = "";
            aradressjobgivetxt.Text = "";
            arphonejobgivetxt.Text = "";
            armoneytxt.Text = "";
            arspecialstxt.Text = "";

            arNametxt.Text = "";
            arAgetxt.Text = "";
            arNumberpasstxt.Text = "";
            arDatapasstxt.Text = "";
            arGivepasstxt.Text = "";
            arAdrestxt.Text = "";
            arPhonetxt.Text = "";
            arLeveltxt.Text = "";
            arNameeducationtxt.Text = "";
            arDocumenttxt.Text = "";
            arSpecializationtxt.Text = "";
            arExperiencetxt.Text = "";
            arDatatxt.Text = "";
        }

        //Открыть форму для добавления новой записи в unempList
        private void Addunepmbutton_Click(object sender, EventArgs e)
        {
            // Создаем новый экземпляр класса DatabaseAccess
            DatabaseAccess db = new DatabaseAccess();

            // Создаем новый экземпляр формы AddUnempForm
            AddUnempForm addUnempForm = new AddUnempForm(db, this);

            // Отображаем форму
            addUnempForm.ShowDialog();

        }

        //Кнопка для внесения изменений в запись в unempList
        private void Changebutton_Click(object sender, EventArgs e)
        {
            if (unempList.SelectedItems.Count > 0)
            {
                ListViewItem item = unempList.SelectedItems[0];
                int id = int.Parse(item.Text);

                EditUnempForm editUnempForm = new EditUnempForm(db, this, id);
                editUnempForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите запись для внесения изменений");
            }
        }

        //Кнопка для удаления выбранной записи в unempList
        private async void Deletebutton_Click(object sender, EventArgs e)
        {
            if (unempList.SelectedItems.Count > 0)
            {
                ListViewItem item = unempList.SelectedItems[0];
                int id = int.Parse(item.Text);

                // Показываем сообщение предупреждения
                DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Предупреждение", MessageBoxButtons.YesNo);

                // Если пользователь нажал "Нет", то прерываем выполнение метода
                if (result == DialogResult.No)
                {
                    return;
                }

                // Удаляем запись из базы данных
                await db.DeleteUnemployed(id);

                // Обновляем главную форму
                RefreshListView();
                ClearFields();
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления");
            }
        }

        //Кнопка для назначения пособия
        private void Stipendbutton_Click(object sender, EventArgs e)
        {
            if (unempList.SelectedItems.Count > 0)
            {
                ListViewItem item = unempList.SelectedItems[0];
                int id = int.Parse(item.Text);

                // Проверяем, назначено ли уже пособие
                if (db.HasStipendForUnemployed(id))
                {
                    MessageBox.Show("Пособие уже назначено");
                    return;
                }

                StipendForm stipendForm = new StipendForm(db, this, id);
                stipendForm.ShowDialog();

            }
            else
            {
                MessageBox.Show("Выберите запись для назначения пособия");
            }
        }

        //кнопка для удаления пособия
        private async void DeleteStipend_Click(object sender, EventArgs e)
        {
            if (stipendList.SelectedItems.Count > 0)
            {
                // Получаем выбранный элемент
                ListViewItem item = stipendList.SelectedItems[0];

                // Получаем ID пособия
                int id = int.Parse(item.Text);

                // Показываем сообщение предупреждения
                DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить пособие?", "Предупреждение", MessageBoxButtons.YesNo);

                // Если пользователь нажал "Нет", то прерываем выполнение метода
                if (result == DialogResult.No)
                {
                    return;
                }

                // Удаляем пособие из базы данных
                await db.DeleteStipend(id);

                // Обновляем список пособий
                RefreshStipendList();
                ClearFieldsStipend();

            }
            else
            {
                MessageBox.Show("Выберите пособие для удаления");
            }
        }

        //Кнопка для внесения изменений в пособие
        private void ChangeStipend_Click(object sender, EventArgs e)
        {
            if (stipendList.SelectedItems.Count > 0)
            {
                ListViewItem item = stipendList.SelectedItems[0];
                int id = int.Parse(item.Text);

                EditStipendForm editStipendForm = new EditStipendForm(db, this, id);
                editStipendForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите пособие для редактирования");
            }
        }

        //Открыть форму для добавления новой записи в vacList
        private void Addvacbutton_Click(object sender, EventArgs e)
        {
            // Создаем новый экземпляр класса DatabaseAccess
            DatabaseAccess db = new DatabaseAccess();

            // Создаем новый экземпляр формы AddUnempForm
            AddVacForm addVacForm = new AddVacForm(db, this);

            // Отображаем форму
            addVacForm.ShowDialog();
        }

        //Кнопка для удаления выбранной записи в vacList
        private async void Deletevacbutton_Click(object sender, EventArgs e)
        {
            if (vacList.SelectedItems.Count > 0)
            {
                ListViewItem item = vacList.SelectedItems[0];
                int id = int.Parse(item.Text);

                // Показываем сообщение предупреждения
                DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить эту запись?", "Предупреждение", MessageBoxButtons.YesNo);

                // Если пользователь нажал "Нет", то прерываем выполнение метода
                if (result == DialogResult.No)
                {
                    return;
                }

                // Удаляем запись из базы данных
                await db.DeleteVacancy(id);

                // Обновляем главную форму
                RefreshvacList();
                ClearFieldsvacList();
            }
            else
            {
                MessageBox.Show("Выберите запись для удаления");
            }
        }

        //Кнопка для внесения изменений в Вакансию
        private void Changevacbutton_Click(object sender, EventArgs e)
        {
            if (vacList.SelectedItems.Count > 0)
            {
                ListViewItem item = vacList.SelectedItems[0];
                int id = int.Parse(item.Text);

                EditVacForm editVacForm = new EditVacForm(db, this, id);
                editVacForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите запись для внесения изменений");
            }
        }

        //Кнопка для открытия формы подтверждения трудоустроиства
        private void Confirmbutton_Click(object sender, EventArgs e)
        {
            if (unempList.SelectedItems.Count > 0)
            {
                ListViewItem item = unempList.SelectedItems[0];
                int id = int.Parse(item.Text);

                ConfirmEmploymentForm confirmEmploymentForm = new ConfirmEmploymentForm(db, this, id);
                confirmEmploymentForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите запись для подтверждения трудоустройства");
            }
        }

        //Кнопка для восстановления записи из архива
        private async void archiveDeletebutton_Click(object sender, EventArgs e)
        {
            if (archiveList.SelectedItems.Count > 0)
            {
                ListViewItem item = archiveList.SelectedItems[0];
                int id = int.Parse(item.Text);

                // Показываем сообщение предупреждения
                DialogResult result = MessageBox.Show("Вы уверены, что хотите восстановить  эту запись?", "Предупреждение", MessageBoxButtons.YesNo);

                // Если пользователь нажал "Нет", то прерываем выполнение метода
                if (result == DialogResult.No)
                {
                    return;
                }

                // Удаляем запись из базы данных
                await db.DeleteArchiveData(id);

                // Обновляем главную форму
                RefreshArchiveList();
                ClearFieldsarchiveList();
            }
            else
            {
                MessageBox.Show("Выберите запись для восстановления");
            }
        }

        //Кнопка Главная
        private void главнаяToolStripMenuItem_Click(object sender, EventArgs e)
        {

            mainPanel.Visible = true;
            unempPanel.Visible = false;
            vacPanel.Visible = false;
            stipendPanel.Visible = false;
            archivePanel.Visible = false;
            this.Text = "Биржа труда : Главная";


            ClearFields();
            ClearFieldsvacList();
            ClearFieldsStipend();
            ClearFieldsarchiveList();

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                главнаяToolStripMenuItem_Click(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }


        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void оПриложенииToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Это приложение разработано для взаимодействия с БД Биржа труда");
        }

        private void горячиеКлавишыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Клавиша ESC открывает главную страницу приложения");
        }
    }

    //Класс с методами для работы с БД
    public class DatabaseAccess
    {
        public OleDbConnection Connection { get; private set; }

        private string databasePath;

        public DatabaseAccess()
        {
            string dbDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DB");
            string DatabasePath = Path.Combine(dbDir, "EmploymentAgencyBD.accdb");
            databasePath = DatabasePath;
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + databasePath + ";Persist Security Info=False;";
            Connection = new OleDbConnection(connectionString);
        }

        public DataTable GetUnemployed()
        {
            string query = "SELECT ID, ФИО FROM Безработные";

            using (OleDbCommand command = new OleDbCommand(query, Connection))
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public DataTable GetUnemployedById(int id)
        {
            string query = "SELECT * FROM Безработные WHERE ID = " + id;

            using (OleDbCommand command = new OleDbCommand(query, Connection))
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public void AddUnemployed(string name, int age, string numberpass, DateTime datapass, string givepass, string adres, string phone, string photoPath, string level, string nameeducation, string document, string specialization, int experience, DateTime data)
        {
            string query = "INSERT INTO Безработные (ФИО, Возраст, Номер_паспорта, Дата_выдачи_паспорта, Кем_выдан_паспорт, Адрес, Телефон, Фотография, Уровень_образования, Название_оконченного_учебного_заведения, Данные_документа_об_образовании, Специальность, Стаж_работы_по_специальности, Дата_постановки_на_учет) VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14)";

            using (OleDbCommand command = new OleDbCommand(query, Connection))
            {
                command.Parameters.AddWithValue("@p1", name);
                command.Parameters.AddWithValue("@p2", age);
                command.Parameters.AddWithValue("@p3", numberpass);
                command.Parameters.AddWithValue("@p4", datapass);
                command.Parameters.AddWithValue("@p5", givepass);
                command.Parameters.AddWithValue("@p6", adres);
                command.Parameters.AddWithValue("@p7", phone);
                command.Parameters.AddWithValue("@p8", photoPath);
                command.Parameters.AddWithValue("@p9", level);
                command.Parameters.AddWithValue("@p10", nameeducation);
                command.Parameters.AddWithValue("@p11", document);
                command.Parameters.AddWithValue("@p12", specialization);
                command.Parameters.AddWithValue("@p13", experience);
                command.Parameters.AddWithValue("@p14", data);

                Connection.Open();
                command.ExecuteNonQuery();
                Connection.Close();
            }
        }

        public void UpdateUnemployed(int id, string name, int age, string numberpass, DateTime datapass, string givepass, string adres, string phone, string photoPath, string level, string nameeducation, string document, string specialization, int experience)
        {
            string query = "UPDATE Безработные SET ФИО = @p1, Возраст = @p2, Номер_паспорта = @p3, Дата_выдачи_паспорта = @p4, Кем_выдан_паспорт = @p5, Адрес = @p6, Телефон = @p7, Фотография = @p8, Уровень_образования = @p9, Название_оконченного_учебного_заведения = @p10, Данные_документа_об_образовании = @p11, Специальность = @p12, Стаж_работы_по_специальности = @p13 WHERE ID = @p14";

            using (OleDbCommand command = new OleDbCommand(query, Connection))
            {
                command.Parameters.AddWithValue("@p1", name);
                command.Parameters.AddWithValue("@p2", age);
                command.Parameters.AddWithValue("@p3", numberpass);
                command.Parameters.AddWithValue("@p4", datapass);
                command.Parameters.AddWithValue("@p5", givepass);
                command.Parameters.AddWithValue("@p6", adres);
                command.Parameters.AddWithValue("@p7", phone);
                command.Parameters.AddWithValue("@p8", photoPath);
                command.Parameters.AddWithValue("@p9", level);
                command.Parameters.AddWithValue("@p10", nameeducation);
                command.Parameters.AddWithValue("@p11", document);
                command.Parameters.AddWithValue("@p12", specialization);
                command.Parameters.AddWithValue("@p13", experience);
                command.Parameters.AddWithValue("@p14", id);

                Connection.Open();
                command.ExecuteNonQuery();
                Connection.Close();
            }
        }

        public async Task DeleteUnemployed(int id)
        {
            string query = "DELETE FROM Безработные WHERE ID = @p1";

            using (OleDbCommand command = new OleDbCommand(query, Connection))
            {
                command.Parameters.AddWithValue("@p1", id);

                await Connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                Connection.Close();
            }
        }

        public void AssignStipend(int id, decimal stipendAmount, int stipendDate)
        {
            string query = "INSERT INTO Пособия (ID_безработного, Сумма_пособия, Дата_выплаты_пособия) VALUES (@p1, @p2, @p3)";

            using (OleDbCommand command = new OleDbCommand(query, Connection))
            {
                command.Parameters.AddWithValue("@p1", id);
                command.Parameters.AddWithValue("@p2", stipendAmount);
                command.Parameters.AddWithValue("@p3", stipendDate);

                Connection.Open();
                command.ExecuteNonQuery();
                Connection.Close();
            }
        }

        public DataTable GetStipendsByUnemployedId(int id)
        {
            string query = "SELECT * FROM Пособия WHERE ID_безработного = @ID_безработного";

            using (OleDbCommand command = new OleDbCommand(query, Connection))
            {
                command.Parameters.AddWithValue("@ID_безработного", id);

                using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public bool HasStipendForUnemployed(int id)
        {
            string query = "SELECT COUNT(*) FROM Пособия WHERE ID_безработного = @ID_безработного";

            using (OleDbCommand command = new OleDbCommand(query, Connection))
            {
                command.Parameters.AddWithValue("@ID_безработного", id);

                Connection.Open();
                int count = (int)command.ExecuteScalar();
                Connection.Close();

                return count > 0;
            }
        }

        public bool HasEmploymentForUnemployed(int id)
        {
            string query = "SELECT COUNT(*) FROM Трудоустройство WHERE ID_безработного = @id";

            using (OleDbCommand command = new OleDbCommand(query, Connection))
            {
                command.Parameters.AddWithValue("@id", id);

                Connection.Open();
                int count = (int)command.ExecuteScalar();
                Connection.Close();

                return count > 0;
            }
        }

        public bool HasEmploymentForVacancy(int id)
        {
            string query = "SELECT COUNT(*) FROM Трудоустройство WHERE ID_вакансии = @id";

            using (OleDbCommand command = new OleDbCommand(query, Connection))
            {
                command.Parameters.AddWithValue("@id", id);

                Connection.Open();
                int count = (int)command.ExecuteScalar();
                Connection.Close();

                return count > 0;
            }
        }



        public DataTable GetVacancies()
        {
            string query = "SELECT ID, Название_вакантной_должности FROM Вакансии";

            using (OleDbCommand command = new OleDbCommand(query, Connection))
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public DataTable GetVacanciesById(int id)
        {
            string query = "SELECT * FROM Вакансии WHERE ID = " + id;

            using (OleDbCommand command = new OleDbCommand(query, Connection))
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                {
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    return table;
                }
            }
        }

        public async Task AddVacancy(string namevac, string typevac, string namejobgive, string adressjobgive, string phonejobgive, string money, string specials)
        {
            string query = "INSERT INTO Вакансии (Тип_вакансии, Название_вакантной_должности, Название_работодателя, Адрес_работодателя, Телефон_работодателя, Примерный_размер_зарплаты, Особые_требования_к_работнику) VALUES (@p1, @p2, @p3, @p4, @p5, @p6, @p7)";

            using (OleDbCommand command = new OleDbCommand(query, Connection))
            {
                command.Parameters.AddWithValue("@p1", typevac);
                command.Parameters.AddWithValue("@p2", namevac);
                command.Parameters.AddWithValue("@p3", namejobgive);
                command.Parameters.AddWithValue("@p4", adressjobgive);
                command.Parameters.AddWithValue("@p5", phonejobgive);
                command.Parameters.AddWithValue("@p6", money);
                command.Parameters.AddWithValue("@p7", specials);

                await Connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                Connection.Close();
            }
        }

        public async Task UpdateVacancy(int id, string namevac, string typevac, string namejobgive, string adressjobgive, string phonejobgive, string money, string specials)
        {
            string query = "UPDATE Вакансии SET Тип_вакансии = @p1, Название_вакантной_должности = @p2, Название_работодателя = @p3, Адрес_работодателя = @p4, Телефон_работодателя = @p5, Примерный_размер_зарплаты = @p6, Особые_требования_к_работнику = @p7 WHERE Id = @p8";

            using (OleDbCommand command = new OleDbCommand(query, Connection))
            {
                command.Parameters.AddWithValue("@p1", typevac);
                command.Parameters.AddWithValue("@p2", namevac);
                command.Parameters.AddWithValue("@p3", namejobgive);
                command.Parameters.AddWithValue("@p4", adressjobgive);
                command.Parameters.AddWithValue("@p5", phonejobgive);
                command.Parameters.AddWithValue("@p6", money);
                command.Parameters.AddWithValue("@p7", specials);
                command.Parameters.AddWithValue("@p8", id);

                await Connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                Connection.Close();
            }
        }

        public async Task DeleteVacancy(int id)
        {
            string query = "DELETE FROM Вакансии WHERE ID = @p1";

            using (OleDbCommand command = new OleDbCommand(query, Connection))
            {
                command.Parameters.AddWithValue("@p1", id);

                await Connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                Connection.Close();
            }
        }

        public async Task DeleteStipend(int unemployedId)
        {
            string query = "DELETE FROM Пособия WHERE ID_безработного = @ID_безработного";

            using (OleDbCommand command = new OleDbCommand(query, Connection))
            {
                command.Parameters.AddWithValue("@ID_безработного", unemployedId);

                await Connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                Connection.Close();
            }
        }

        public async Task UpdateStipend(int unemployedId, decimal stipendAmount, int stipendDate)
        {
            string query = "UPDATE Пособия SET Сумма_пособия = @p1, Дата_выплаты_пособия = @p2 WHERE ID_безработного = @p3";

            using (OleDbCommand command = new OleDbCommand(query, Connection))
            {
                command.Parameters.AddWithValue("@p1", stipendAmount);
                command.Parameters.AddWithValue("@p2", stipendDate);
                command.Parameters.AddWithValue("@p3", unemployedId);

                await Connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                Connection.Close();
            }
        }

        public async Task ConfirmEmployed(int vac_id, int unemp_ID, DateTime data)
        {
            string query = "INSERT INTO Трудоустройство (ID_вакансии, ID_безработного, Дата_трудоустроиства) VALUES (@p1, @p2, @p3)";

            using (OleDbCommand command = new OleDbCommand(query, Connection))
            {
                command.Parameters.AddWithValue("@p1", vac_id);
                command.Parameters.AddWithValue("@p2", unemp_ID);
                command.Parameters.AddWithValue("@p3", data);

                await Connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
                Connection.Close();
            }
        }

        public async Task AddArchiveData(int unemp_ID, DateTime data, string Name)
        {
            string query = "INSERT INTO Архив (ID_безработного, Дата_перевода_в_архив, Лицо_выполнившее_операцию) VALUES (@p1, @p2, @p3)";

            using (OleDbCommand command = new OleDbCommand(query, Connection))
            {
                command.Parameters.AddWithValue("p1", unemp_ID);
                command.Parameters.AddWithValue("p2", data);
                command.Parameters.AddWithValue("p3", Name);

                Connection.Open();
                await command.ExecuteNonQueryAsync();
                Connection.Close();
            }
        }

        public async Task<DataTable> GetArchivedData()
        {
            string query = @"SELECT Архив.ID_безработного AS ID, Безработные.ФИО AS ФИО
                 FROM Архив
                 INNER JOIN Безработные ON Архив.ID_безработного = Безработные.ID";

            DataTable dt = new DataTable();

            using (OleDbCommand command = new OleDbCommand(query, Connection))
            {
                Connection.Open();
                DbDataReader reader = await command.ExecuteReaderAsync();

                dt.Load(reader);

                Connection.Close();
            }

            return dt;
        }

        public int GetVacancyIdByEmploymentId(int employmentId)
        {
            string query = "SELECT ID_вакансии FROM Трудоустройство WHERE ID_безработного = @employmentId";

            using (OleDbCommand command = new OleDbCommand(query, Connection))
            {
                command.Parameters.AddWithValue("@employmentId", employmentId);

                Connection.Open();
                object result = command.ExecuteScalar();
                Connection.Close();

                return Convert.ToInt32(result);
            }
        }

        public async Task DeleteArchiveData(int id)
        {
            string queryArchive = "DELETE FROM Архив WHERE ID_безработного = @id";
            string queryEmployment = "DELETE FROM Трудоустройство WHERE ID_безработного = @id";

            using (OleDbCommand commandArchive = new OleDbCommand(queryArchive, Connection))
            using (OleDbCommand commandEmployment = new OleDbCommand(queryEmployment, Connection))
            {
                commandArchive.Parameters.AddWithValue("@id", id);
                commandEmployment.Parameters.AddWithValue("@id", id);

                await Connection.OpenAsync();
                await commandArchive.ExecuteNonQueryAsync();
                await commandEmployment.ExecuteNonQueryAsync();
                Connection.Close();
            }
        }



    }
}
