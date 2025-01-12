using System.IO.Ports;
using System.Text;

namespace SerialToConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            //Console.SetCursorPosition(40, 12);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Clear();

            Console.SetWindowSize(25, 25);
            Console.Title = "Serial to Console";
                        
            var ports = SerialPort.GetPortNames();

            if (ports is null || !ports.Any())
            {
                Console.WriteLine("No ports found.");
            }
            else
            {
                Console.WriteLine("Ports found:");
                foreach (var port in ports)
                {
                    Console.WriteLine(port);
                }
            }
            
            var serialPort = new SerialPort(/*ports[0], 9600*/);

            if (serialPort.IsOpen) serialPort.Close();
            else
            {
                var name = ports[2];
                serialPort.PortName = name;
                serialPort.BaudRate = 9600;
            }
            serialPort.Open();


            string data;
            var tela = new StringBuilder();
            do
            {
                data = serialPort.ReadExisting();

                if ("Clear\n".Equals(data)) Console.Clear();
                if ("Print\n".Equals(data)) Console.WriteLine(tela);
                else if (!string.IsNullOrEmpty(data))
                {
                    tela.Append(data);
                }
            } while (data != "sair\n") ;

            serialPort.Close();





        }
    }
}
