using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace ChatServer
{
    class Program
    {
        static List<ClientHandler> Clients = new List<ClientHandler>();
        static Dictionary<int, ConsoleColor> colors = new Dictionary<int, ConsoleColor>();
        static int alive = 0;
        static async Task StartServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 5000);
            listener.Start();
            System.Console.WriteLine("Server started");
            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                alive++;
                ClientHandler handler = new ClientHandler(client);
                System.Console.WriteLine("New client has connected!");
                handler.SendMessage("Server:Welcome to chat!\nWhat is your name?");
                handler.Name();
                handler.SendMessage("Server:What color do you want?" +
                                    "\n1.Blue\n2.Cyan\n3.Green\n4.Yellow\n5.Red");
                handler.Color();
                handler.MessageReceived += MessageReceived;
                if (isNewUser(handler) == -1)
                    Clients.Add(handler);
                else
                    Clients[isNewUser(handler)] = handler;
                client.GetStream();
                handler.ProcessMessage();
                System.Console.WriteLine("Users info:");
                System.Console.WriteLine("IP Adress           Name       Color      Status");
                UserInfo();
            }
        }
        static void MessageReceived(object sender, string message)
        {
            ClientHandler ch = (ClientHandler)sender;
            Console.ForegroundColor = colors[ch._color];
            bool direct = false;
            String s = message;
            String sendto = "";
            String[] check = s.Split(" ");
            if (check[0].Contains("@") && check.Length > 1)
            {
                direct = true;
                sendto = check[0].Substring(1);
            }
            if (direct == true)
            {
                foreach (var item in Clients)
                {
                    if (item._name.Equals(sendto))
                    {
                        item.SendMessage(ch._color + " " + ch._name + ":" + message);
                        break;
                    }
                }
            }
            else
            {
                System.Console.WriteLine(ch._name + ":" + message);
                foreach (var item in Clients)
                {
                    if (sender != item)
                    {
                        item.SendMessage(ch._color + " " + ch._name + ":" + message);
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static int isNewUser(ClientHandler c)
        {
            for (int i = 0; i < Clients.Count; i++)
            {
                if (c._name.Equals(Clients[i]._name))
                    return i;
            }
            return -1;
        }
        public static void UserInfo()
        {
            alive = 0;
            foreach (var item in Clients)
            {
                String adress = item._client.Client.RemoteEndPoint.ToString();
                String name = item._name;
                ConsoleColor cc = colors[item._color];
                String s = "not alive";
                if (item.isActive)
                {
                    s = "alive";
                    alive++;
                }
                System.Console.WriteLine(adress + "     " + name + "    " + cc + "    " + s);
            }
            System.Console.WriteLine("Alive clients number: " + alive);
        }
        public static void Main(string[] args)
        {
            colors.Add(1, ConsoleColor.Blue);
            colors.Add(2, ConsoleColor.Cyan);
            colors.Add(3, ConsoleColor.Green);
            colors.Add(4, ConsoleColor.Yellow);
            colors.Add(5, ConsoleColor.Red);
            StartServer();
            while (true)
            {
                string str = Console.ReadLine();
                foreach (var item in Clients)
                {
                    item.SendMessage("Server:" + str);
                }
            }
        }
    }
}
