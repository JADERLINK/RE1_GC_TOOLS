using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RE1_GC_CONTAINERS_TOOL
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("# RE1_GC_CONTAINERS_TOOL");
            Console.WriteLine("# By: JADERLINK");
            Console.WriteLine("# youtube.com/@JADERLINK");
            Console.WriteLine("# github.com/JADERLINK");
            Console.WriteLine("# VERSION 1.0.0 (2025-08-24)");


            bool usingBatFile = false;
            int start = 0;
            if (args.Length > 0 && args[0].ToLowerInvariant() == "-bat")
            {
                usingBatFile = true;
                start = 1;
            }

            for (int i = start; i < args.Length; i++)
            {
                if (File.Exists(args[i]))
                {
                    try
                    {
                        Continue(args[i]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + args[i]);
                        Console.WriteLine(ex);
                    }
                }
                else
                {
                    Console.WriteLine("File specified does not exist: " + args[i]);
                }

            }

            if (args.Length == 0)
            {
                Console.WriteLine("How to use: drag the file to the executable.");
                Console.WriteLine("For more information read:");
                Console.WriteLine("https://github.com/JADERLINK/RE1_GC_TOOLS");
                Console.WriteLine("Press any key to close the console.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Finished!!!");
                if (!usingBatFile)
                {
                    Console.WriteLine("Press any key to close the console.");
                    Console.ReadKey();
                }
            }

        }

        private static void Continue(string file)
        {
            var fileInfo = new FileInfo(file);
            Console.WriteLine();
            Console.WriteLine("File: " + fileInfo.Name);
            var Extension = fileInfo.Extension.ToUpperInvariant();

            if (Extension == ".DAT" || Extension == ".EMD" || Extension == ".EMG" || Extension == ".IIDAT" || Extension == ".IIEMD")
            {
                Console.WriteLine("Extract Mode!");

                EXTRACT.FileFormat fileFormat = EXTRACT.FileFormat.Null;
                switch (Extension)
                {
                    case ".DAT": fileFormat = EXTRACT.FileFormat.DAT; break;
                    case ".EMD": fileFormat = EXTRACT.FileFormat.EMD; break;
                    case ".EMG": fileFormat = EXTRACT.FileFormat.EMG; break;
                    case ".IIDAT": fileFormat = EXTRACT.FileFormat.IIDAT; break;
                    case ".IIEMD": fileFormat = EXTRACT.FileFormat.IIEMD; break;
                }

                if (fileFormat != EXTRACT.FileFormat.Null)
                {
                    _ = new EXTRACT.Extract(fileInfo, fileFormat);
                }
            }
            else if (Extension == ".IDXRE1")
            {
                Console.WriteLine("Repack Mode!");
                Console.WriteLine("function not implemented!");
                //_ = new REPACK.RepackIdx(fileInfo);
            }
            else
            {
                Console.WriteLine("The extension is not valid: " + Extension);
            }
        }

    }
}
