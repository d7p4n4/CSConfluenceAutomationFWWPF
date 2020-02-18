
using CSConfluenceCapFW;
using CSConfluenceServiceFW;
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
using System.Xml;
using System.Xml.Serialization;

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
            uiJelszo.Password = "szappan60";
            uiFelhasznaloNev.Text = "d7p4n4";
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
                ConfluenceServices confluenceServices = new ConfluenceServices(); 

                string html = "";
                string xsl = "";
                string xml = "";

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "xsl files (*.xsl)|*.xsl";
                openFileDialog.InitialDirectory = @"d:\Server\bizalmas\";

                if (openFileDialog.ShowDialog() == true)
                {
                    xsl = File.ReadAllText(openFileDialog.FileName);
                }

                OpenFileDialog openFileDialog2 = new OpenFileDialog();
                openFileDialog.Filter = "xml files (*.xml)|*.xml";
                openFileDialog.InitialDirectory = @"d:\Server\bizalmas\";

                if (openFileDialog2.ShowDialog() == true)
                {
                    xml = File.ReadAllText(openFileDialog2.FileName);
                }

                html = confluenceAPIMetodusok.TransformXMLToHTML(xml, xsl);

                if (uiFelhasznaloNev.Text.Equals("") || uiJelszo.Password.Equals(""))
                {
                    MessageBox.Show("Username and password can not be empty!");
                }
                else
                {
                    
                    AddNewPageCompositeResponse addNewPageCompositeResponse =
                        confluenceServices.AddNewPageComposite(new AddNewPageCompositeRequest()
                        { 
                            Username = uiFelhasznaloNev.Text
                            , PageTitle = uiPageName.Text
                            , Password = uiJelszo.Password
                            , SpaceKey = uiSpaceKey.Text
                            , URL = APPSETTINGS_URL
                            , Content = html
                            , ParentPageTitle = uiParentPageName.Text
                        });
                        
                    if (addNewPageCompositeResponse.AddNewPageResult.FailedResponse == null)
                    {
                        MessageBox.Show("Sikeres feltöltés!");
                    }
                    else
                    {
                        MessageBox.Show("Hiba!\n\n" + addNewPageCompositeResponse.AddNewPageResult.FailedResponse.StatusCode + "\n" + 
                            addNewPageCompositeResponse.AddNewPageResult.FailedResponse.Message);
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
                string base64String = "";
                byte[] kepEleresiUtja = null;

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "image files (*.jpeg)|*.jpeg|(*.jpg)|*.jpg|(*.png)|*.png";
                openFileDialog.InitialDirectory = @"c:\";

                if (openFileDialog.ShowDialog() == true)
                {
                    kepEleresiUtja = File.ReadAllBytes(openFileDialog.FileName);
                    fajlNev = System.IO.Path.GetFileName(openFileDialog.FileName);

                        base64String = Convert.ToBase64String(kepEleresiUtja);
                }
                if (uiFelhasznaloNev.Text.Equals("") || uiJelszo.Password.Equals(""))
                {
                    MessageBox.Show("Username and password can not be empty!");
                }
                else
                {

                    UploadAttachmentCompositeRespone uploadAttachmentCompositeResponse = await
                        new ConfluenceServices().UploadAttachmentComposite(new UploadAttachmentCompositeRequest()
                        {
                            Password = uiJelszo.Password
                            , FileName = fajlNev
                            , PageTitle = uiPageName.Text
                            , SpaceKey = uiSpaceKey.Text
                            , URL = APPSETTINGS_URL
                            , Username = uiFelhasznaloNev.Text
                            , ImageFileBase64String = base64String
                        });

                    if (uploadAttachmentCompositeResponse.UploadAttachmentResult.FailedResponse == null)
                    {
                        MessageBox.Show("Sikeres feltöltés!");
                    }
                    else
                    {
                        MessageBox.Show("Hiba!\n\n" + uploadAttachmentCompositeResponse.UploadAttachmentResult.FailedResponse.StatusCode + "\n" + 
                            uploadAttachmentCompositeResponse.UploadAttachmentResult.FailedResponse.Message);
                    }
                }
            }catch(Exception exception)
            {
                _naplo.Error(exception.StackTrace);
            }
        }
    }
       
    
}
