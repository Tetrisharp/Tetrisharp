using System.IO.Ports;

namespace EnviaSerial
{
    internal class Program
    {
        static void Main(string[] args)
        {

            //Console.SetCursorPosition(40, 12);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();

            Console.SetWindowSize(25, 25);
            Console.Title = "Envia serial";

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
                var name = ports[3];
                serialPort.PortName = name;
                serialPort.BaudRate = 9600;
            }

            serialPort.Open();
            string linha;

            do
            {
                linha = Console.ReadLine();
                serialPort.WriteLine(linha);
            } while (linha?.Length != 0);

            serialPort.Close();





        }
    }
}
