using System;
using System.Collections.Generic;
using System.Linq;
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

            HttpResponseMessage messge = client.PostAsync(URL, content).Result;
            string description = string.Empty;
            if (messge.IsSuccessStatusCode)
            {
               string result = messge.Content.ReadAsStringAsync().Result;
                description = result;
            }
        }
    }
}
