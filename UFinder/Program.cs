using System;
using System.Collections.Generic;
using System.IO;

namespace UFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> paths = new List<string>();
            List<Pool> pool = null;
            bool keepCopy = false;
            bool stopFirst = false;
            bool ignoreCase = false;

            if (args.Length == 0)
            {
                printHelp();
                Environment.Exit(-1);
                return;
            }


            try
            {
                for (int i = 0; i < args.Length; i++)
                {
                    var arg = args[i];

                    switch (arg)
                    {
                        case "-File":
                        case "-F":
                            HandlePath(ref paths, ref i, args);
                            break;

                        case "-Directory":
                        case "-D":
                            HandleDirectory(ref paths, ref i, args);
                            break;

                        case "-Template-Path":
                        case "-T":
                            HandleTemplate(ref pool, ref i, args);
                            break;

                        case "--Ignore-Case":
                        case "--IC":
                            ignoreCase = true;
                            break;

                        case "--Keep-Copy":
                        case "--KC":
                            keepCopy = true;
                            break;

                        default:
                            printHelp();
                            Environment.Exit(-1);
                            return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(-1);
                return;
            }

            paths.ForEach((path) =>
            {
                UFinder finder = UFinder
                    .NewBuilder(path, pool)
                    .CaseSensitive(ignoreCase)
                    .KeepCopy(keepCopy)
                    .Build();

                finder.FindAndReplace();
            });
        }

        private static void printHelp()
        {
            Console.WriteLine("UFinder.exe <options>");
            Console.WriteLine("  -F, -File <path to file> ................. Indicates the path for the target file");
            Console.WriteLine("  -D, -Directory <path to directoy> ........ All files in said directory will be targeted");
            Console.WriteLine("  -Template-Path <path to template> ........ Path to template");
            Console.WriteLine("  --IC, --Ignore-Case ...................... (Optional) Search will be not case be case sensitive");
            Console.WriteLine("  --KC, --Keep-Copy <path to directoy> ..... (Optional) Maintains a copy of the file");
            
        }


        private static void HandleTemplate(ref List<Pool> pool, ref int i, string[] args)
        {
            string template = args[++i];
    
            pool = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Pool>>(File.ReadAllText(template));
        }

        private static void HandleDirectory(ref List<string> paths, ref int i, string[] args)
        {
            string path = args[++i];

            if (!Directory.Exists(path))
            {
                throw new Exception("Directory " + path + " does not exist");
            }
            paths.AddRange(Directory.EnumerateFiles(path));

        }

        private static void HandlePath(ref List<string> paths, ref int i, string[] args)
        {

            string path = args[++i];

            if (!File.Exists(path))
            {
                throw new Exception("File " + path + " does not exist");
            }
            paths.Add(path);

        }
    }
}