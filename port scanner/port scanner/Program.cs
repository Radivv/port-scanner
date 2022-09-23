using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


//.----.  .----. .----.  .---.     .----. .---.   .--.  .-. .-..-. .-..----..----.
//| {}  }/  {}  \| {}  }{_   _}   { {__  /  ___} / {} \ |  `| ||  `| || {_  | {}  }
//| .--' \      /| .-. \  | |     .-._} }\     }/  /\  \| |\  || |\  || {__ | .-. \
//`-'     `----' `-' `-'  `-'     `----'  `---' `-'  `-'`-' `-'`-' `-'`----'`-' `-'
// github.com/Radivv/port-scanner

namespace port_scanner
{
    enum types
    { 
        none = 0,
        portsFromTxt = 1,
        usefulPorts = 2,
        everyPorts = 3,
        portsStartFinish = 4,
    }

    internal class Program
    {
        private static string IP = "";

        private static int start_Port = 0;

        private static int finish_Port = 0;

        private static types type;

        public static int threads = 0;

        public static int last_Value = 0;

        private static List<int> port_List = new List<int>();

        private static List<string> output = new List<string>();

        public static void ThreadingStart()
        {
            for (int s = 0; s <= threads; s++) // Multithreading
            {
                if (last_Value < port_List.Count())
                {
                    int val = port_List[last_Value];
                    Thread thr1 = new Thread(() => PortScan(IP, val)); // Port scanning
                    thr1.Start();
                    last_Value++;
                    Console.Title = $"[!] Port Scanner Ip:{IP} Scaned: {last_Value}/{port_List.Count().ToString()}";
                }
            }
        }

        public static void Main(string[] args)
        {

            Console.Title = "[!] Port Scanner";
            Console.WriteLine("\r\n.----.  .----. .----.  .---.     .----. .---.   .--.  .-. .-..-. .-..----..----. \r\n| {}  }/  {}  \\| {}  }{_   _}   { {__  /  ___} / {} \\ |  `| ||  `| || {_  | {}  }\r\n| .--' \\      /| .-. \\  | |     .-._} }\\     }/  /\\  \\| |\\  || |\\  || {__ | .-. \\\r\n`-'     `----' `-' `-'  `-'     `----'  `---' `-'  `-'`-' `-'`-' `-'`----'`-' `-'\r\n");
            Console.WriteLine("Modern port scanner created by Radiv\ngithub.com/Radivv/port-scanner\n");
            UserInput();
            Console.WriteLine("[~] Start scanning\n");
            while (last_Value < port_List.Count)
            {
                ThreadingStart();
            }
            File.WriteAllLines("export.txt", output); // Exporting the succes connection to export.txt near the .exe
            Console.ReadKey();
            Console.WriteLine("\n");
        }

        private static void UserInput()
        {
            Console.WriteLine("IP Address:");
            IP = Console.ReadLine();
            Console.WriteLine("Threads:");
            threads = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Type of Ports:\n1 - Ports from port.txt \n2 - The most popular safety \n3 - Every 0-65.536\n4 - Start and Finish port");
            repeat:
            int selected_Type = Convert.ToInt32(Console.ReadLine()); // Choosing scan type
            switch (selected_Type)
            {
                default:
                    {
                        Console.WriteLine("[!] Problem with selecting scanning type..."); // Selected value is over the limit
                        Console.WriteLine("[!] Try once again");
                        goto repeat;
                    }
                case 1:
                    {
                        type = types.portsFromTxt;
                    }break;
                case 2:
                    {
                        type = types.usefulPorts;
                    }break;
                case 3:
                    {
                        type = types.everyPorts;
                    }
                    break;
                case 4:
                    {
                        type = types.portsStartFinish;
                    }break;
            }
            switch (type)
            {
                case types.portsFromTxt:
                {
                    string[] text = System.IO.File.ReadAllText("port.txt").Split('\n');

                    foreach (var val in text)
                    {
                        port_List.Add(Convert.ToInt32(val));
                    }
                }
                    break;
                case types.usefulPorts:
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
                    break;
                case types.everyPorts:
                    {
                        for (int z = 1; z <= 65536; z++)
                        {
                            port_List.Add(z);
                        }
                    }
                    break;
                case types.portsStartFinish:
                    {
                        Console.WriteLine($"[!] Write the start port");
                        repeats:
                        start_Port = Convert.ToInt32(Console.ReadLine());
                        if (start_Port < 0) // Start port is over the limit
                        {
                            Console.WriteLine($"[!] The port is over the limit minimum size is 0");
                            Console.WriteLine($"[!] Write the start port once again!");
                            goto repeats;
                        }
                        Console.WriteLine($"[!] Write the finish port");
                        repeatf:
                        finish_Port = Convert.ToInt32(Console.ReadLine());
                        if (finish_Port > 65535) // Finish port is over the limit
                        {
                            Console.WriteLine($"[!] The port is over the limit max size is 65.535");
                            Console.WriteLine($"[!] Write the finish port once again!");
                            goto repeatf;
                        }
                        Console.WriteLine("");


                        for (int s = start_Port; s <= finish_Port; s++)
                        {
                            port_List.Add(s);
                        }
                    }
                    break;
            }
            if (threads >= port_List.Count()) // Selected threads are over the limit
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
                Console.WriteLine($"[~] Open | {ip}:{port}");
                output.Add($"{ip}:{port}");
            }
            catch
            {
                // nothing
            }
        }
    }
}
