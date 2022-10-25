using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.IO;

namespace HttpClientCommentsForPosts
{

    internal class Program
    {
        static void Main(string[] args)
        {
            //Outdoor cinema weather check
            // Create an HTTP client (should only have one of these for whole application)
            var http = new HttpClient();

            //// Send a GET request and process the response as JSON, using C# dynamic objects
            string city = "Leeds";
            string url = $"https://weather-api.qaalabs.com/api/weather/{city}";
            string json = http.GetStringAsync(url).Result;
            dynamic obj = JsonConvert.DeserializeObject(json);
            string temp = obj["TemperatureInCelsius"] >= 15 ? "warm" : "cold";
            Console.WriteLine($"The weather for the outdoor cinema event " +
                $"in {obj["City"]} is {obj["WeatherDescription"]} and it will be {temp}.");


            // Sending headers with request, and extracting headers from response
            var blog = new
            {
                title = "My movie blog post - Up",
                body = "Up is an uplifting film",
                userId = 101
            };
            string dataJson = JsonConvert.SerializeObject(blog);
            url = "https://jsonplaceholder.typicode.com/posts";
            HttpContent content = new StringContent(dataJson, Encoding.UTF8, "application/json");
            HttpResponseMessage response = http.PostAsync(url, content).Result;
            string responseJson = response.Content.ReadAsStringAsync().Result;
            dynamic responseData = JsonConvert.DeserializeObject(responseJson);
            Console.WriteLine($"");
            Console.WriteLine($"New blog post has ID: {responseData["id"]}");
            Console.WriteLine($"New blog post has title text of: {responseData["title"]}");
            Console.WriteLine($"New blog post has body text of: {responseData["body"]}");
            Console.WriteLine($"New blog post has userID of: {responseData["userId"]}");

            //******************************************************************************

            int postId = 5;
            url = $@"https://jsonplaceholder.typicode.com/posts/{postId}";
            content = new StringContent(dataJson, Encoding.UTF8, "application/json");
            response = http.GetAsync(url).Result;
            responseJson = response.Content.ReadAsStringAsync().Result;
            dynamic post = JsonConvert.DeserializeObject(responseJson, typeof(Post));
            Console.WriteLine($"");
            Console.WriteLine($"New blog post has an ID of {post.Id}");
            Console.WriteLine($"New blog post has userID of {post.UserId}");
            Console.WriteLine($"New blog post has title text of {post.Title}");
            Console.WriteLine($"New blog post has body text of {post.Body}");

            url = $@"https://jsonplaceholder.typicode.com/posts/{postId}/comments";
            content = new StringContent(dataJson, Encoding.UTF8, "application/json");
            response = http.GetAsync(url).Result;
            responseJson = response.Content.ReadAsStringAsync().Result;
            dynamic comments = JsonConvert.DeserializeObject(responseJson, typeof(List<Comment>));

            Console.WriteLine($"\nPost {postId} has the following comments:");
            foreach (Comment comment in comments)
            {
                Console.WriteLine($"From: {comment.Name}, Comment: {comment.Body}");
            }
        }
    }
}
