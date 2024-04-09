﻿using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Mono.Unix.Native;
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Net.Http;

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

        public async static Task<Player> ReturnPlayerInformation(string userName, string password, string userInfoURL, string playerId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var Body = new
                {
                    rfid = playerId,
                    username = userName,
                    password = password
                };

                string jsonData = JsonConvert.SerializeObject(Body);
                StringContent content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(userInfoURL, content);
                Console.WriteLine($"response.IsSuccessStatusCode {response.IsSuccessStatusCode}");
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


        public async static Task<string> NextRoomStatus(string nextRoomURL)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(nextRoomURL);
                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content as a string
                        string responseBody = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("Response received:");
                        Console.WriteLine(responseBody);
                        return responseBody;
                    }
                    else
                    {
                        // Handle the unsuccessful response (non-success status code)
                        Console.WriteLine($"Error: {response.StatusCode}");
                        return null;
                    }
                }
                catch (HttpRequestException ex)
                {
                    // Handle any exceptions that occurred during the request
                    Console.WriteLine($"Request failed: {ex.Message}");
                    return null;
                }
            }
        }

    }
}
