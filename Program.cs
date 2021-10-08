using System;
using System.Collections.Generic;
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
        public static readonly List<Book> Data = new List<Book>();






     
    
        
        static void Main(string[] args)
        {

            Book book1 = new Book("Beauty ", "Maria", 200, "978-0-000-00000-1");
            Book book2 = new Book(" beast", "Carol", 200, "978-0-000-00000-2");
            Book book3 = new Book("Cars", "Mria", 200, "978-0-000-00000-3");
            Book book4 = new Book("Women", "Maa", 200, "978-0-000-00000-4");


            Data.Add(book1);
            Data.Add(book2);
            Data.Add(book3);
            Data.Add(book4);

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
            string messageA = reader.ReadLine();
            string messageB = reader.ReadLine();


            Book FromJsonBook = JsonSerializer.Deserialize<Book>(message);

            Console.WriteLine("Client sent:" + FromJsonBook.Title + FromJsonBook.Author + FromJsonBook.PageNumber + FromJsonBook.ISBN13);

            writer.WriteLine("Book received");
            writer.Flush();
            socket.Close();

        }


        //Handles the information coming from the client
        public static string GetAll()
        {
            
            Book message = Data.
            string fromJsonBook = JsonSerializer.Serialize<Book>(message);

            return fromJsonBook;


            //if (message == "GetAll")
            //{
            //    var returnedJsonBooks = GetAll_Books();

            //    writer.WriteLine(returnedBooks);
            //}


            //    Console.WriteLine("Client sent book \r\nType of Book{0}", fromJsonBook.Title);
            //    writer.WriteLine("server: Book Received");

            //    writer.Flush();
            //    socket.Close();

            //}

            ///// <summary>
            ///// GetAll returns a list of books as a JsonString,     //GetAll //all books from the server, line two is empty e.g. Retrieve All
            ///// </summary>
            ///// <returns>alist of books as Json String</returns>

            //public static List<Book> GetAll_Books()
            //{
            //    // a list of books
            //    return new List<Book>(Data);
            //}

            ///// <summary>
            ///// Get - takes in an ISBN!3 string and returns a book as a JsonString
            ///// </summary>
            ///// <param name="in_ISBN13"></param>
            ///// <returns>Returns a Json string</returns>


         
            ////

            ///// <summary>
            ///// Save // Book is saved (added to static list)
            ///// </summary>
            ///// <param name="in_newBook" >{"Title": "UML", "Author": "Larman", "Page Number": 654, "ISBN13:" 9780133594140 "”} Book is saved like json</param>

            //public static void Save(Book in_newBook)
            //{
            //    Data.Add(in_newBook);
            //}
        }

        public static string Get(string in_ISBN13)
        {
            Book message = Data.Find(b => b.ISBN13.Equals(in_ISBN13));
            string fromJsonBook = JsonSerializer.Serialize<Book>(message);

            return fromJsonBook;
        }

        public static void Save(string jsonbook)
        {
            Book message = JsonSerializer.Deserialize<Book>(jsonbook);
            Data.Add(message);
        }

    }
