using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace LinkedInTester
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window, INotifyPropertyChanged
    {
        private string _name = "";
        private string _position = "";
        private bool? _validated = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string PersonName { get { return _name; }            set { _name = value; NotifyPropertyChanged(); } }
        public string PersonPosition { get { return _position; }    set { _position = value; NotifyPropertyChanged(); } }
        public bool? PersonValidated { get { return _validated; }   set { _validated = value; NotifyPropertyChanged(); } }

        private EditWindow()
        {
            InitializeComponent();
        }
        public EditWindow(Window parent, Person person)
        {
            InitializeComponent();
            this.Owner = parent;
            this.DataContext = this;
            PersonName = person.Name;
            PersonPosition = person.Position;
            PersonValidated = person.Validated;
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
