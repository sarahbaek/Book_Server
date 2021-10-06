using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.ConstrainedExecution;
using System.Text.Json;
using System.Threading.Tasks;
using Book_Library;

namespace Book_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("This is the Book Server");

            TcpListener listener = new TcpListener(System.Net.IPAddress.Any, 4646);

            listener.Start();
            while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();
                Console.WriteLine("Incoming client");
                Task.Run((() => { HandleClient(socket); }));

            }

        }

        private static void HandleClient(TcpClient socket)
        {
            NetworkStream ns = socket.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);
            string message = reader.ReadLine();

            Book FromJsonBook = JsonSerializer.Deserialize<Book>(message);

            Console.WriteLine("Client sent:" + FromJsonBook.Title + FromJsonBook.Author + FromJsonBook.PageNumber + FromJsonBook.ISBN13);

            writer.WriteLine("Book received");
            writer.Flush();
            socket.Close();

        }
    }
}
