using System;
using System.Threading.Tasks;

namespace WebPagesDownload
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Enter web page address");
            //string address = Console.ReadLine();

            string address = "http://www.natel.ru/links";
            //string address = "https://www.topuniversities.com/blog/33-useful-websites-students";
            //string address = "http://google.com";
            //string address = "https://www.labnol.org/internet/101-useful-websites/18078/";

            Downloader dwnldr = new Downloader();
            Task task = dwnldr.LinksInfo(address);
            task.Wait();

            Console.ReadKey();
        }
    }
}
