using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmploymentAgency
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Создание нового экземпляра формы авторизации
            meetForm loginForm = new meetForm();

            // Показать форму авторизации как модальное диалоговое окно
            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                // Создание нового экземпляра главной формы
                Programm mainForm = new Programm();

                // Создание нового контекста приложения с главной формой
                ApplicationContext appContext = new ApplicationContext(mainForm);

                // Запуск приложения с контекстом приложения
                Application.Run(appContext);
            }
        }
    }
}
