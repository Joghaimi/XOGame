using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Mono.Unix.Native;
using System.Net.Http.Headers;

namespace Library.APIIntegration
{
    public static class APIIntegration
    {
        public static async Task<string> AuthorizationAsync(string authorizationURL, string userName, string password)
        {
            try
            {

                using (HttpClient httpClient = new HttpClient())
                {
                    var Body = new
                    {
                        username = userName,
                        password = password
                    };

                    string jsonData = JsonConvert.SerializeObject(Body);
                    StringContent content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(authorizationURL, content);
                    Console.WriteLine($"response.IsSuccessStatusCode {response.IsSuccessStatusCode}");

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        return responseContent;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Authorization failed {ex}");
            }
            return "";
        }

        public async static Task<Player> ReturnPlayerInformation(string userInfoURL, string token, string playerId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var Body = new
                {
                    rfid = playerId,
                };
                string jsonData = JsonConvert.SerializeObject(Body);
                StringContent content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                
                var tokenObject = new { token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6MiwiaWF0IjoxNzEyNTY4MzgzLCJleHAiOjE3MTI1NzE5ODN9.PfqD0-S9kgVCJ9YLT4D2IBWOKbYKZnQNz0WMpYN32mk" };
                string tokenJson = JsonConvert.SerializeObject(tokenObject);
                
                //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
                //httpClient.DefaultRequestHeaders.Add("Authorization", "{\"token\":\"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6MiwiaWF0IjoxNzEyNTY4MzgzLCJleHAiOjE3MTI1NzE5ODN9.PfqD0-S9kgVCJ9YLT4D2IBWOKbYKZnQNz0WMpYN32mk\"}");
                httpClient.DefaultRequestHeaders.Add("Authorization", "{}");
                HttpResponseMessage response = await httpClient.PostAsync(userInfoURL, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    List<Player> people = JsonConvert.DeserializeObject<List<Player>>(responseContent);
                    if (people?.Count > 0)
                    {
                        return people[0];

                    }
                }
            }
            return null;
        }

    }
}
