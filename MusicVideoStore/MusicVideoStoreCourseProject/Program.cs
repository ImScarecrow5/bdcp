using System;
using System.Windows.Forms;
using MusicVideoStoreCourseProject.Services;

namespace MusicVideoStoreCourseProject
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                DatabaseInitializer.EnsureDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "База данных не была создана автоматически. Проверьте строку подключения в App.config или выполните Scripts\\CreateDatabase.sql вручную.\n\n" + ex.Message,
                    "Предупреждение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            using (LoginForm loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                    Application.Run(new MainForm(loginForm.UserRole));
            }
        }
    }
}
