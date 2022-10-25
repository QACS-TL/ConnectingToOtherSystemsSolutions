using System;
using System.IO;
using System.Net;
using System.Net.Http;
using LabCinemaTicketPriceCalculator;
using Newtonsoft.Json;

namespace LabCinemaTicketPriceCalculator
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Simple listener
            using (HttpListener listener = new HttpListener())
            {
                listener.Prefixes.Add("http://localhost:8000/");
                listener.Start();
                while (true)
                {
                    HttpListenerContext context = listener.GetContext(); // Wait for request

                    // Extracting information from the request URL
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    if (context == null) { 
                        Console.WriteLine("Context is null");
                        return;
                    }
                    sb.AppendLine($"Host: {context.Request.Url.Host}");
                    sb.AppendLine($"Document: {context.Request.Url.AbsolutePath}");



                    CustomerTicket ticket = new CustomerTicket();
                    ticket.AdultCount = 0;
                    ticket.ChildCount = 0;
                    if (context.Request is not null)
                    {
                        Console.WriteLine("Context.Request is null");
                        return;
                    }
                    ticket.AdultCount = int.Parse(context.Request.QueryString["AdultCount"]);
                    ticket.ChildCount = int.Parse(context.Request.QueryString["ChildCount"]);

                    string responseText = sb.ToString();

                    // Extracting the payload body
                    string bodyText = null;
                    if (context.Request.HasEntityBody)
                    {
                        using (StreamReader reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                        {
                            bodyText = reader.ReadToEnd();
                        }
                    }

                    dynamic filmDetails = JsonConvert.DeserializeObject(bodyText);
                    byte[] responseBytes = null;
                    context.Response.StatusCode = 200;
                    if (filmDetails != null && filmDetails.film_id != null && filmDetails.date != null & filmDetails.time != null)
                    {
                        //TODO: Code that looks up file title would go here
                        if (filmDetails.film_id < 500)
                            ticket.Title = "It's a Wonderful Life";
                        else
                            ticket.Title = "Carry on carrying on";

                        ticket.Date = filmDetails.date;
                        ticket.Time = filmDetails.time;
                        responseText += $"Booking is for \"{ticket.Title}\" on {filmDetails.date} at {filmDetails.time}"
                                        + $" and will cost {ticket.TotalPrice:C}";
                        responseBytes = System.Text.Encoding.UTF8.GetBytes(responseText);
                        context.Response.StatusCode = 200;
                        context.Response.StatusDescription = "OK";
                    }
                    else
                    {
                        responseText += "There were NO film details passed in a JSON format within the request body"
                                     + "\n Format should be {\"film_id\":\"nnnn\", \"date\":\"HH/MM/YYYY\", \"time\":\"nn:nn\"}";
                        responseBytes = System.Text.Encoding.UTF8.GetBytes(responseText);
                        context.Response.StatusCode = 400;
                        context.Response.StatusDescription = "Badness";
                    }
                    context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
                    context.Response.OutputStream.Close();
                }
                listener.Stop();
            }
        }
    }
}
