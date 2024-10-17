using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using System.IO;
using Telegram.Bot;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        int i1 = 0, i2 = 0, i3 = 0, i4 = 0, i5 = 0;
        void list()
        {
            StreamReader sr = File.OpenText("tasks.txt");
            while (!sr.EndOfStream)
            {
                string str = sr.ReadLine();
                tasks.Add(str.Split('~'));
            }
        }
        void sr_date(System.DateTime date)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                string[] data = tasks[i];
                if ($"{data[3]}" == date.ToLongDateString())
                {
                    int o = 0;
                    for (int j = 0; j < email.Count; j++)
                    {
                        if (email[j] != data[4])
                        {
                            o++;
                        }
                        else
                            data1[j] += $"\n{data[2]}";
                    }
                    if (o == email.Count)
                    {
                        email.Add(data[4]);
                        data1.Add($"{data[0]} {data[1]}\n{data[4]} \n{data[2]}");
                    }
                }
            }
        }

        List<string[]> contacts = new List<string[]>();
        List<string[]> tasks = new List<string[]>();
        List<string> email = new List<string>();
        List<string> data1 = new List<string>();

        string api = "7807553130:AAFxO9VmK4VBHHxI4j7qC2DNxH7BmnEk9Ig";
        string chatId = "1424710372";
        void close()
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            i1 = i2 = i3 = i4 = i5 = 0;
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
                panel1.Visible = false;
                panel2.Visible = false;
                panel3.Visible = false;
                panel4.Visible = false;
                panel5.Visible = false;
                dateTimePicker2.Format = DateTimePickerFormat.Time;
                dateTimePicker2.ValueChanged += dateTimePicker2_ValueChanged;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (i1 % 2== 0)
            {
                panel1.Visible = true;
                panel2.Visible = false;
                panel3.Visible = false;
                panel4.Visible = false;
                panel5.Visible = false;
                i1++;
                i2 = i3 = i4 = i5 = 0;
            }
            else close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StreamReader sr = File.OpenText("contacts.txt");
            while (!sr.EndOfStream)
            {
                string str = sr.ReadLine();
                contacts.Add(str.Split(' '));
            }
            comboBox1.Items.Clear();
            for (int i = 0; i < contacts.Count; i++)
            {
                string[] data = contacts[i];
                comboBox1.Items.Add($"{data[0]} {data[1]}");
            }
            if (i2 % 2 == 0)
            {
                panel1.Visible = false;
                panel2.Visible = true;
                panel3.Visible = false;
                panel4.Visible = false;
                panel5.Visible = false;
                i2++;
                i1 = i3 = i4 = i5 = 0;
            }
            else close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (i3 % 2 == 0)
            {
                panel1.Visible = false;
                panel2.Visible = false;
                panel3.Visible = true;
                panel4.Visible = false;
                panel5.Visible = false;
                i3++;
                i2 = i1 = i4 = i5 = 0;

            }
            else close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (i4 % 2 == 0)
            {
                panel1.Visible = false;
                panel2.Visible = false;
                panel3.Visible = false;
                panel4.Visible = true;
                panel5.Visible = false;
                i4++;
                i2 = i3 = i1 = i5 = 0;

            }
            else close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox4.Text != "" && textBox5.Text != "")
            {
                if (textBox4.Text.Length > 7)
                {
                    StreamWriter sw = File.AppendText("contacts.txt");
                    sw.WriteLine($"{textBox1.Text} {textBox2.Text} {textBox3.Text} {textBox4.Text} {textBox5.Text}");
                    sw.Close();
                    MessageBox.Show("Данные успешно добавлены");
                    textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = "";
                }
                else MessageBox.Show("Пароль не меньше 8 символов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else MessageBox.Show("Сначала заполните все поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async void button10_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "" && textBox6.Text != "")
            {
                var bot = new TelegramBotClient(api);
                string[] data = contacts[comboBox1.SelectedIndex];
                await bot.SendTextMessageAsync(chatId, $"{data[0]} {data[1]} вам необходимо {textBox6.Text} до {dateTimePicker1.Text} {dateTimePicker2.Text}");
                StreamWriter sw = File.AppendText("tasks.txt");
                sw.WriteLine($"{data[0]}~{data[1]}~{textBox6.Text}~{dateTimePicker1.Text}~{data[4]}");
                sw.Close();
                textBox6.Text = "";
                comboBox1.Text = "";
                MessageBox.Show("Сообщение успешно отправлено");
            }
            else MessageBox.Show("Сначала заполните все поля", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private async void button11_Click(object sender, EventArgs e)
        {
            list();
            var bot = new TelegramBotClient(api);
            await bot.SendTextMessageAsync(chatId, $"Задачи на сегодня");
            System.DateTime date = System.DateTime.Now;
            sr_date(date);
            for(int i = 0; i < data1.Count; i++)
            {
                await bot.SendTextMessageAsync(chatId, data1[i]);
            }
        }

        private async void button12_Click(object sender, EventArgs e)
        {
            list();
            var bot = new TelegramBotClient(api);
            await bot.SendTextMessageAsync(chatId, $"Задачи на завтра");
            System.DateTime date = System.DateTime.Today.AddDays(1);
            sr_date(date);
            for (int i = 0; i < data1.Count; i++)
            {
                await bot.SendTextMessageAsync(chatId, data1[i]);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (i5 % 2 == 0)
            {
                panel1.Visible = false;
                panel2.Visible = false;
                panel3.Visible = false;
                panel4.Visible = false;
                panel5.Visible = true;
                i5++;
                i2 = i3 = i4 = i1 = 0;
            }
            else close();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            panel6.BackColor = Color.Aqua;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            panel6.BackColor = Color.Red;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            panel6.BackColor = Color.White;
        }
    }
}
