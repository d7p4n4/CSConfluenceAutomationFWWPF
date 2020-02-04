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
        public void AddConfluencePage(string cim, string terAzonosito, string szuloOsztalyAzonosito, string html, string URL, string felhasznaloNev, string jelszo)
       {
            //string DATA = @"{""type"":""page"",""title"":" + cim + @",""ancestor"":[{""type"":""page"",""id:" + szuloOsztalyAzonosito +
            //    @"}],""space"":{""key"":" + terAzonosito + @",""body"":{""storage"":{""value"":" + html + @",""representation"":""storage""}}}";


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
                description = result;
        }

        public async void UploadAttachment(string felhasznaloNev, string jelszo, string URL)
        {
            HttpClient httpClient = new HttpClient();
            MultipartFormDataContent form = new MultipartFormDataContent();
            httpClient.DefaultRequestHeaders.Add("X-Atlassian-Token", "nocheck");

            httpClient.BaseAddress = new System.Uri("http://confluence.sycompla.org/rest/api/content/11337787/child/attachment");
            byte[] cred = UTF8Encoding.UTF8.GetBytes(felhasznaloNev + ":" + jelszo);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(cred));
            //httpClient.DefaultRequestHeaders.Add("X-Atlassian-Token", "nockeck");

            byte[] byteArray = Encoding.UTF8.GetBytes(KepKonvertalasBase64("d:\\image1.jpeg"));
            HttpContent content = new ByteArrayContent(byteArray);

            form.Add(content, "image/jpeg", "image1.jpg");
            var response = await httpClient.PostAsync("http://confluence.sycompla.org/rest/api/content/11337787/child/attachment", form);


            response.EnsureSuccessStatusCode();
            httpClient.Dispose();
            string sd = response.Content.ReadAsStringAsync().Result;
        }

        public string KepKonvertalasBase64(string kepHelye)
        {
            byte[] kepBajtjai = System.IO.File.ReadAllBytes(kepHelye);
            string base64String = Convert.ToBase64String(kepBajtjai);
            return base64String;
        }

        public byte[] FajlKepkent(string kepHelye)
        {
            return System.IO.File.ReadAllBytes(kepHelye);
        }

        public async void ConvertFromCURL(string felhasznaloNev, string jelszo, string URL, string oldalAzonositoja, ByteArrayContent kepByteTomb, string fajlNev)
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), URL + oldalAzonositoja + "/child/attachment"))
                {
                    request.Headers.TryAddWithoutValidation("X-Atlassian-Token", "nocheck");

                    var base64authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(felhasznaloNev + ":" + jelszo));
                    request.Headers.TryAddWithoutValidation("Authorization", $"Basic {base64authorization}");

                    var multipartContent = new MultipartFormDataContent();
                    multipartContent.Add(kepByteTomb, "file", fajlNev);
                    multipartContent.Add(new StringContent("This is my File"), "comment");
                    request.Content = multipartContent;

                    var response = await httpClient.SendAsync(request);
                }
            }
        }
    }
}