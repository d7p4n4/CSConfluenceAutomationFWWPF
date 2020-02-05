using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSConfluenceAutomationFWWPF
{
    public class Metodus
    {
        public string AddConfluencePage(string cim, string terAzonosito, string szuloOsztalyNeve, string html, string URL, string felhasznaloNev, string jelszo)
       {
            string szuloOsztalyAzonosito = GetOldalIDNevAlapjan(felhasznaloNev, jelszo, URL, szuloOsztalyNeve);

            string DATA = "{\"type\":\"page\",\"ancestors\":[{\"type\":\"page\",\"id\":" + szuloOsztalyAzonosito + 
                "}],\"title\":\"" + cim + "\",\"space\":{\"key\":\"" + terAzonosito + "\"},\"body\":{\"storage\":{\"value\":\"" 
                + html + "\",\"representation\":\"storage\"}}}";

        System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            client.BaseAddress = new System.Uri(URL);
            byte[] cred = UTF8Encoding.UTF8.GetBytes(felhasznaloNev + ":" + jelszo);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(cred));
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            System.Net.Http.HttpContent content = new StringContent(DATA, UTF8Encoding.UTF8, "application/json");

            HttpResponseMessage message = client.PostAsync(URL, content).Result;
            string description = string.Empty;
               string result = message.Content.ReadAsStringAsync().Result;
                return result;
        }


        public async Task<string> KepFeltoltes(string felhasznaloNev, string jelszo, string URL, string oldalNeve, ByteArrayContent kepByteTomb, string fajlNev)
        {
            string oldalAzonositoja = GetOldalIDNevAlapjan(felhasznaloNev, jelszo, URL, oldalNeve);
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), URL + "/" + oldalAzonositoja + "/child/attachment"))
                {
                    request.Headers.TryAddWithoutValidation("X-Atlassian-Token", "nocheck");

                    var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(felhasznaloNev + ":" + jelszo));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                    var multipartContent = new MultipartFormDataContent();
                    multipartContent.Add(kepByteTomb, "file", fajlNev);
                    multipartContent.Add(new StringContent("This is my File"), "comment");
                    request.Content = multipartContent;

                    var response = await httpClient.SendAsync(request);
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }

        public string GetOldalIDNevAlapjan(string felhasznaloNev, string jelszo, string URL, string oldalNeve)
        {
            string eredmeny = "";
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("GET"), URL + "?title=" + oldalNeve + "&spaceKey=AT&expand=history"))
                {
                    var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(felhasznaloNev + ":" + jelszo));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                    //var response = await httpClient.SendAsync(request).Result;
                    HttpResponseMessage message = httpClient.SendAsync(request).Result;
                    string description = string.Empty;
                    string result = message.Content.ReadAsStringAsync().Result;
                    description = result;

                    eredmeny = result.Replace("{\"results\":[{\"id\":\"", "").Substring(0, 8);
                }
            }
            return eredmeny;
        }
    }
}