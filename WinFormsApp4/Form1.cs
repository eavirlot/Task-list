using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WinFormsApp4
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Функция проверки залогинен ли пользователь
        private bool IsUserLoggedIn()
        {
            // Проверяем, есть ли имя пользователя в настройках приложения
            return !string.IsNullOrEmpty(Properties.Settings.Default.Username);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
                // Проверяем, залогинен ли пользователь

                // Показываем форму входа, если пользователь не залогинен

                LoginForm loginform = new LoginForm();
                loginform.ShowDialog();
                Console.WriteLine("Allowed");

                // Проверяем, выполнил ли пользователь успешный вход
                if (!IsUserLoggedIn())
                {
                    // Закрываем приложение, если пользователь не выполнил успешный вход
                    Close();
                    return;
                }

                string username = Properties.Settings.Default.Username;
                
                //Загружаем список задач из файла
                LoadList(listBox1, username + "_todo.txt");
                LoadList(listBox2, username + "_inprogress.txt");
                LoadList(listBox3, username + "_done.txt");
            
        }
        // Функция сохранения списка в файл
        private void SaveList(ListBox listBox, string filename)
        {
            // Используем using, чтобы автоматически закрыть файл после записи
            using (StreamWriter writer = new StreamWriter(filename))
            {
                // Цикл по всем элементам в указанном списке
                foreach (var item in listBox.Items)
                {
                    // Запись строки в файл
                    writer.WriteLine(item.ToString());
                    // Вызов функции UpdateListboxCount для обновления счетчиков элементов в списках
                    UpdateListboxCount(listBox1, label4);
                    UpdateListboxCount(listBox2, label5);
                    UpdateListboxCount(listBox3, label6);
                }
            }
        }
        // Функция загрузки списка из файла
        private void LoadList(ListBox listBox, string filename)
        {
            // Проверка существования файла
            if (!File.Exists(filename))
            {
                // Создание пустого файла
                using (FileStream fs = File.Create(filename)) { }
            }

            // Открытие файла для чтения с помощью блока using, чтобы автоматически закрыть файл после чтения
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(fs))
            {
                // Очистка списка
                listBox.Items.Clear();
                // Считывание построчно
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // Добавление строки в список
                    listBox.Items.Add(line);
                    // Вызов функции UpdateListboxCount для обновления счетчика элементов в ListBox
                    UpdateListboxCount(listBox1, label4);
                    UpdateListboxCount(listBox2, label5);
                    UpdateListboxCount(listBox3, label6);
                }
            }
        }
        // Функция, возвращающая список, из которого был удален элемент
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

        // Функция, возвращающая имя файла пользователя
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

        // Обработчик события нажатия кнопки "Добавить задачу"
        private void button1_Click(object sender, EventArgs e)
        {
            // Запрос новой задачи от пользователя
            string newTask = Microsoft.VisualBasic.Interaction.InputBox("Введите имя новой задачи", "Добавить заачу", "");
            if (string.IsNullOrEmpty(newTask))
            {
                MessageBox.Show("Не введено название задачи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            listBox1.Items.Add(newTask);
            string filename = Properties.Settings.Default.Username + "_todo.txt";

            // Сохранение списка
            SaveList(listBox1, filename);
        }

        // Обработчик события нажатия кнопки "Удалить", удаляет только из listBox3
        private void button2_Click(object sender, EventArgs e)
        {
            // Удаление элемента из ListBox
            if (listBox3.SelectedIndex != -1)
            {
                listBox3.Items.RemoveAt(listBox3.SelectedIndex);
                // Сохранение списка
                SaveList(listBox3, GetUserFileName(listBox3));
            }
        }
       
        // Обработка события нажатия мыши на элемент списка
        private void listBox1_MouseDown_1(object sender, MouseEventArgs e)
        {
            // Получение индекса элемента в списке
            int index = listBox1.IndexFromPoint(e.X, e.Y);
            // Если элемент выбран, происходит начало перетаскивания
            if (index != -1)
            {
                Console.WriteLine("listBox1_MouseDown index ok");
                // Получение выбранного элемента
                string selectedItem = (string)listBox1.Items[index];
                // Начало перетаскивания
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

        // Обработка события входа в область перетаскивания
        private void listBox1_DragEnter_1(object sender, DragEventArgs e)
        {
            // Установка эффекта перетаскивания
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

        // Обработка события окончания перетаскивания
        private void listBox1_DragDrop_1(object sender, DragEventArgs e)
        {
            // Получение перетаскиваемого элемента
            string item = (string)e.Data.GetData(typeof(string));
            Console.WriteLine(item);
            // Если элемент перетаскиваемый
            if (e.Data.GetDataPresent(typeof(string)))
            {
                // Получение целевого списка
                ListBox target = (ListBox)sender;
                // Получение исходного списка
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
            label4.Text = "Количество задач: 0";
            listBox2.Items.Clear();
            label5.Text = "Количество задач: 0";
            listBox3.Items.Clear();
            label6.Text = "Количество задач: 0";
        }

        //Функция обновления счетчика заадач
        private void UpdateListboxCount(ListBox listbox, Label label)
        {
            int count = listbox.Items.Count;
            label.Text = "Количество задач: " + count.ToString();
        }

        //Обновляет счетчик при изменении listBox 
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
