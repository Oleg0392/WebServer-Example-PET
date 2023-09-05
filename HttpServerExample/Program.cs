using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HttpServerExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            MyHttpServer server = new MyHttpServer("0.0.0.0", 8001);
            server.Start();
        }
    }
}


/*          var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:8001/");
            listener.Start();

            while (true)
            {
                var context = listener.GetContext();
                HttpListenerRequest request = context.Request;
                Console.WriteLine("Accepted Request: " + request.Url + ", request method: " + request.HttpMethod);

                using HttpListenerResponse response = context.Response;

                response.AddHeader("Content-Type", "text/plain");
                response.StatusCode = (int)HttpStatusCode.OK;
                response.StatusDescription = "y melya umau";

                string data = "salsa epicante";
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                response.ContentLength64 = buffer.Length;

                using Stream outStream = response.OutputStream;
                outStream.Write(buffer, 0, buffer.Length);
            }*/