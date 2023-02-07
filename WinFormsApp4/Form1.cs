using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WinFormsApp4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // ������� �������� ��������� �� ������������
        private bool IsUserLoggedIn()
        {
            // ���������, ���� �� ��� ������������ � ���������� ����������
            return !string.IsNullOrEmpty(Properties.Settings.Default.Username);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
                // ���������, ��������� �� ������������

                // ���������� ����� �����, ���� ������������ �� ���������

                LoginForm loginform = new LoginForm();
                loginform.ShowDialog();
                Console.WriteLine("Allowed");

                // ���������, �������� �� ������������ �������� ����
                if (!IsUserLoggedIn())
                {
                    // ��������� ����������, ���� ������������ �� �������� �������� ����
                    Close();
                    return;
                }

                string username = Properties.Settings.Default.Username;
                
                //��������� ������ ����� �� �����
                LoadList(listBox1, username + "_todo.txt");
                LoadList(listBox2, username + "_inprogress.txt");
                LoadList(listBox3, username + "_done.txt");
            
        }
        // ������� ���������� ������ � ����
        private void SaveList(ListBox listBox, string filename)
        {
            // ���������� using, ����� ������������� ������� ���� ����� ������
            using (StreamWriter writer = new StreamWriter(filename))
            {
                // ���� �� ���� ��������� � ��������� ������
                foreach (var item in listBox.Items)
                {
                    // ������ ������ � ����
                    writer.WriteLine(item.ToString());
                    // ����� ������� UpdateListboxCount ��� ���������� ��������� ��������� � �������
                    UpdateListboxCount(listBox1, label4);
                    UpdateListboxCount(listBox2, label5);
                    UpdateListboxCount(listBox3, label6);
                }
            }
        }
        // ������� �������� ������ �� �����
        private void LoadList(ListBox listBox, string filename)
        {
            // �������� ������������� �����
            if (!File.Exists(filename))
            {
                // �������� ������� �����
                using (FileStream fs = File.Create(filename)) { }
            }

            // �������� ����� ��� ������ � ������� ����� using, ����� ������������� ������� ���� ����� ������
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(fs))
            {
                // ������� ������
                listBox.Items.Clear();
                // ���������� ���������
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // ���������� ������ � ������
                    listBox.Items.Add(line);
                    // ����� ������� UpdateListboxCount ��� ���������� �������� ��������� � ListBox
                    UpdateListboxCount(listBox1, label4);
                    UpdateListboxCount(listBox2, label5);
                    UpdateListboxCount(listBox3, label6);
                }
            }
        }
        // �������, ������������ ������, �� �������� ��� ������ �������
        private ListBox GetSourceListBox(string item)
        {
            if (listBox1.Items.Contains(item))
                return listBox1;
            else if (listBox2.Items.Contains(item))
                return listBox2;
            else if (listBox3.Items.Contains(item))
                return listBox3;
            return null;
        }

        // �������, ������������ ��� ����� ������������
        private string GetUserFileName(ListBox listBox)
        {
            string username = Properties.Settings.Default.Username;
            if (listBox == listBox1)
                return username + "_todo.txt";
            else if (listBox == listBox2)
                return username + "_inprogress.txt";
            else if (listBox == listBox3)
                return username + "_done.txt";
            return "";
        }

        // ���������� ������� ������� ������ "�������� ������"
        private void button1_Click(object sender, EventArgs e)
        {
            // ������ ����� ������ �� ������������
            string newTask = Microsoft.VisualBasic.Interaction.InputBox("������� ��� ����� ������", "�������� �����", "");
            if (string.IsNullOrEmpty(newTask))
            {
                MessageBox.Show("�� ������� �������� ������", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            listBox1.Items.Add(newTask);
            string filename = Properties.Settings.Default.Username + "_todo.txt";

            // ���������� ������
            SaveList(listBox1, filename);
        }

        // ���������� ������� ������� ������ "�������", ������� ������ �� listBox3
        private void button2_Click(object sender, EventArgs e)
        {
            // �������� �������� �� ListBox
            if (listBox3.SelectedIndex != -1)
            {
                listBox3.Items.RemoveAt(listBox3.SelectedIndex);
                // ���������� ������
                SaveList(listBox3, GetUserFileName(listBox3));
            }
        }
       
        // ��������� ������� ������� ���� �� ������� ������
        private void listBox1_MouseDown_1(object sender, MouseEventArgs e)
        {
            // ��������� ������� �������� � ������
            int index = listBox1.IndexFromPoint(e.X, e.Y);
            // ���� ������� ������, ���������� ������ ��������������
            if (index != -1)
            {
                Console.WriteLine("listBox1_MouseDown index ok");
                // ��������� ���������� ��������
                string selectedItem = (string)listBox1.Items[index];
                // ������ ��������������
                listBox1.DoDragDrop(selectedItem, DragDropEffects.Move);
            }
        }

        private void listBox2_MouseDown_1(object sender, MouseEventArgs e)
        {
            int index = listBox2.IndexFromPoint(e.X, e.Y);
            if (index != -1)
            {
                Console.WriteLine("listBox2_MouseDown index ok");
                string selectedItem = (string)listBox2.Items[index];
                listBox2.DoDragDrop(selectedItem, DragDropEffects.Move);
            }
        }

        private void listBox3_MouseDown_1(object sender, MouseEventArgs e)
        {
            int index = listBox3.IndexFromPoint(e.X, e.Y);
            if (index != -1)
            {
                Console.WriteLine("listBox3_MouseDown index ok");
                string selectedItem = (string)listBox3.Items[index];
                listBox3.DoDragDrop(selectedItem, DragDropEffects.Move);
            }
        }

        // ��������� ������� ����� � ������� ��������������
        private void listBox1_DragEnter_1(object sender, DragEventArgs e)
        {
            // ��������� ������� ��������������
            e.Effect = DragDropEffects.Move;
        }

        private void listBox2_DragEnter_1(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void listBox3_DragEnter_1(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        // ��������� ������� ��������� ��������������
        private void listBox1_DragDrop_1(object sender, DragEventArgs e)
        {
            // ��������� ���������������� ��������
            string item = (string)e.Data.GetData(typeof(string));
            Console.WriteLine(item);
            // ���� ������� ���������������
            if (e.Data.GetDataPresent(typeof(string)))
            {
                // ��������� �������� ������
                ListBox target = (ListBox)sender;
                // ��������� ��������� ������
                ListBox source = GetSourceListBox(item);
                target.Items.Add(item);
                source.Items.Remove(item);
                SaveList(target, GetUserFileName(target));
                SaveList(source, GetUserFileName(source));
            }
        }

        private void listBox2_DragDrop_1(object sender, DragEventArgs e)
        {
            string item = (string)e.Data.GetData(typeof(string));
            Console.WriteLine(item);
            if (e.Data.GetDataPresent(typeof(string)))
            {
                ListBox target = (ListBox)sender;
                ListBox source = GetSourceListBox(item);
                target.Items.Add(item);
                source.Items.Remove(item);
                SaveList(target, GetUserFileName(target));
                SaveList(source, GetUserFileName(source));
            }
        }

        private void listBox3_DragDrop_1(object sender, DragEventArgs e)
        {
            string item = (string)e.Data.GetData(typeof(string));
            Console.WriteLine(item);
            if (e.Data.GetDataPresent(typeof(string)))
            {
                ListBox target = (ListBox)sender;
                ListBox source = GetSourceListBox(item);
                target.Items.Add(item);
                source.Items.Remove(item);
                SaveList(target, GetUserFileName(target));
                SaveList(source, GetUserFileName(source));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            label4.Text = "���������� �����: 0";
            listBox2.Items.Clear();
            label5.Text = "���������� �����: 0";
            listBox3.Items.Clear();
            label6.Text = "���������� �����: 0";
        }

        //������� ���������� �������� ������
        private void UpdateListboxCount(ListBox listbox, Label label)
        {
            int count = listbox.Items.Count;
            label.Text = "���������� �����: " + count.ToString();
        }

        //��������� ������� ��� ��������� listBox 
        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            UpdateListboxCount(listBox1, label4);
            UpdateListboxCount(listBox2, label5);
            UpdateListboxCount(listBox3, label6);
        }

        private void listBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            UpdateListboxCount(listBox1, label4);
            UpdateListboxCount(listBox2, label5);
            UpdateListboxCount(listBox3, label6);
        }

        private void listBox3_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            UpdateListboxCount(listBox1, label4);
            UpdateListboxCount(listBox2, label5);
            UpdateListboxCount(listBox3, label6);
        }
    }
    
}
