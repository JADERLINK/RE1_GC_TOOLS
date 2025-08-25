using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RE1_GC_BIN_TOOL
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Globalization.CultureInfo.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            Console.WriteLine(Shared.HeaderText());

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
                        FileInfo fileInfo1 = new FileInfo(args[i]);
                        string file1Extension = fileInfo1.Extension.ToUpperInvariant();
                        Console.WriteLine("File: " + fileInfo1.Name);
                        ContinueActions(fileInfo1, file1Extension);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error: " + Environment.NewLine + ex);
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
                Console.WriteLine("Finished!!!");
                if (!usingBatFile)
                {
                    Console.WriteLine("Press any key to close the console.");
                    Console.ReadKey();
                }
            }

        }

        private static void ContinueActions(FileInfo fileInfo1, string file1Extension)
        {
            string baseDirectory = fileInfo1.DirectoryName;
            string baseName = Path.GetFileNameWithoutExtension(fileInfo1.Name);

            Stream binFile;

            switch (file1Extension)
            {
                case ".BIN":
                    binFile = fileInfo1.OpenRead();
                    break;
                default:
                    Console.WriteLine("The file format is invalid: " + fileInfo1.Name);
                    return;
            }

            //-----------
            //carrega os objetos arquivos.

            EXTRACT.RE1GCBIN BIN = null;
            ALL.IdxMaterial material = null;

            if (binFile != null) //.BIN
            {
                BIN = EXTRACT.Re1GcBinDecoder.Decoder(binFile, 0, out _);
                material = ALL.IdxMaterialParser.Parser(BIN);
                binFile.Close();
            }

            if (file1Extension == ".BIN") // modo extract
            {
                EXTRACT.OutputFiles.CreateSMD(BIN, baseDirectory, baseName);
                EXTRACT.OutputFiles.CreateOBJ(BIN, baseDirectory, baseName);
                EXTRACT.OutputMaterial.CreateIdxMaterial(material, baseDirectory, baseName);

                var _idxMtl = ALL.IdxMtlParser.Parser(material, baseName);
                EXTRACT.OutputMaterial.CreateMTL(_idxMtl, baseDirectory, baseName);
            }
        }
    }
}
