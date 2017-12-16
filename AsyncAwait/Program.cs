using System;
using System.Threading.Tasks;

namespace WebPagesDownload
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter web page address");
            string address = Console.ReadLine();

            Downloader dwnldr = new Downloader();
            Task task = dwnldr.LinksInfo(address);
            task.Wait();

            Console.ReadKey();
        }
    }
}
