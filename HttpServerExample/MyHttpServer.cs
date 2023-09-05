using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.Http;
using System.Net.WebSockets;

namespace HttpServerExample
{
    public class MyHttpServer
    {
        private TcpListener tcpListener;
        public bool IsRunning { get; private set; }

        public MyHttpServer(string ipAddress, int port)
        {
            tcpListener = new TcpListener(IPAddress.Parse(ipAddress), port);
        }

        public MyHttpServer(string ipAddress)
        {
            tcpListener = new TcpListener(IPAddress.Parse(ipAddress), 8001);
        }

        public void Start()
        {
            if (IsRunning) return;

            tcpListener.Start();
            IsRunning = true;
            Console.WriteLine("Server is started");

            try
            {
                while(IsRunning)
                {
                    TcpClient client = tcpListener.AcceptTcpClient();
                    Thread requestThread = new Thread(() => { ProcessRequest(client); });
                    requestThread.Start();
                }
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        private void ProcessRequest(TcpClient clientHandler)
        {
            if (!clientHandler.Connected) return;

            Stream clientStream = clientHandler.GetStream();

            if (clientStream == null) return;

            byte[] buffer = new byte[1024];
            clientStream.Read(buffer, 0, buffer.Length);
            string requestData = Encoding.UTF8.GetString(buffer);
            Console.WriteLine(requestData);

            //string path = RestUtils.GetPath(requestData);
            HttpRequest request = RestUtils.CreateRequest(requestData);

            Console.WriteLine();    
            Console.WriteLine("current path: |" + request.Url + "|");

            if (request.Method.Equals("GET"))
            {
                switch (request.Url)
                {
                    case "/":
                        sendFileMedia(@"D:\Users\khusainovom\Documents\My Web Sites\design\examplepage.html", clientStream);
                        break;
                    case "/webapp.css":
                        sendFileMedia(@"D:\Users\khusainovom\Documents\My Web Sites\design\webapp.css", clientStream);
                        break;
                    case "/favicon.ico":
                        sendTextResponse("Maleykum SALAA", clientStream);
                        break;
                }
            }
            
            clientHandler.Close();
        }

        public void Stop()
        {
            if (!IsRunning) return;

            tcpListener.Stop();
            IsRunning = false;

            Console.WriteLine("Server is stopped");
        }

        private void sendFileMedia(string filePath, Stream clientStream)
        {
            byte[] fileRaw = new byte[4096];
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                stream.Read(fileRaw, 0, fileRaw.Length);
                stream.Close();
            }
            string fileContent = Encoding.UTF8.GetString(fileRaw);
           
            string responseData = "HTTP/1.1 200 salam maleykum\n";
            responseData += "Content-Length: " + fileContent.Length.ToString() + "\n";
            responseData += "Content-Type: text/html; charset=UTF-8\n";
            responseData += "Server: Microsoft-HTTPAPI/2.0\n";
            responseData += "Date: Fri, 01 Sep 2023 07:09:18 GMT\n\n";
            responseData += fileContent;

            Console.WriteLine("response media: " + responseData);
            byte[] buffer = Encoding.UTF8.GetBytes(responseData);
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Close();
        }

        private void sendTextResponse(string text, Stream clientStream)
        {
            string responseData = "HTTP/1.1 200 salam maleykum\n";
            responseData += "Content-Length: " + text.Length.ToString() + "\n";
            responseData += "Content-Type: text/plain; charset=UTF-8\n";
            responseData += "Server: Microsoft-HTTPAPI/2.0\n";
            responseData += "Date: Fri, 01 Sep 2023 07:09:18 GMT\n\n";
            responseData += text;

            Console.WriteLine("response string: " + responseData);
            byte[] buffer = Encoding.UTF8.GetBytes(responseData);
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Close();

        }
    }
}
