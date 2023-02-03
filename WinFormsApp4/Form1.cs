namespace WinFormsApp4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            // Check if the user is logged in
            
                // Show the login form if the user is not logged in

                LoginForm loginform = new  LoginForm();
                loginform.ShowDialog();
            Console.WriteLine("Allowed");
                
                // Check if the user has logged in successfully
                if (!IsUserLoggedIn())
                {
                    // Close the application if the user has not logged in successfully
                    Close();
                    return;
                }

            string username = Properties.Settings.Default.Username;

            LoadList(listBox1, username + "_todo.txt");
            LoadList(listBox2, username + "_inprogress.txt");
            LoadList(listBox3, username + "_done.txt");
        }

        private void SaveList(ListBox listBox, string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                foreach (var item in listBox.Items)
                {
                    writer.WriteLine(item.ToString());
                }
            }
        }
        private void LoadList(ListBox listBox, string filename)
        {
            if (!File.Exists(filename))
            {
                using (FileStream fs = File.Create(filename)) { }
            }

            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(fs))
            {
                listBox.Items.Clear();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    listBox.Items.Add(line);
                }
            }
        }
        private string GetUserFileName()
        {
            // Get the current logged in user's username
            string username = Properties.Settings.Default.Username;

            // Return the file name for the user's to-do list
            return $"{username}.txt";
        }

        private bool IsUserLoggedIn()
        {
            // Check if the user's username is stored in the application settings
            return !string.IsNullOrEmpty(Properties.Settings.Default.Username);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string newTask = Microsoft.VisualBasic.Interaction.InputBox("Enter a new task", "Add task", "");
            listBox1.Items.Add(newTask);
            string filename = Properties.Settings.Default.Username + "_todo.txt";
            SaveList(listBox1, filename);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex != -1)
            {
                listBox3.Items.RemoveAt(listBox3.SelectedIndex);
                SaveList(listBox3, GetUserFileName());
            }
        }

        private void listBox1_MouseDown_1(object sender, MouseEventArgs e)
        {
            int index = listBox1.IndexFromPoint(e.X, e.Y);
            if (index != -1)
            {
                Size dragSize = SystemInformation.DragSize;
                Console.WriteLine(dragSize);
                Rectangle dragRect = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                    e.Y - (dragSize.Height / 2)), dragSize);
                Console.WriteLine($"{e.X} {e.Y}");
                if (!dragRect.Contains(e.X, e.Y))
                {
                    Console.WriteLine(dragSize+"second");
                    listBox1.DoDragDrop(listBox1.Items[index], DragDropEffects.Move);
                }
            }
        }
        private void listBox2_MouseDown(object sender, MouseEventArgs e)
        {
            int index = listBox2.IndexFromPoint(e.X, e.Y);
            if (index != -1)
            {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                    e.Y - (dragSize.Height / 2)), dragSize);

                if (!dragRect.Contains(e.X, e.Y))
                {
                    listBox2.DoDragDrop(listBox2.Items[index], DragDropEffects.Move);
                }
            }
        }

        private void listBox3_MouseDown(object sender, MouseEventArgs e)
        {
            int index = listBox3.IndexFromPoint(e.X, e.Y);
            if (index != -1)
            {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                    e.Y - (dragSize.Height / 2)), dragSize);

                if (!dragRect.Contains(e.X, e.Y))
                {
                    listBox3.DoDragDrop(listBox1.Items[index], DragDropEffects.Move);
                }
            }
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {


            string task = e.Data.GetData(DataFormats.StringFormat).ToString();
            listBox1.Items.Add(task);
            string filename = Properties.Settings.Default.Username + "_todo.txt";
            SaveList(listBox1, filename);
            Console.WriteLine("listBox1_DragDrop, savelist action");
        }
        private void listBox2_DragDrop_1(object sender, DragEventArgs e)
        {


            string task = e.Data.GetData(DataFormats.StringFormat).ToString();
            listBox2.Items.Add(task);
            string filename = Properties.Settings.Default.Username + "_inprogress.txt";
            SaveList(listBox2, filename);
            Console.WriteLine("listBox2_DragDrop, savelist action");

        }

        private void listBox3_DragDrop_1(object sender, DragEventArgs e)
        {


            string task = e.Data.GetData(DataFormats.StringFormat).ToString();
            listBox3.Items.Add(task);
            string filename = Properties.Settings.Default.Username + "_done.txt";
            SaveList(listBox3, filename);

        }

        private void listBox1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void listBox2_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void listBox3_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.StringFormat))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
    }
}
