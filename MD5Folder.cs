using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace MD5Folder
{
    class Program
    {
        static string ComputeMD5String(string s)
        {
            MD5 md5 = MD5.Create();
            byte[] data = md5.ComputeHash(Encoding.Default.GetBytes(s));
            string hashStr = BitConverter.ToString(data).Replace("-", String.Empty);

            return hashStr;
        }

        static string ComputeMD5File(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                MD5 md5 = MD5.Create();
                string filename = Path.GetFileName(path);
                byte[] fileData = new byte[fs.Length];
                fs.Read(fileData, 0, (int)fs.Length);
                byte[] checkSum = md5.ComputeHash(fileData);
                string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
                
                return ComputeMD5String(filename + result);
            }
        }

        static string ComputeMD5Folder(string path)
        {
            string folderName = Path.GetFileName(path);
            StringBuilder result = new StringBuilder();
            result.Append(folderName);

            DirectoryInfo currFolder = new DirectoryInfo(path);
            FileInfo[] files = currFolder.GetFiles();
            DirectoryInfo[] folders = currFolder.GetDirectories();

            List<Task<string>> filesTasks = new List<Task<string>>();
            foreach (FileInfo f in files)
            {
                Task<string> fileTask = Task.Run(() => ComputeMD5File(f.FullName));
                filesTasks.Add(fileTask);
            }
            Task.WaitAll(filesTasks.ToArray());
            foreach(var t in filesTasks)
            {
                result.Append(t.Result);
            }

            List<Task<string>> foldersTasks = new List<Task<string>>();
            foreach (DirectoryInfo f in folders)
            {
                Task<string> folderTask = Task.Run(() => ComputeMD5Folder(f.FullName));
                foldersTasks.Add(folderTask);
            }
            Task.WaitAll(foldersTasks.ToArray());
            foreach (var t in foldersTasks)
            {
                result.Append(t.Result);
            }

            return ComputeMD5String(result.ToString());
        }

        //let hash = md5(name + md5(content))

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
                string hash = ComputeMD5Folder(folder);
                Console.WriteLine("MD5 of folder: " + folder);
                Console.WriteLine(hash);    
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
