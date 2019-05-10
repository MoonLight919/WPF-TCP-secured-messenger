using System;
using System.Collections;
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

namespace DialogUC
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public static readonly DependencyProperty MyItemsSourceProperty;
        public bool enable;
        public int marker;

        public UserControl1(string chatName)
        {
            InitializeComponent();

            enable = false;
            marker = 0;
            HideB.Content = chatName;
        }

        static UserControl1()
        {
            UserControl1.MyItemsSourceProperty = DependencyProperty.Register("MyItemsSource", typeof(IEnumerable), typeof(UserControl1));
        }

        public IEnumerable MyItemsSource
        {
            get
            {
                return (IEnumerable)GetValue(UserControl1.MyItemsSourceProperty);
            }
            set
            {
                SetValue(UserControl1.MyItemsSourceProperty, value);
            }
        }
        public void chatLb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(enable)
            {
                enable = false;
                chatLb.Background = Brushes.LightGray;
                marker = 0;
            }
            else
            {
                enable = true;
                chatLb.Background = Brushes.LightBlue;
                marker = 2;
            }
        }

        private void HideB_Click(object sender, RoutedEventArgs e)
        {
            if (chatLb.Visibility == Visibility.Collapsed)
                chatLb.Visibility = Visibility.Visible;
            else
                chatLb.Visibility = Visibility.Collapsed;
        }

        private void HideB_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
