using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ChatClient
{
    class Program
    {
        static Dictionary<int, ConsoleColor> colors = new Dictionary<int, ConsoleColor>();
        public string _name;
        public int _color;
        static StreamWriter sw;
        static StreamReader sr;
        public event EventHandler<string> MessageReceived;
        static void Main(string[] args)
        {
            colors.Add(1, ConsoleColor.Blue);
            colors.Add(2, ConsoleColor.Cyan);
            colors.Add(3, ConsoleColor.Green);
            colors.Add(4, ConsoleColor.Yellow);
            colors.Add(5, ConsoleColor.Red);
            TcpClient client = new TcpClient("127.0.0.1", 5000);
            NetworkStream ns = client.GetStream();
            sw = new StreamWriter(ns);
            sr = new StreamReader(ns);
            Receive();
            Send();
        }
        static async Task Send()
        {
            while (true)
            {
                await sw.WriteLineAsync(Console.ReadLine());
                await sw.FlushAsync(); 
            }
        }
        static async Task Receive()
        {
            while (true)
            {
                string message = await sr.ReadLineAsync();
                String[] splited = message.Split(" ");
                try{
                    Console.ForegroundColor = colors[int.Parse(splited[0])];
                    System.Console.WriteLine(message.Substring(2));
                }
                catch(Exception e){
                    System.Console.WriteLine(message);
                }
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}