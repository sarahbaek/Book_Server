using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static readonly List<Book> data = new List<Book>();

        static void Main(string[] args)
        {
            #region Initializing new books 
            Book book1 = new Book("Beauty ", "Maria", 600, "978-0-000-00000-1");
            Book book2 = new Book(" beast", "Carol", 200, "978-0-000-00000-2");
            Book book3 = new Book("Cars", "Andreas", 400, "978-0-000-00000-3");
            Book book4 = new Book("Women", "Maa", 500, "978-0-000-00000-4");
            data.Add(book1);
            data.Add(book2);
            data.Add(book3);
            data.Add(book4);
            #endregion

            #region Server
            Console.WriteLine("This is the Book Server");
            TcpListener listener = new TcpListener(System.Net.IPAddress.Any, 4646);
            listener.Start();

            while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();
                Console.WriteLine("Incoming client");
                Task.Run((() => { HandleClient(socket); }));
            }
            #endregion
        }

        #region Method: HandleClient()
        /// <summary>
        /// Here we handle all the requests coming from the client, like when the client asks to see what is in the server, tries to get a secific item or tries to save an item
        /// </summary>
        /// <param name="socket"></param>
        private static void HandleClient(TcpClient socket)
        {
            NetworkStream ns = socket.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);

            string firstLineMessage = reader.ReadLine();
            string secondLineMessage = reader.ReadLine();

            if (firstLineMessage == "GetAll")
            {
                var tempString = GetAll();
                Console.WriteLine(tempString);
                writer.WriteLine(tempString);
            }

            if (firstLineMessage == "Get")
            {
                writer.WriteLine(Get(secondLineMessage));
            }

            if (firstLineMessage == "Save")
            {
                Save(secondLineMessage);
            }

            writer.Flush();
            socket.Close();

        }

        #endregion

        #region Method: Get ALL()
        //Handles the information coming from the client
        /// <summary>
        /// GetAll returns a list of books as a JsonString,     //GetAll //all books from the server, line two is empty e.g. Retrieve All
        /// </summary>
        /// <returns>alist of books as Json String</returns>
        public static string GetAll()
        {
            string serializedData = JsonSerializer.Serialize(data);

            return serializedData;
        }
        #endregion

        #region Method: Get()
        /// <summary>
        /// Get - takes in an ISBN!3 string and returns a book as a JsonString
        /// </summary>
        /// <param name="incoming_ISBN13"></param>
        /// <returns>Returns a Json string</returns>
        public static string Get(string incomingISBN13)
        {
            Book searchedBook = data.Find(book => book.ISBN13.Equals(incomingISBN13));
            string serializedJsonData = JsonSerializer.Serialize<Book>(searchedBook);

            return serializedJsonData;
        }
        #endregion

        #region Method: Save()
        /// <summary>
        /// Save // Book is saved (added to static list)
        /// </summary>
        /// <param name="inNewJsonBook" >{"Title": "UML", "Author": "Larman", "Page Number": 654, "ISBN13:" 9780133594140 "”} Book is saved like json</param>

        public static void Save(string inNewJsonBook)
        {
            Book saveDeserializedBook = JsonSerializer.Deserialize<Book>(inNewJsonBook);
            data.Add(saveDeserializedBook);
        }

        #endregion

    }

}
