using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
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
        private static readonly log4net.ILog _naplo = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string APPSETTINGS_JELSZO = ConfigurationManager.AppSettings["Jelszo"];
        public string APPSETTINGS_FELHASZNALONEV = ConfigurationManager.AppSettings["FelhasznaloNev"];
        public string APPSETTINGS_URL = ConfigurationManager.AppSettings["URL"];
        public string APPSETTINGS_TERAZONOSITO = ConfigurationManager.AppSettings["TerAzonosito"];
        public string APPSETTINGS_SZULOOSZTALYNEVE = ConfigurationManager.AppSettings["SzuloOsztalyNeve"];
        public string APPSETTINGS_OLDALCIM = ConfigurationManager.AppSettings["OldalCim"];
        public string APPSETTINGS_OLDALNEVE = ConfigurationManager.AppSettings["OldalNeve"];
        public MainWindow()
        {
            InitializeComponent();
            uiPageName.Text = APPSETTINGS_OLDALNEVE;
            uiParentPageName.Text = APPSETTINGS_SZULOOSZTALYNEVE;
            uiSpaceKey.Text = APPSETTINGS_TERAZONOSITO;
        }
        
        private void UploadClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Metodus metodus = new Metodus();

                string html = "";

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "html files (*.html)|*.html";
                openFileDialog.InitialDirectory = @"c:\";

                if (openFileDialog.ShowDialog() == true)
                {
                    html = File.ReadAllText(openFileDialog.FileName);
                }

                Regex re = new Regex("\r(?= *\r)");

                html = html.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("\"", "'");

                var response = metodus.AddConfluencePage(
                    uiPageName.Text
                    , uiSpaceKey.Text
                    , uiParentPageName.Text
                    , html
                    , APPSETTINGS_URL
                    , APPSETTINGS_FELHASZNALONEV
                    , APPSETTINGS_JELSZO
                    );

                Response JSONObj = new Response();
                JSONObj = JsonConvert.DeserializeObject<Response>(response);
                if (JSONObj.statusCode == null)
                {
                    MessageBox.Show("Sikeres feltöltés!");
                }
                else
                {
                    MessageBox.Show("Hiba!\n\n" + JSONObj.statusCode + "\n" + JSONObj.message);
                }
            }catch(Exception exception)
            {
                _naplo.Error(exception.StackTrace);
            }
        }

        public async void UploadImage(object sender, RoutedEventArgs e)
        {
            try
            {
                Metodus metodus = new Metodus();

                string fajlNev = "";
                ByteArrayContent kepByteTomb = null;

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "image files (*.jpeg)|*.jpeg|(*.jpg)|*.jpg|(*.png)|*.png";
                openFileDialog.InitialDirectory = @"c:\";

                if (openFileDialog.ShowDialog() == true)
                {
                    kepByteTomb = new ByteArrayContent(File.ReadAllBytes(openFileDialog.FileName));
                    fajlNev = System.IO.Path.GetFileName(openFileDialog.FileName);
                }

                string response = await metodus.KepFeltoltes(
                    APPSETTINGS_FELHASZNALONEV
                    , APPSETTINGS_JELSZO
                    , APPSETTINGS_TERAZONOSITO
                    , APPSETTINGS_URL
                    , uiPageName.Text
                    , kepByteTomb
                    , fajlNev
                    );

                Response JSONObj = new Response();
                JSONObj = JsonConvert.DeserializeObject<Response>(response);
                if (JSONObj.statusCode == null)
                {
                    MessageBox.Show("Sikeres feltöltés!");
                }
                else
                {
                    MessageBox.Show("Hiba!\n\n" + JSONObj.statusCode + "\n" + JSONObj.message);
                }
            }catch(Exception exception)
            {
                _naplo.Error(exception.StackTrace);
            }
        }
    }
       
    
}
