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
            // Check if the user file exists
            string filename = $"{username}_user.txt";
            if (!File.Exists(filename))
            {
                return false;
            }

            // Read the user file and compare the password
            using (StreamReader reader = new StreamReader(filename))
            {
                string storedPassword = reader.ReadLine();
                return password == storedPassword;
            }
        }



        private void loginButton_Click_1(object sender, EventArgs e)
        {
            // Get the entered username and password
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;
            Console.WriteLine(username + ":" + password);
            // Check if the username and password are correct
            if (IsValidUser(username, password))
            {
                // Store the username in the application settings
                Properties.Settings.Default.Username = username;
                Properties.Settings.Default.Save();

                // Close the login form
                Close();
            }
            else
            {
                // Show an error message if the username and password are incorrect
                MessageBox.Show("Incorrect username or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void registerButton_Click_1(object sender, EventArgs e)
        {
            // Get the entered username and password
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            // Check if the username is already taken
            string filename = $"{username}_user.txt";
            if (File.Exists(filename))
            {
                MessageBox.Show("Username already taken", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Create the user file
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.WriteLine(password);
            }

            // Show a success message
            MessageBox.Show("Registration successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}