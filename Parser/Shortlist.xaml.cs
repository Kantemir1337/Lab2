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
    /// Interaction logic for Shortlist.xaml
    /// </summary>
    public partial class Shortlist : Window
    {
        private MainWindow m_parent;
        private readonly PagingCollectionView _cview;
        public Shortlist()
        {
            InitializeComponent();
            this._cview = new PagingCollectionView(Parser.MainWindow.threats, 15);
            this.DataContext = this._cview;
        }
        public Shortlist(MainWindow parent) : this()
        {
            m_parent = parent;
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
                        " Name: " + danger.Name +
                        "\n";
                }
                File.WriteAllText(Directory.GetCurrentDirectory() + "\\localShortBase.txt", allText);
                MessageBox.Show($"Путь к файлу:\n{Directory.GetCurrentDirectory()}\\localShortBase.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.Close();
            m_parent.Show();
        }
        private void Shortlist_Closing(object sender, CancelEventArgs e)
        {
            m_parent.Show();
        }
    }
}
