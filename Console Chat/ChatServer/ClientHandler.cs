using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace ChatServer
{
    class ClientHandler
    {
        public bool isActive = false;
        public string _name;
        public int _color;
        public TcpClient _client;
        StreamReader _sr;
        StreamWriter _sw;
        public event EventHandler<string> MessageReceived;
        public ClientHandler(TcpClient client)
        {
            _client = client;
            isActive = true;
            _sr = new StreamReader(_client.GetStream());
            _sw = new StreamWriter(_client.GetStream());
        }
        public async Task ProcessMessage()
        {
            while (true)
            {
                try
                {
                    string message = await _sr.ReadLineAsync();
                    if (MessageReceived != null)
                    {
                        MessageReceived(this, message);
                    }
                }
                catch (Exception e)
                {
                    if (e.GetType() == typeof(IOException))
                    {
                        isActive = false;
                        break;
                    }
                }
            }
        }
        public async Task SendMessage(string message)
        {
            await _sw.WriteLineAsync(message);
            await _sw.FlushAsync();
        }
        public async void Name()
        {
            _name = _sr.ReadLine();
        }
        public async void Color()
        {
            _color = int.Parse(_sr.ReadLine());
        }
    }
}