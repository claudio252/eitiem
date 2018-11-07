using Amazon.Lambda.Core;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Eitiem
{
    public class Function
    {

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public HttpResponseMessage[] FunctionHandler(JObject data, ILambdaContext context)
        {
            Task<HttpResponseMessage[]> resultTasks;
            List<Task<HttpResponseMessage>> results = new List<Task<HttpResponseMessage>>();

            if (data == null)
            {
                throw new MalformedObjectData();
            }

            data.TryGetValue("url", StringComparison.OrdinalIgnoreCase, out var url);
            data.TryGetValue("api_key", StringComparison.OrdinalIgnoreCase, out var key);
            data.TryGetValue("numbers", StringComparison.OrdinalIgnoreCase, out var numbers);
            data.TryGetValue("message", StringComparison.OrdinalIgnoreCase, out var message);

            foreach (var jToken in numbers)
            {
                results.Add(sendMessage(url.ToString(), key.ToString(), jToken.ToString(), message.ToString()));
            }

            resultTasks = Task.WhenAll(results);

            return resultTasks.Result;
        }

        private async Task<HttpResponseMessage> sendMessage(string url, string api_key, string number, string message)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = null;

            url = $"{url.ToString()}?apiKey={api_key}&to={number}&content={message}";

            Console.WriteLine($"Will send message to: {number}");
            Console.WriteLine($"Message is: {message}");
            Console.WriteLine($"URL is: {url}");

            try
            {
                response = await client.GetAsync(url);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
            }

            client.Dispose();

            return response;
        }
    }
}
