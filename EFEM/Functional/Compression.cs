using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.IO.Compression;

namespace FrameOfSystem3.Functional
{
    /// <summary>
    /// 2021.05.25 jhlim : 파일 및 폴더 압축 및 해제 기능을 하는 클래스
    /// </summary>
	class Compression
    {
        public static void Compress(string strSourceDirPath, string strZipFilePath)
        {
            // 같음 이름의 파일이 존재하면 제거한다.
            if (File.Exists(strZipFilePath) == true)
                File.Delete(strZipFilePath);

            using (FileStream fileStream = new FileStream(strZipFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                using (ZipArchive zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Create))
                {
                    foreach (string filePath in Directory.EnumerateFileSystemEntries(strSourceDirPath, "*.*", SearchOption.AllDirectories))
                    {
                        string relativePath = filePath.Replace(strSourceDirPath, "");

                        try
                        {
                            FileAttributes attr = File.GetAttributes(filePath);
                            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                            {
                                zipArchive.CreateEntry(relativePath + @"\");
                            }
                            else
                            {
                                zipArchive.CreateEntryFromFile(filePath, relativePath);
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }

        public static void Extract(string strZipFilePath, string strDestDirPath)
        {
            using (ZipArchive zipArchive = System.IO.Compression.ZipFile.OpenRead(strZipFilePath))
            {
                foreach (ZipArchiveEntry zipArchiveEntry in zipArchive.Entries)
                {
                    try
                    {
                        string folderPath = Path.GetDirectoryName(Path.Combine(strDestDirPath, zipArchiveEntry.FullName));

                        if (!Directory.Exists(folderPath))
                        {
                            Directory.CreateDirectory(folderPath);
                        }

                        string fileName = Path.GetFileName(Path.Combine(strDestDirPath, zipArchiveEntry.FullName));
                        if (String.IsNullOrEmpty(fileName) == false)
                        {
                            zipArchiveEntry.ExtractToFile(Path.Combine(strDestDirPath, zipArchiveEntry.FullName), true);
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

    }
}
