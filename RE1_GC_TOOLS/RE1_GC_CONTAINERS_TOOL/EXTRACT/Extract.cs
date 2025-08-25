using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RE1_GC_CONTAINERS_TOOL.EXTRACT
{
    internal class Extract
    {
        public Extract(FileInfo info, FileFormat fileFormat)
        {
            FileStream stream;
            StreamWriter idx;

            try
            {
                stream = info.OpenRead();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
                return;
            }

            try
            {
                FileInfo idxjInfo = new FileInfo(Path.ChangeExtension(info.FullName, ".idxre1"));
                idx = idxjInfo.CreateText();
            }
            catch (Exception ex)
            {
                stream.Close();
                Console.WriteLine("Error: " + ex);
                return;
            }

            idx.WriteLine("# github.com/JADERLINK");
            idx.WriteLine("# youtube.com/@JADERLINK");
            idx.WriteLine("# RE1 GC CONTAINERS TOOL By JADERLINK");
            idx.WriteLine("TOOL_VERSION:V01");
            switch (fileFormat)
            {
                case FileFormat.DAT:
                    idx?.WriteLine("FILE_FORMAT:DAT");
                    break;
                case FileFormat.EMD:
                    idx?.WriteLine("FILE_FORMAT:EMD");
                    break;
                case FileFormat.EMG:
                    idx?.WriteLine("FILE_FORMAT:EMG");
                    break;
                case FileFormat.IIDAT:
                    idx?.WriteLine("FILE_FORMAT:IIDAT");
                    break;
                case FileFormat.IIEMD:
                    idx?.WriteLine("FILE_FORMAT:IIEMD");
                    break;
                default:
                    idx?.WriteLine("FILE_FORMAT:NULL");
                    break;
            }

            string directory = info.Directory.FullName;
            string baseName = Path.GetFileNameWithoutExtension(info.Name);
            if (baseName.Length == 0)
            {
                baseName = "NULL";
            }

            if (fileFormat == FileFormat.DAT || fileFormat == FileFormat.EMD || fileFormat == FileFormat.EMG || fileFormat == FileFormat.IIEMD || fileFormat == FileFormat.IIDAT)
            {
                try
                {
                    DAT a = new DAT(idx, stream, 0, (uint)info.Length, directory, baseName, fileFormat);

                    //Console
                    Console.WriteLine("COUNT:" + a.Amount);
                    if (a.FilesNames != null)
                    {
                        for (int i = 0; i < a.FilesNames.Length; i++)
                        {
                            Console.WriteLine("FILE_" + i.ToString("D3") + ":" + a.FilesNames[i]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                }
            }

            stream.Close();
            idx.Close();
        }


    }
}
