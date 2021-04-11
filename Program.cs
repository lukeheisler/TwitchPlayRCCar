using System;
//using SerialPort
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot bot = new Bot();
            Console.ReadLine();
        }
    }

    class Bot
    {
        TwitchClient client;
        String accessToken;
        public string Move { get; set; }

        public Bot()
        {
            accessToken = System.IO.File.ReadAllText("C:\\Users\\Zacke\\Documents\\Projects\\Twitch-Processes\\access_token");
            ConnectionCredentials credentials = new ConnectionCredentials("twitchplayrccar", accessToken);
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient customClient = new WebSocketClient(clientOptions);
            client = new TwitchClient(customClient);
            client.Initialize(credentials, "twitchplayrccar");

            // adds listener functions for events
            client.OnMessageReceived += Client_OnMessageReceived;
            client.OnLog += Client_OnLog;
            client.OnJoinedChannel += Client_OnJoinedChannel;
            client.OnConnected += Client_OnConnected;

            client.Connect();
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            if (e.ChatMessage.Message.Contains("badword"))
                Console.WriteLine(e.ChatMessage.Message);

            String data = isCommand(e.ChatMessage.Message);
            if (data == null) return;

            String strJSON = parseCommand(data);

        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            Console.WriteLine($"{e.DateTime.ToString()}: {e.BotUsername} - {e.Data}");
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            Console.WriteLine($"Connected to {e.AutoJoinChannel}");
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            Console.WriteLine("Hey guys! I am a bot connected via TwitchLib!");
            client.SendMessage(e.Channel, "Hey guys! I am a bot connected via TwitchLib!");

        }


        // funtion: isCommand
        // desc: checks if the given message is a commands
        // return: string
        //      null if not a command, actual command if it is
        private string isCommand(string command)
        {
            if (command[0] == '!')
                return command.Substring(1);
            return null;
        }

        // parseCommand
        // function: parseCommand
        // desc: parses command and)
        // example inputs: "move:left, dist:2"
        // example output: "{"move": "left", "dist": 2}"

        public string parseCommand(string command)
        {
            string[] separatingStrings = { ":", "," };


            string text = command;
            System.Console.WriteLine($"Original text: '{text}'");

            string[] words = text.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);

            string first = words[0];
            string second = words[1];
            string third = words[2];
            string fourth = words[3];

            string data = "{'" + first + "': '" + second + "', '" + third + ": " + fourth + "}";
            System.Console.WriteLine(data);
            return data;
        }

        // function: pipe
        // arguments: json data as string, outputProcess
        // desc: passes the xml to a named pipe
        // retun: void

        /*
        static void Main(string[] args)
        {
            var pipe = new System.IO.Pipes.NamedPipeServerStream("MyPipe", System.IO.Pipes.PipeDirection.InOut, 1);

            pipe.WaitForConnection();
            Console.WriteLine("Client connected.");
        }
        */
        //idk which is right
        /*    public static void pipeData(String jsonData, outputProcess)
        {
            using (System.IO.Pipes.NamedPipeServerStream namedPipeServer = new System.IO.Pipes.NamedPipeServerStream("command"))
            {
                namedPipeServer.WaitForConnection();
                namedPipeServer.WriteByte(0);
                int byteFromClient = namedPipeServer.ReadByte();
                Console.WriteLine(byteFromClient);
            }
        }
        */
    }
}

