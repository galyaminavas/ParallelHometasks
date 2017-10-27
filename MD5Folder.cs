using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Security.Cryptography;

namespace MD5Folder
{
    class Program
    {
        static void ComputeMD5String(List<string> hashList, string name)
        {
            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(name));
            string hashname = BitConverter.ToString(data).Replace("-", String.Empty);
            hashList.Add(hashname);
        }

        static void ComputeMD5File(List<string> hashList, string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                MD5 md5 = MD5.Create();
                byte[] fileData = new byte[fs.Length];
                fs.Read(fileData, 0, (int)fs.Length);
                byte[] checkSum = md5.ComputeHash(fileData);
                string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
                hashList.Add(result);
            }
        }

        static void Main(string[] args)
        {
            string folder;
            if (args.Length < 1)
            {
                Console.WriteLine("No path provided");
                Console.ReadLine();
                return;
            }
            else
            {
                folder = args[0];
            }


            if (Directory.Exists(folder))
            {
                List<string> hashList = new List<string>();
                string folderName = Path.GetFileName(folder);
                ComputeMD5String(hashList, folderName);

                /*foreach (string hash in hashList)
                {
                    Console.WriteLine(hash);
                }
                Console.ReadLine();*/

                DirectoryInfo fld = new DirectoryInfo(folder);
                FileInfo[] files = fld.GetFiles();

                Parallel.ForEach(files, (currFile) => 
                {
                    ComputeMD5File(hashList, currFile.FullName);
                    Console.WriteLine("Processing {0} on thread {1}", currFile, Thread.CurrentThread.ManagedThreadId);
                });

                /*foreach (FileInfo fi in files)
                {
                    ComputeMD5File(hashList, fi.FullName);
                }*/

                /*foreach (string hash in hashList)
                {
                    Console.WriteLine(hash);
                }
                Console.ReadLine();*/

                string concatenatedHashes = string.Concat(hashList);

                /*Console.WriteLine(concatenatedHashes);
                Console.ReadLine();*/

                List<string> finalHash = new List<string>();
                ComputeMD5String(finalHash, concatenatedHashes);
                Console.WriteLine("MD5 of folder: " + folder);
                foreach (string s in finalHash)
                {
                    Console.WriteLine(s);
                }

                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Folder not found");
                Console.ReadLine();
            }            
        }
    }
}
