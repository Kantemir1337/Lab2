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
using System.Net;
using System.IO;
using Microsoft.Win32;
using Excel = Microsoft.Office.Interop.Excel;

namespace Parser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Dictionary<int, string> objChange = new Dictionary<int, string>
        {
            [2] = "Наименование УБИ: ",
            [3] = "Описание: ",
            [4] = "Источник угрозы: ",
            [5] = "Объект воздействия: ",
            [6] = "Нарушение конфиденциальности: ",
            [7] = "Нарушение целостности: ",
            [8] = "Нарушение доступности: "
        };
        public static List<Risk> threats = new List<Risk>();
        public static List<Changes> changes = new List<Changes>();

        public MainWindow()
        {
            InitializeComponent();
            if (Directory.GetFiles(Directory.GetCurrentDirectory(), "thrlist.xlsx").Count() > 0)
            {
                MessageBox.Show("Необходимый файл находится на вашем компьютере.\nМожем продолжить работу...");
            }
            else
            {
                MessageBoxResult res = MessageBox.Show("Файл, необходимый для работы, отсутствует.\nСкачать файл?", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (res)
                {
                    case MessageBoxResult.None:
                        MessageBox.Show("Сорре бро, в другой раз(((");
                        Application.Current.Shutdown();
                        break;
                    case MessageBoxResult.Yes:
                        string a = Directory.GetCurrentDirectory();
                        using (var client = new WebClient())
                        {
                            client.DownloadFile("https://bdu.fstec.ru/files/documents/thrlist.xlsx", String.Concat(a, "\\thrlist.xlsx"));
                        }
                        MessageBox.Show("Файл успешно загружен. Идет запуск программы. \nПожалуйста подождите...");
                        break;
                    case MessageBoxResult.No:
                        MessageBox.Show("Что-то пошло не так...");
                        Application.Current.Shutdown();
                        break;
                    default:
                        break;
                }
            }
            File.Delete(Directory.GetCurrentDirectory() + "\\oldthrlist.xlsx");
            File.Copy(Directory.GetCurrentDirectory() + "\\thrlist.xlsx", Directory.GetCurrentDirectory() + "\\oldthrlist.xlsx");
            CreateCollection("\\oldthrlist.xlsx");
        }

        private void ShowShortList_Click(object sender, RoutedEventArgs e)

        {
            Shortlist shortlist = new Shortlist(this);
            shortlist.Show();
            this.Hide();
        }
        private void ShowFullList_Click(object sender, RoutedEventArgs e)

        {
            Fulllist fulllist = new Fulllist(this);
            fulllist.Show();
            this.Hide();
        }
        private void Upd_Click(object sender, RoutedEventArgs e)
        {
            using (var client = new WebClient())
            {
                try
                {
                    client.DownloadFile("https://bdu.fstec.ru/files/documents/thrlist.xlsx", String.Concat(Directory.GetCurrentDirectory(), "\\thrlist.xlsx"));
                    MessageBox.Show("Идет проверка актуальности файла.\nПожалуйста подождите...");
                    threats.Clear();
                    Excel.Application xlApp = new Excel.Application();
                    Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(Directory.GetCurrentDirectory() + "\\oldthrlist.xlsx");
                    Excel.Workbook xlWorkBook1 = xlApp.Workbooks.Open(Directory.GetCurrentDirectory() + "\\thrlist.xlsx");
                    Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                    Excel.Worksheet xlWorkSheet1 = (Excel.Worksheet)xlWorkBook1.Worksheets.get_Item(1);
                    for (int i = 3; i <= 219; i++)
                    {
                        for (int j = 2; j <= 8; j++)
                        {
                            if (((Excel.Range)xlWorkSheet1.Cells[i, j]).Value2.ToString() != ((Excel.Range)xlWorkSheet.Cells[i, j]).Value2.ToString())
                            {
                                changes.Add(new Changes()
                                {
                                    Id = ((Excel.Range)xlWorkSheet1.Cells[i, 1]).Value2.ToString(),
                                    Became = objChange[j] + ((Excel.Range)xlWorkSheet1.Cells[i, j]).Value2.ToString(),
                                    Was = objChange[j] + ((Excel.Range)xlWorkSheet.Cells[i, j]).Value2.ToString()
                                });
                            }
                        }
                    }
                    xlApp.Quit();
                    MessageBox.Show("Проверка завершена.");
                    File.Delete(Directory.GetCurrentDirectory() + "\\oldthrlist.xlsx");
                    File.Copy(Directory.GetCurrentDirectory() + "\\thrlist.xlsx", Directory.GetCurrentDirectory() + "\\oldthrlist.xlsx");
                    if (changes.Count > 0)
                    {
                        ShowChanges showChanges = new ShowChanges(this);
                        showChanges.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Изменений не обнаружено");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Что-то пошло не так...\nВозможная причина:" + ex.Message);
                }
                CreateCollection("\\thrlist.xlsx");
            }
        }

        public static void CreateCollection(string fileName)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(Directory.GetCurrentDirectory() + fileName);
            Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            //Excel.Range range = (Excel.Range)xlWorkSheet.get_Range(xlStart, "Z500");
            int j = 1;
            try
            {
                while (true)
                {
                    var s = ((Excel.Range)xlWorkSheet.Cells[j, 1]).Value2.ToString();
                    j++;
                }
            }
            catch (Exception) { }
            for (int i = 3; i < j; i++)
            {
                Risk threat = new Risk()
                {
                    ID = ((Excel.Range)xlWorkSheet.Cells[i, 1]).Value2.ToString(),
                    Name = ((Excel.Range)xlWorkSheet.Cells[i, 2]).Value2.ToString(),
                    Description = ((Excel.Range)xlWorkSheet.Cells[i, 3]).Value2.ToString(),
                    Source = ((Excel.Range)xlWorkSheet.Cells[i, 4]).Value2.ToString(),
                    Object = ((Excel.Range)xlWorkSheet.Cells[i, 5]).Value2.ToString(),
                    ConfBreach = ((Excel.Range)xlWorkSheet.Cells[i, 6]).Value2.ToString() == "1" ? true : false,
                    IntegrBreach = ((Excel.Range)xlWorkSheet.Cells[i, 7]).Value2.ToString() == "1" ? true : false,
                    AvailabBreach = ((Excel.Range)xlWorkSheet.Cells[i, 8]).Value2.ToString() == "1" ? true : false,
                    In = ((Excel.Range)xlWorkSheet.Cells[i, 9]).Value2.ToString(),
                    Change = ((Excel.Range)xlWorkSheet.Cells[i, 10]).Value2.ToString()
                };
                threats.Add(threat);
            }
            xlApp.Quit();
        }
    }
}
