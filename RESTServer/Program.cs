using System;
using Nancy;
using Nancy.Hosting.Self;
using Nancy.ModelBinding;
using RESTLib;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Data.SqlTypes;
using Nancy.Swagger.Annotations.Attributes;

namespace RESTServer
{
    [Model("HOME1")]
    public class Module : NancyModule
    {

        public Module()
            : base()
        {
            Get("/", _ => Response.AsRedirect("/swagger-ui/dist/index.html"), null, "PROGRAMM");

            // Get("/swagger-ui/dist/index.html", _ => Response.AsFile("swagger-ui/dist/index.html"), null, "Home");

            Post("/Hello", (_) =>
            {
                var request = this.Bind<HelloRequest>();
                return Response.AsJson(new HelloResponse
                {
                    Text = request.Text + " => " + DateTime.Now.ToString("HH:mm:ss"),
                    Id = request.Id + 1,
                });
            });

            Get("/user/{id}", (parameters) => UserId(parameters));
        }

        public static string UserId(dynamic parameters)
        {
            if (((int)parameters.id) == 666)
                return $"Blocked user #{parameters.id}!";
            else
                return "Regular user!";
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var Host = "localhost";
                var Port = 4711;
                string HostUriAsString = string.Format("http://{0}:{1}", Host, Port);
                using (NancyHost host = new NancyHost(new Uri(HostUriAsString)))
                {
                    Console.WriteLine("RESTServer started on {0}", HostUriAsString);
                    host.Start();
                    Console.WriteLine("Return will terminate the program");
                    while (true)
                    {
                        if (Console.KeyAvailable)
                        {
                            var key = Console.ReadKey();
                            switch (key.KeyChar)
                            {
                                case '\n':
                                    return;
                            }
                        }
                        else
                            Thread.Sleep(100);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }
    }
}
