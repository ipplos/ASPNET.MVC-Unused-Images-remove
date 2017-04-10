using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindUnusedImages
{
    class Program
    {
        private static List<string> NotFoundFiles;
        static void Main(string[] args)
        {
            String[] Files = Directory.GetFiles(args[0]);
            NotFoundFiles = new List<string>(Files);

            for (int i = 0; i < Files.Count(); i++)
            {
                Console.Write(Files[i]);
                DirSearch(args[1], Files[i], args[2].Split(new String[] {"|"},StringSplitOptions.RemoveEmptyEntries).ToList());
                if (NotFoundFiles.Contains(Files[i]))
                {
                    Console.WriteLine("NOT FOUND");
                }
                else
                    Console.WriteLine();
            }
            Console.WriteLine("============================");
            foreach (var file in NotFoundFiles)
            {
                Console.WriteLine(file);
                Directory.Move(file, args[3]+Path.GetFileName(file));
            }
            Console.ReadKey();
        }

        static void DirSearch(string sDir,string filename,List<string> exclude)
        {
            try
            {
                foreach (string d in Directory.GetDirectories(sDir))
                {
                    if (exclude.Count(x=>d.IndexOf(x)!=-1)==0)
                    {
                        foreach (string f in Directory.GetFiles(d))
                        {
                            if (Path.GetExtension(f) == ".cshtml" || Path.GetExtension(f) == ".js" || Path.GetExtension(f) == ".less" || Path.GetExtension(f) == ".scss" || Path.GetExtension(f) == ".css" || Path.GetExtension(f) == ".ts" || Path.GetExtension(f) == ".cs")
                            {
                                string nameToSearch = Path.GetFileName(filename);
                                StreamReader Reader = new StreamReader(f);
                                String Contents = Reader.ReadToEnd();
                                if (Contents.IndexOf(nameToSearch) != -1)
                                {
                                    Console.WriteLine("\tFound in:" + f);
                                    NotFoundFiles.Remove(filename);
                                }
                                Reader.Close();
                            }
                        }
                    }
                    DirSearch(d, filename, exclude);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }
    }
}
