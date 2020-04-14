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

namespace Parser
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ShowChanges : Window
    {
        private MainWindow m_parent;
        private readonly PagingCollectionView _cview;
        public ShowChanges()
        {
            InitializeComponent();
            this._cview = new PagingCollectionView(Parser.MainWindow.changes, 15);
            this.DataContext = this._cview;
        }
        public ShowChanges(MainWindow parent) : this()
        {
            m_parent = parent;
        }
        private void Changes_Closing(object sender, CancelEventArgs e)
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
    }
}
