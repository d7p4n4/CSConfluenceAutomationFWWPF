using CSConfluenceAutomationFWWPFLib;
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
        public int APPSETTINGS_IDHOSSZA = Convert.ToInt32(ConfigurationManager.AppSettings["IdHossza"]);
        public MainWindow()
        {
            InitializeComponent();
            uiPageName.Text = APPSETTINGS_OLDALNEVE;
            uiParentPageName.Text = APPSETTINGS_SZULOOSZTALYNEVE;
            uiSpaceKey.Text = APPSETTINGS_TERAZONOSITO;
        }
        /*
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
                if (uiFelhasznaloNev.Text.Equals("") || uiJelszo.Password.Equals(""))
                {
                    MessageBox.Show("Username and password can not be empty!");
                }
                else
                {

                    var response = metodus.AddConfluencePage(
                        uiPageName.Text
                        , uiSpaceKey.Text
                        , uiParentPageName.Text
                        , html
                        , APPSETTINGS_URL
                        , uiFelhasznaloNev.Text
                        , uiJelszo.Password
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
                }
            }catch(Exception exception)
            {
                _naplo.Error(exception.StackTrace);
            }
        }
        */
        private void UploadOrUpdatePage(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfluenceAPIMetodusok confluenceAPIMetodusok = new ConfluenceAPIMetodusok();

                string html = "";

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "html files (*.html)|*.html";
                openFileDialog.InitialDirectory = @"c:\";

                if (openFileDialog.ShowDialog() == true)
                {
                    html = File.ReadAllText(openFileDialog.FileName);
                }

                if (uiFelhasznaloNev.Text.Equals("") || uiJelszo.Password.Equals(""))
                {
                    MessageBox.Show("Username and password can not be empty!");
                }
                else
                {
                    var letezikAzOldal = confluenceAPIMetodusok.IsPageExists(
                    APPSETTINGS_URL
                    , uiPageName.Text
                    , uiSpaceKey.Text
                    , uiFelhasznaloNev.Text
                    , uiJelszo.Password
                    , APPSETTINGS_IDHOSSZA
                    );

                    if (letezikAzOldal)
                    {
                        if (uiVersionNumber.Text.Equals(""))
                        {
                            MessageBox.Show("Version number can not be empty!");
                        }
                        else
                        {

                            var response = confluenceAPIMetodusok.UpdateConfluencePage(
                                uiPageName.Text
                                , uiSpaceKey.Text
                                , html
                                , APPSETTINGS_URL
                                , uiFelhasznaloNev.Text
                                , uiJelszo.Password
                                , uiVersionNumber.Text
                                , APPSETTINGS_IDHOSSZA
                                );

                            ConfluenceAPIResponse JSONObj = new ConfluenceAPIResponse();
                            JSONObj = JsonConvert.DeserializeObject<ConfluenceAPIResponse>(response);
                            if (JSONObj.statusCode == null)
                            {
                                MessageBox.Show("Sikeres feltöltés!");
                            }
                            else
                            {
                                MessageBox.Show("Hiba!\n\n" + JSONObj.statusCode + "\n" + JSONObj.message);
                            }
                        }
                    }
                    else
                    {
                        var response = confluenceAPIMetodusok.AddConfluencePage(
                        uiPageName.Text
                        , uiSpaceKey.Text
                        , uiParentPageName.Text
                        , html
                        , APPSETTINGS_URL
                        , uiFelhasznaloNev.Text
                        , uiJelszo.Password
                        , APPSETTINGS_IDHOSSZA
                        );

                        ConfluenceAPIResponse JSONObj = new ConfluenceAPIResponse();
                        JSONObj = JsonConvert.DeserializeObject<ConfluenceAPIResponse>(response);
                        if (JSONObj.statusCode == null)
                        {
                            MessageBox.Show("Sikeres feltöltés!");
                        }
                        else
                        {
                            MessageBox.Show("Hiba!\n\n" + JSONObj.statusCode + "\n" + JSONObj.message);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                _naplo.Error(exception.StackTrace);
            }
        }
       
        public async void UploadImage(object sender, RoutedEventArgs e)
        {
            try
            {
                ConfluenceAPIMetodusok congluenceAPIMetodusok = new ConfluenceAPIMetodusok();

                string fajlNev = "";
                byte[] kepEleresiUtja = null;

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "image files (*.jpeg)|*.jpeg|(*.jpg)|*.jpg|(*.png)|*.png";
                openFileDialog.InitialDirectory = @"c:\";

                if (openFileDialog.ShowDialog() == true)
                {
                    kepEleresiUtja = File.ReadAllBytes(openFileDialog.FileName);
                    fajlNev = System.IO.Path.GetFileName(openFileDialog.FileName);
                }
                if (uiFelhasznaloNev.Text.Equals("") || uiJelszo.Password.Equals(""))
                {
                    MessageBox.Show("Username and password can not be empty!");
                }
                else
                {

                    string response = await congluenceAPIMetodusok.KepFeltoltes(
                    uiFelhasznaloNev.Text
                    , uiJelszo.Password
                    , APPSETTINGS_TERAZONOSITO
                    , APPSETTINGS_URL
                    , uiPageName.Text
                    , kepEleresiUtja
                    , fajlNev
                    , APPSETTINGS_IDHOSSZA
                    );

                    ConfluenceAPIResponse JSONObj = new ConfluenceAPIResponse();
                    JSONObj = JsonConvert.DeserializeObject<ConfluenceAPIResponse>(response);
                    if (JSONObj.statusCode == null)
                    {
                        MessageBox.Show("Sikeres feltöltés!");
                    }
                    else
                    {
                        MessageBox.Show("Hiba!\n\n" + JSONObj.statusCode + "\n" + JSONObj.message);
                    }
                }
            }catch(Exception exception)
            {
                _naplo.Error(exception.StackTrace);
            }
        }
    }
       
    
}
