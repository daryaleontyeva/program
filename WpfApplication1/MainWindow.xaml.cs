using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml.Serialization;

namespace WpfApplication1
{
    public partial class MainWindow : Window
    {
        LicenseForStudentCard license = new LicenseForStudentCard();
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                LicenseForStudentCard license = new LicenseForStudentCard();
                license.Verify();
            }
            catch (ApplicationException e)
            {
                MessageBox.Show(e.Message);
                Close();
            }
        }
        [XmlInclude(typeof(DataBaseForStudents))]
        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            DataBaseForStudents d1 = new DataBaseForStudents(textBox_FIO.Text, textBox_Age.Text, comboBox_fak.Text, comboBox_direction.Text, Convert.ToInt32(textBox_course.Text),  comboBox_expirience.Text);
            XmlSerializer formatter = new XmlSerializer(typeof(DataBaseForStudents));
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".xml";
            dlg.Filter = "Студенты|*.xml";
            dlg.FileName = "Студент №";
            dlg.ShowDialog();
            Stream myStream = dlg.OpenFile();
            formatter.Serialize(myStream, d1);
            myStream.Close();
        }

        private void buttonOpen_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openSaveFileDialog = new Microsoft.Win32.OpenFileDialog();
            XmlSerializer formatter = new XmlSerializer(typeof(DataBaseForStudents));
            Stream myStream = null;
            openSaveFileDialog.DefaultExt = ".xml"; 
            openSaveFileDialog.Filter = "Студенты|*.xml";
            openSaveFileDialog.ShowDialog();
            myStream = openSaveFileDialog.OpenFile();
            DataBaseForStudents d2 = (DataBaseForStudents)formatter.Deserialize(myStream);
            textBox_FIO.Text = d2.fio;
            textBox_Age.Text = d2.age;
            comboBox_fak.Text = d2.fak;
            comboBox_direction.Text = d2.direction;
            textBox_course.Text = Convert.ToString(d2.course);
            comboBox_expirience.Text = d2.expirience;
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           MessageBox.Show("Добро пожаловать!");
        }
    }
}
