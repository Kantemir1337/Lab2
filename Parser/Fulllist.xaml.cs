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
using System.Windows.Shapes;
using System.ComponentModel;
using System.IO;

namespace Parser
{
    /// <summary>
    /// Interaction logic for Fulllist.xaml
    /// </summary>
    public partial class Fulllist : Window
    {
        private MainWindow m_parent;
        private readonly PagingCollectionView _cview;
        public Fulllist()
        {
            InitializeComponent();
            this._cview = new PagingCollectionView(Parser.MainWindow.threats, 15);
            this.DataContext = this._cview;
        }
        public Fulllist(MainWindow parent) : this()
        {
            m_parent = parent;
        }
        private void Fulllist_Closing(object sender, CancelEventArgs e)
        {
            m_parent.Show();
        }
        private void Back(object sender, RoutedEventArgs e)
        {
            this.Close();
            m_parent.Show();
        }
        private void OnNextClicked(object sender, RoutedEventArgs e)
        {
            this._cview.MoveToNextPage();
        }
        private void OnPreviousClicked(object sender, RoutedEventArgs e)
        {
            this._cview.MoveToPreviousPage();
        }
        private void FileSave(object sender, RoutedEventArgs e)
        {
            try
            {
                string allText = "";
                foreach (var danger in Parser.MainWindow.threats)
                {
                    allText += "ID: " + danger.ID +
                            " Name: " + danger.Name.Replace("\r", "").Replace("\n", "") +
                            " Description: " + danger.Description.Replace("\r", "").Replace("\n", "") +
                            " Source: " + danger.Source.Replace("\r", "").Replace("\n", "") +
                            " Object: " + danger.Object.Replace("\r", "").Replace("\n", "") +
                            " Confidentity breach: " + (bool)danger.ConfBreach +
                            " Integrity breach: " + danger.IntegrBreach +
                            " Availability breach: " + danger.AvailabBreach +
                            "\n";
                }
                File.WriteAllText(Directory.GetCurrentDirectory() + "\\localFullBase.txt", allText);
                MessageBox.Show($"Файлик сохранился тут:\n{Directory.GetCurrentDirectory()}\\localFullBase.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
