using ClassesForNP;
using DialogUC;
using Microsoft.Win32;
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
using WpfApp1.ViewModels;

namespace WpfApp1.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel vm;

        public MainWindow()
        {
            InitializeComponent();
            vm = new MainViewModel();
            DataContext = vm;
            contactsLb.ItemsSource = vm.Contacts;
            vm.PlayRequested += (sender, e) =>
            {
                if (vm.mediaPlayerIsPlaying)
                {
                    vm.mediaPlayerIsPlaying = false;
                    mePlayer.Pause();
                }
                else
                {
                    vm.mediaPlayerIsPlaying = true;
                    mePlayer.Play();
                }
            };

            vm.ChangePositionRequested += (sender, e) =>
            {
                mePlayer.Position = TimeSpan.FromSeconds(vm.PlayerSliderValue);
            };

            vm.FileOpenedRequested += (sender, e) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();

                openFileDialog.Multiselect = false;
                openFileDialog.Filter = "Media files (*.mp3;*.mpg;*.mpeg;*.wmv;*.mp4;*.avi)|*.mp3;*.mpg;*.mpeg;*.wmv;*.mp4;*.avi|All files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    mePlayer.Source = new Uri(openFileDialog.FileName);
                    vm.CurrentFile = openFileDialog.FileName;
                }
            };

            vm.allUsersViewModel.ShowAllUsersWindwowRequested += (o, e) =>
            {
                var win = new AllUsersWindow(vm.allUsersViewModel);
                win.Show();
            };

            vm.RefreshListBoxRequested += (o, e) =>
            {
            };

            vm.CreateChatRequested += (o, e) =>
            {
                string s = "";
                vm.Chats[vm.Chats.Count - 1].Members.ForEach(new Action<WTruUser>((x) => s += $"{x.NickName},"));
                s = s.Remove(s.Length - 1);
                UserControl1 chat = null;
                this.Dispatcher.Invoke(new Action(() => { chat = new UserControl1(s); }));
                vm.ChatControls.Add(chat);
                this.Dispatcher.Invoke(new Action(() => { chatStackPanel.Children.Add(chat); }));
                var collecton = new System.Collections.ObjectModel.ObservableCollection<ListBoxItem>();
                this.Dispatcher.Invoke(new Action(() => { chat.MyItemsSource = collecton; }));
                vm.SourcesOFChats.Add(collecton);
            };

            vm.TickEvent += (o, e) =>
            {
                if ((mePlayer.Source != null) && (mePlayer.NaturalDuration.HasTimeSpan) && (!vm.userIsDraggingSlider))
                {
                    sliProgress.Minimum = 0;
                    sliProgress.Maximum = mePlayer.NaturalDuration.TimeSpan.TotalSeconds;
                    sliProgress.Value = mePlayer.Position.TotalSeconds;
                }
            };
        }
    }
}

