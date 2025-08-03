using Gym.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;

namespace Gym
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly GymListManagementtContext _context = new();
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUserName.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                txtError.Text = "Vui lòng nhập tài khoản và mật khẩu.";
                return;
            }

            var user = _context.UserAccounts
                .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                MainWindow mainWindow = new();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                txtError.Text = "Tài khoản hoặc mật khẩu không đúng.";
            }
        }
    }
}
