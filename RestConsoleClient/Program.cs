using System;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable;
using RESTLib;
using System.Text;
using System.Threading;
using System.CodeDom.Compiler;
using System.Threading.Tasks;

namespace RestConsoleClient
{
    class MainClass
    {
        async static Task MainAsync(string[] args, CancellationTokenSource cts)
        {
            using (RestClient client = new RestClient("http://localhost:4711") { Timeout = new TimeSpan(0, 0, 10) })
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey();
                        switch (key.KeyChar)
                        {
                            case 'a':
                                {
                                    RestRequest request = new RestRequest("Hello", RestSharp.Portable.Method.POST);
                                    request.AddJsonBody(new HelloRequest
                                        {
                                            Text = "HelloText",
                                            Id = 1,
                                        });
                                    var response = await client.Execute<HelloResponse>(request);
                                    Console.WriteLine($"Hello Response Text={response.Data.Text} Id={response.Data.Id}");
                                    break;
                                }
                            case'b':
                                {
                                    RestRequest request = new RestRequest("User/{id}", RestSharp.Portable.Method.GET);
                                    request.AddUrlSegment("id", 1);
                                    var response = await client.Execute<string>(request);
                                    Console.WriteLine($"User Response Text={Encoding.UTF8.GetString(response.RawBytes)}");
                                    break;
                                }
                            case 'c':
                                {
                                    RestRequest request = new RestRequest("User/{id}", RestSharp.Portable.Method.GET);
                                    request.AddUrlSegment("id", 666);
                                    var response = await client.Execute(request);
                                    Console.WriteLine($"User Response Text={Encoding.UTF8.GetString(response.RawBytes)}");
                                    break;
                                }
                            case '\n':
                                cts.Cancel();
                                return;
                        }
                    }
                    else
                        Thread.Sleep(100);
                }
            } 
        }

        [STAThread]
        public static void Main(string[] args)
        {
            Console.WriteLine(nameof(RestConsoleClient));

            CancellationTokenSource cts = new CancellationTokenSource();

            System.Console.CancelKeyPress += (s, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            MainAsync(args, cts).Wait();
        }
    }
}
