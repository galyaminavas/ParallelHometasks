using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebPagesDownload
{
    class Program
    {
        public static void LinksInfo(string addr)
        {
            Console.WriteLine("Web page address: " + addr);
            Console.WriteLine();

            Downloader dwnldr = new Downloader();
            var mainPage = dwnldr.DownloadPage(addr);
            var innerPages = dwnldr.FindLinks(mainPage.Result);
            Console.WriteLine("Wait for it...\n");
            foreach (var p in innerPages)
            {
                var len = dwnldr.DownloadPage(p).Result.Length;
                Console.WriteLine("Page: " + p + " -- Size: " + len);
            }
            Console.WriteLine();
            Console.WriteLine("That's all.");
        }

        static void Main(string[] args)
        {
            //Console.WriteLine("Enter web page address");
            //string address = Console.ReadLine();

            string address = "http://www.natel.ru/links";
            //string address = "https://www.topuniversities.com/blog/33-useful-websites-students";
            //string address = "http://google.com";
            //string address = "https://www.labnol.org/internet/101-useful-websites/18078/";

            LinksInfo(address);

            Console.ReadKey();
        }
    }
}
