using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SimpleEndianBinaryIO;

namespace RE1_GC_CONTAINERS_TOOL.EXTRACT
{
    internal class DAT
    {
        public int Amount = 0;
        public string[] FilesNames = null;

        public DAT(StreamWriter idx, Stream readStream, uint offsetStart, uint fullLength, string directory, string baseName, FileFormat fileFormat)
        {
            List<uint> offsets = new List<uint>();
            Dictionary<uint, List<int>> repeatedOffsets = new Dictionary<uint, List<int>>();

            EndianBinaryReader br = new EndianBinaryReader(readStream, Endianness.BigEndian);
            br.Position = offsetStart;

            uint first = br.ReadUInt32();

            if (first == 0x00_20_AF_30)
            {
                Console.WriteLine("This file can be a TPL file!");
                return;
            }

            uint smallestOffset = first != 0 ? first : uint.MaxValue;

            br.Position = offsetStart;

            if (fileFormat == FileFormat.IIDAT) // diferente
            {
                int i = 0;
                while (true)
                {
                    ushort ID = br.ReadUInt16();
                    if (ID == 0xFFFF) // final
                    {
                        break;
                    }

                    byte type = br.ReadByte();
                    byte unkV = br.ReadByte();

                    string line = $"INFO_{i:D3}:{ID:X4}!{type:X2}!{unkV:X2}";
                    idx.WriteLine(line);

                    offsets.Add(0);

                    int forCount = 0;
                    i++;

                    if (type == 0)
                    {
                        forCount = 3;
                    }
                    else
                    {
                        forCount = 1;
                    }

                    for (int k = 0; k < forCount; k++)
                    {
                        uint tempOffset = br.ReadUInt32();

                        offsets.Add(tempOffset);

                        if (repeatedOffsets.ContainsKey(tempOffset))
                        {
                            repeatedOffsets[tempOffset].Add(i);
                        }
                        else
                        {
                            repeatedOffsets.Add(tempOffset, new List<int> {i});
                        }

                        if (tempOffset != 0 && tempOffset < smallestOffset)
                        {
                            smallestOffset = tempOffset;
                        }

                        i++;
                    }

                    if (br.Position >= smallestOffset + offsetStart)
                    {
                        break;
                    }

                    if (br.Position >= fullLength - 8)
                    {
                        Console.WriteLine("Error extracting file.");
                        return;
                    }
                }              
            }
            else // outros
            {
                int i = 0;
                while (true)
                {
                    uint tempOffset = br.ReadUInt32();

                    offsets.Add(tempOffset);

                    if (repeatedOffsets.ContainsKey(tempOffset))
                    {
                        repeatedOffsets[tempOffset].Add(i);
                    }
                    else
                    {
                        repeatedOffsets.Add(tempOffset, new List<int> { i });
                    }

                    if (tempOffset != 0 && tempOffset < smallestOffset)
                    {
                        smallestOffset = tempOffset;
                    }

                    i++;
                    if (br.Position >= smallestOffset + offsetStart)
                    {
                        break;
                    }

                    if (br.Position >= fullLength - 8)
                    {
                        Console.WriteLine("Error extracting file.");
                        return;
                    }
                }
            }
           

            if (!Directory.Exists(Path.Combine(directory, baseName)))
            {
                try
                {
                    Directory.CreateDirectory(Path.Combine(directory, baseName));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to create directory: " + Path.Combine(directory, baseName));
                    Console.WriteLine(ex);
                    return;
                }
            }

            int EndFileOffsetId = -1; // se o ultimo offset é o final do arquivo

            while (true) // remove os ultimos offsets que estão zerados
            {
                if (offsets.Count == 0)
                {
                    break;
                }

                if (offsets[offsets.Count -1] == 0)
                {
                    offsets.RemoveAt(offsets.Count - 1);
                }
                else if (offsets[offsets.Count - 1] == fullLength)
                {
                    EndFileOffsetId = offsets.Count - 1;
                    break;
                }
                else 
                {
                    break;
                }
            }


            Amount = offsets.Count; // count
            FilesNames = new string[Amount];
            idx.WriteLine("COUNT:" + Amount);

            List<uint> ordenedOffsets = new List<uint>();
            ordenedOffsets.AddRange(offsets);
            ordenedOffsets.Add(fullLength);
            ordenedOffsets = ordenedOffsets.OrderByDescending(x => x).ToList();

            for (int j = 0; j < offsets.Count; j++)
            {
                uint myOffset = offsets[j];
                if (myOffset > fullLength)
                {
                    myOffset = 0;
                }
                uint nextOfset = myOffset; // Inicialmente define o mesmo offset. Daí o 'length' fica 0;

                if (myOffset != 0)
                {
                    foreach (var item in ordenedOffsets)
                    {
                        if (item > myOffset)
                        {
                            nextOfset = item;
                        }
                    }
                }
      
                // conteudo do arquivo
                int subFileLength = (int)(nextOfset - myOffset);
                readStream.Position = offsetStart + myOffset;

                int nameId = -1;
                if (myOffset != 0 && repeatedOffsets.ContainsKey(myOffset) && repeatedOffsets[myOffset].Count > 0)
                {
                    nameId = repeatedOffsets[myOffset][0];
                }

                string Name = Path.Combine(baseName, baseName + "_" + nameId.ToString("D3"));
                if (nameId == -1)
                {
                    Name = Path.Combine(baseName, baseName + "_EMPTY");
                }

                if (subFileLength > 0 && nameId == j)
                {
                    byte[] endfile = new byte[subFileLength];
                    readStream.Read(endfile, 0, subFileLength);

                    uint magic = EndianBitConverter.ToUInt32(endfile, 0, Endianness.BigEndian);
                    if (magic == 0x00_00_00_60 || magic == 0x00_00_00_40) // BIN
                    {
                        Name += ".BIN";
                    }
                    else if (magic == 0x00_20_AF_30) // TPL
                    {
                        Name += ".TPL";
                    }
                    else if (j == 10 && (fileFormat == FileFormat.DAT || fileFormat == FileFormat.IIEMD))
                    {
                        Name += ".IIDAT";
                    }
                    else if (j == 1 && fileFormat == FileFormat.EMD)
                    {
                        Name += ".IIEMD";
                    }
                    else
                    {
                        Name += ".UNK";
                    }

                    try
                    {
                        File.WriteAllBytes(Path.Combine(directory, Name), endfile);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(Name + ": " + ex);
                    }
                }

                string Line = "FILE_" + j.ToString("D3") + ":" + Name;
                idx.WriteLine(Line);
                FilesNames[j] = Name;
            }

            if (EndFileOffsetId != -1)
            {
                idx.WriteLine("ENDFILEID:" + EndFileOffsetId.ToString("D3"));
            }

            //end
        }
    
    }
}
