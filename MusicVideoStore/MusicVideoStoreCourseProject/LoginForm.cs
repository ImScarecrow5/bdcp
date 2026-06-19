using System;
using System.Drawing;
using System.Windows.Forms;

namespace MusicVideoStoreCourseProject
{
    public partial class LoginForm : Form
    {
        public string UserRole { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
        }

        private void EnterButton_Click(object sender, EventArgs e)
        {
            string login = loginTextBox.Text.Trim().ToLowerInvariant();
            string password = passwordTextBox.Text.Trim();

            if (login == "admin" && password == "admin")
            {
                UserRole = "Admin";
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            if (login == "manager" && password == "manager")
            {
                UserRole = "Manager";
                DialogResult = DialogResult.OK;
                Close();
                return;
            }

            messageLabel.Text = "Неверный логин или пароль.";
            messageLabel.ForeColor = Color.Firebrick;
        }
    }
}
