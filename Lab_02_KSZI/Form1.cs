using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_02_KSZI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var usbDeviceList = USBManager.GetListUsbDevicesId();

            foreach (var item in usbDeviceList)
            {
                comboBox1.Items.Add(item);
            }

            foreach (var item in usbDeviceList)
            {
                comboBox2.Items.Add(item);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var folderPath = String.Empty;

            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    textBox1.Text = fbd.SelectedPath;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var folderPath = String.Empty;

            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    textBox2.Text = fbd.SelectedPath;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != String.Empty && comboBox1.GetItemText(comboBox1.SelectedItem) != String.Empty)
            {
                USBManager.Lock(textBox1.Text);
                File.AppendAllText("config.txt", $"{textBox1.Text}-{comboBox1.GetItemText(comboBox1.SelectedItem)}-");

                MessageBox.Show(
                        "Папка успешно заблокирована",
                        "Блокировка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                return;
            }

            MessageBox.Show(
                        "Не выбрана папка либо не выбрано USB-устройство",
                        "Блокировка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != String.Empty && comboBox2.GetItemText(comboBox2.SelectedItem) != String.Empty)
            {
                var listConfig = File.ReadAllText(@"config.txt").Replace("\n", " ");

                if (listConfig.Contains(textBox2.Text+"-"+comboBox2.GetItemText(comboBox2.SelectedItem)))
                {
                    USBManager.Unlock(textBox2.Text);

                    MessageBox.Show(
                        "Папка успешно разблокирована",
                        "Разблокировка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }

                MessageBox.Show(
                        "Неправильно выбрана папка либо USB-устройство",
                        "Разблокировка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
            }
        }
    }
}
