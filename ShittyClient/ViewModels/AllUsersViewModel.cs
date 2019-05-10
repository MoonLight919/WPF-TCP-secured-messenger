using ClassesForNP;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Commands;

namespace WpfApp1.ViewModels
{
    public class AllUsersViewModel : INotifyPropertyChanged
    {
        MainViewModel vm;
        private AddUserToContactsCommand addContact;
        public BaseCommand AddContact
        {
            get
            {
                return addContact;
            }
        }
        public ObservableCollection<WUser> Contacts { get; set; }
        public event EventHandler ShowAllUsersWindwowRequested;

        public AllUsersViewModel(MainViewModel viewModel)
        {
            vm = viewModel;
            Contacts = new ObservableCollection<WUser>();
            addContact = new AddUserToContactsCommand(vm, this);
        }

        public void ShowWindow()
        {
            vm.GetAllContacts(Contacts);
            OnShowAllUsersWindwowRequested();
        }

        public void OnShowAllUsersWindwowRequested()
        {
            if (ShowAllUsersWindwowRequested != null)
                ShowAllUsersWindwowRequested(this, EventArgs.Empty);
            else
            {
                MessageBox.Show("If you suddenly get this message, please inform developer team!");
                throw new NotImplementedException("Not implemented event handler 'ShowAllUsersWindwowRequested'.");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
