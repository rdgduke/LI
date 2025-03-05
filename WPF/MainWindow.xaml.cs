using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Web.WebView2.Core;
using Xceed.Wpf.Toolkit;
using static System.Net.WebRequestMethods;

namespace LinkedInTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static char[] separators = { ',' };
        private Company? _company;
        public MainWindow()
        {
            InitializeComponent();
        }

        private string _filePath = "names.csv";

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Load data file
            _company = LoadFile(_filePath);
            data.ItemsSource = _company.People;
        }

        private string Escaped(string original)
        {
            return System.Uri.EscapeDataString(original);
        }

        private void Validate_Click(object sender, RoutedEventArgs e)
        {
            if (_company == null)
                return;

            // Get the selected item in the data grid.....
            Object testee = data.SelectedItem;
            if (testee != null)
            {
                Person p = (Person)testee;
                if (p != null)
                {
                    string req;
                    string? tag = "";
                    // Which type of check are we doing?
                    if (sender is SplitButton)
                    {
                        tag = ((SplitButton)sender).Tag as string;
                    }
                    else if (sender is Button)
                    {
                        tag = ((Button)sender).Tag as string;
                    }
                    switch (tag)
                    {
                        case "Name":
                            req = $"https://www.linkedin.com/search/results/people/?keywords=%22{Escaped(_company.Name)}%22%20%22{Escaped(p.Name)}%22&origin=SWITCH_SEARCH_VERTICAL&sid=f!Z";
                            break;
                        case "Position":
                            req = $"https://www.linkedin.com/search/results/people/?keywords=%22{Escaped(_company.Name)}%22%20%22{Escaped(p.Position)}%22&origin=SWITCH_SEARCH_VERTICAL&sid=f!Z";
                            break;
                        case "NameWeak":
                            req = $"https://www.linkedin.com/search/results/people/?keywords=%22{Escaped(_company.Name)}%22%20{Escaped(p.Name)}&origin=SWITCH_SEARCH_VERTICAL&sid=f!Z";
                            break;
                        default:
                            req = $"https://www.linkedin.com/search/results/people/?keywords=%22{Escaped(_company.Name)}%22%20%22{Escaped(p.Position)}%22%20%22{Escaped(p.Name)}%22&origin=SWITCH_SEARCH_VERTICAL&sid=f!Z";
                            break;
                    }
                    Uri uri = new Uri(req);
                    browser.Source = uri;
                }
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".csv"; // Default file extension
            dialog.Filter = "Comma Separated Lists(.csv)|*.csv"; // Filter files by extension
            dialog.Title = "Choose file to load";
            // Show open file dialog box
            bool? result = dialog.ShowDialog();
            if (result.HasValue && result.Value == true)
            {
                var company = LoadFile(dialog.FileName);
                if (company.Name != "Unknown")
                {
                    _company = company;
                    _filePath = dialog.FileName;
                    data.ItemsSource = _company.People;
                    data.Items.Refresh();
                }
            }

        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_company == null)
                return;
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = _filePath;
            dialog.DefaultExt = ".csv"; // Default file extension
            dialog.Filter = "Comma Separated Lists(.csv)|*.csv"; // Filter files by extension
            dialog.Title = "Choose file to save";
            // Show open file dialog box
            bool? result = dialog.ShowDialog();
            if (result.HasValue && result.Value == true)
            {
                _filePath = dialog.FileName;
                SaveFile();
            }


        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (_company == null)
                return;
            var person = new Person();
            var window = new EditWindow(this, person);
            window.Title = "New Person";
            var result = window.ShowDialog();
            if (result.HasValue && result.Value == true)
            {
                // Update record
                person.Name = window.PersonName;
                person.Position = window.PersonPosition;
                _company.People.Add(person);
                data.Items.Refresh();
                //data.ItemsSource = _company.People;
            }

        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (_company == null)
                return;
            var person = data.SelectedItem as Person;
            if (person != null)
            {
                var window = new EditWindow(this, person);
                
                var result = window.ShowDialog();
                if (result.HasValue && result.Value == true)
                {
                    // Update record
                    person.Name = window.PersonName;
                    person.Position = window.PersonPosition;
                    data.Items.Refresh();
                    //data.ItemsSource = _company.People;
                }
            }
        }

        private Company LoadFile(string filePath)
        {
            Company company = new Company();
            company.Name = "Unknown";

            // Load data file
            using (FileStream stream = new FileStream(filePath, FileMode.Open))
            using (StreamReader streamReader = new StreamReader(stream, new UTF8Encoding(false)))
            {
                // the first line contains a company name
                // the rest a person and a job:  PERSON,POSITION, VALIDATED
                string? line;

                line = streamReader.ReadLine();
                if (line != null)
                {
                    company.Name = line;
                    do
                    {
                        line = streamReader.ReadLine();
                        if (line != null)
                        {
                            var parts = line?.Split(separators, StringSplitOptions.TrimEntries);
                            if (parts != null)
                            {
                                Person p = new Person() { Name = parts[0] };
                                if (parts.Count() > 1)
                                {
                                    p.Position = parts[1];
                                }
                                if (parts.Count() > 2)
                                {
                                    bool check = false;
                                    Boolean.TryParse(parts[2], out check);
                                    p.Validated = check;
                                }
                                company.People.Add(p);
                            }
                        }
                    }
                    while (!string.IsNullOrEmpty(line));
                }
            }
            return company;
        }

        private void SaveFile()
        {
            if (_company == null)
                return;

            using (FileStream stream = new FileStream(_filePath, FileMode.Create))
            using (StreamWriter sw = new StreamWriter(stream, new UTF8Encoding(false)))
            {
                sw.WriteLine(_company.Name);
                foreach (var person in _company.People)
                {
                    sw.WriteLine(person);
                }
            }
        }
    }
}