using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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

namespace CSConfluenceAutomationFWWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string APPSETTINGS_JELSZO = ConfigurationManager.AppSettings["Jelszo"];
        public string APPSETTINGS_FELHASZNALONEV = ConfigurationManager.AppSettings["FelhasznaloNev"];
        public string APPSETTINGS_URL = ConfigurationManager.AppSettings["URL"];
        public string APPSETTINGS_TERAZONOSITO = ConfigurationManager.AppSettings["TerAzonosito"];
        public string APPSETTINGS_SZULOOSZTALYAZONOSITO = ConfigurationManager.AppSettings["SzuloOsztalyAzonosito"];
        public string APPSETTINGS_OLDALCIM = ConfigurationManager.AppSettings["OldalCim"];
        public string APPSETTINGS_OLDALAZONOSITO = ConfigurationManager.AppSettings["OldalAzonosito"];
        public MainWindow()
        {
            InitializeComponent();
        }
        /*-
        private string btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            string html = "";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "html files (*.html)|*.html";
            openFileDialog.InitialDirectory = @"d:\";

            if (openFileDialog.ShowDialog() == true)
            {
                html = File.ReadAllText(openFileDialog.FileName);
            }

            return html;
        }
        */
        private void UploadClick(object sender, RoutedEventArgs e)
        {
            Metodus metodus = new Metodus();

            string html = "";

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "html files (*.html)|*.html";
            openFileDialog.InitialDirectory = @"d:\";

            if (openFileDialog.ShowDialog() == true)
            {
                html = File.ReadAllText(openFileDialog.FileName);
            }

            Regex re = new Regex("\r(?= *\r)");

            html = html.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("\"", "'");

            metodus.AddConfluencePage(
                APPSETTINGS_OLDALCIM
                , APPSETTINGS_TERAZONOSITO
                , APPSETTINGS_SZULOOSZTALYAZONOSITO
                , html
                , APPSETTINGS_URL
                , APPSETTINGS_FELHASZNALONEV
                , APPSETTINGS_JELSZO
                );
        }

        public void UploadImage(object sender, RoutedEventArgs e)
        {
            Metodus metodus = new Metodus();

            string fajlNev = "";
            ByteArrayContent kepByteTomb = null;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "image files (*.jpeg)|*.jpeg|(*.jpg)|*.jpg|(*.png)|*.png";
            openFileDialog.InitialDirectory = @"d:\";

            if (openFileDialog.ShowDialog() == true)
            {
                kepByteTomb = new ByteArrayContent(File.ReadAllBytes(openFileDialog.FileName));
                fajlNev = System.IO.Path.GetFileName(openFileDialog.FileName);
            }

            metodus.ConvertFromCURL(
                APPSETTINGS_FELHASZNALONEV
                , APPSETTINGS_JELSZO
                , APPSETTINGS_URL
                , APPSETTINGS_OLDALAZONOSITO
                , kepByteTomb
                , fajlNev
                );
        }
       
    }
}
