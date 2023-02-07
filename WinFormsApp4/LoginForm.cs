namespace WinFormsApp4
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }


        private bool IsValidUser(string username, string password)
        {
            // Проверка если файл пользователя отсутсвует
            string filename = $"{username}_user.txt";
            if (!File.Exists(filename))
            {
                return false;
            }

            // Проверка пароля
            using (StreamReader reader = new StreamReader(filename))
            {
                string storedPassword = reader.ReadLine();
                return password == storedPassword;
            }
        }



        private void loginButton_Click_1(object sender, EventArgs e)
        {
            // Принимает значение логина и пароля из соответсвутющих textbox
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;
            Console.WriteLine(username + ":" + password);
            // Проверка на корретные логин и пароль
            if (IsValidUser(username, password))
            {
                // Записывает имя пользователя в настройки приложения
                Properties.Settings.Default.Username = username;
                Properties.Settings.Default.Save();

                // Закрыть форму логона
                Close();
            }
            else
            {
                // Сообщение об ошибке 
                MessageBox.Show("Неправильное имя пользователя или пароль", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void registerButton_Click_1(object sender, EventArgs e)
        {
            // Принимает значение логина и пароля из соответсвутющих textbox
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;
            //логин или пароль не могут быть пустыми
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Логин или пароль не могут быть пустыми", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Проверка на дублирование лоигна
            string filename = $"{username}_user.txt";
            if (File.Exists(filename))
            {
                MessageBox.Show("Имя уже используется", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Создание пользовательского фалйа
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine(password);
            }

            // Показ окна успешной регистрации
            MessageBox.Show("Регистрация завершена", "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}