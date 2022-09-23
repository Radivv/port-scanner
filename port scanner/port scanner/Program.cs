using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace port_scanner
{
    internal class Program
    {
        private static string IP = "wp.pl";

        private static int type = 0;

        public static int threads = 0;

        public static int last_Value = 0;

        private static List<int> port_List = new List<int>();

        private static List<string> output = new List<string>();


        public static void ThreadingStart()
        {
            for (int s = 0; s <= threads; s++) // multithreading
            {
                if (last_Value < port_List.Count())
                {
                    int val = port_List[last_Value];
                    Thread thr1 = new Thread(() => PortScan(IP, val));
                    thr1.Start();
                    last_Value++;
                    Console.Title = $"Port Scanner Ip:{IP} Scaned: {last_Value}/{port_List.Count().ToString()}";
                }

            }
        }

        public static void Main(string[] args)
        {
            Console.Title = "Port Scanner";
            Console.WriteLine("\r\n.----.  .----. .----.  .---.     .----. .---.   .--.  .-. .-..-. .-..----..----. \r\n| {}  }/  {}  \\| {}  }{_   _}   { {__  /  ___} / {} \\ |  `| ||  `| || {_  | {}  }\r\n| .--' \\      /| .-. \\  | |     .-._} }\\     }/  /\\  \\| |\\  || |\\  || {__ | .-. \\\r\n`-'     `----' `-' `-'  `-'     `----'  `---' `-'  `-'`-' `-'`-' `-'`----'`-' `-'\r\n");
            Console.WriteLine("Modern port scanner created by Radiv\ngithub.com/Radivv/port-scanner\n");
            UserInput();
            Console.WriteLine("[~] Start scanning\n");
            while (last_Value < port_List.Count)
            {
                ThreadingStart();
            }
            File.WriteAllLines("export.txt", output);
            Console.ReadKey();
            Console.WriteLine("\n");
        }

        private static void UserInput()
        {
            Console.WriteLine("IP Address:");
            IP = Console.ReadLine();
            Console.WriteLine("Threads:");
            threads = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Type of Ports:\n1 - Ports from port.txt \n2 - The most popular safety \n3 - Every 0-65.536\n");
            type = Convert.ToInt32(Console.ReadLine());
            if (type == 1)
            {
                string[] text = System.IO.File.ReadAllText("port.txt").Split('\n');

                foreach (var val in text)
                {
                    port_List.Add(Convert.ToInt32(val));
                }
            }
            else if (type == 2)
            {
                port_List.Add(21);
                port_List.Add(22);
                port_List.Add(23);
                port_List.Add(25);
                port_List.Add(53);
                port_List.Add(80);
                port_List.Add(110);
                port_List.Add(111);
                port_List.Add(135);
                port_List.Add(139);
                port_List.Add(143);
                port_List.Add(443);
                port_List.Add(445);
                port_List.Add(993);
                port_List.Add(995);
                port_List.Add(1723);
                port_List.Add(3306);
                port_List.Add(3389);
                port_List.Add(5900);
                port_List.Add(8080);

            }
            else if (type == 3)
            {
                for(int z = 1; z <= 65536; z++)
                {
                    port_List.Add(z);
                }
            }
            if (threads >= port_List.Count())
            {
                Console.WriteLine("[!] The number of threads is greater than the number of ports, the maximum possible value has been set\n");
                threads = port_List.Count();
            }

        }

        public static void PortScan(string ip, int port)
        {
            TcpClient Scan = new TcpClient();
            try
            {
                Scan.Connect(ip, port);
                Console.WriteLine($"[~] OPEN | {ip}:{port}");
                output.Add($"{ip}:{port}");
            }
            catch
            {

            }
        }
    }
}
