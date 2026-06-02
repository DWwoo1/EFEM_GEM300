using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace ScenarioLogger
{
    public sealed class LogArchiveManager
    {
        public LogArchiveResult ArchiveOldLogs(
            string rootPath,
            DateTime now,
            int archiveDays)
        {
            LogArchiveResult result = new LogArchiveResult();

            if (string.IsNullOrWhiteSpace(rootPath))
                return result;

            if (false == Directory.Exists(rootPath))
                return result;

            DateTime cutoffDate = now.Date.AddDays(-archiveDays);

            IEnumerable<string> filePaths;
            try
            {
                filePaths = Directory.EnumerateFiles(
                    rootPath,
                    "*.txt",
                    SearchOption.AllDirectories);
            }
            catch (Exception ex)
            {
                result.Errors.Add(string.Format("EnumerateFiles failed: {0}", ex.Message));
                return result;
            }

            foreach (string filePath in filePaths)
            {
                try
                {
                    DateTime logDate;
                    if (false == TryGetLogDateFromPath(rootPath, filePath, out logDate))
                    {
                        result.Skipped++;
                        continue;
                    }

                    // 폴더 날짜 기준으로 오늘 기준 이틀 전까지 대상
                    if (logDate.Date > cutoffDate)
                        continue;

                    string zipPath = filePath + ".zip";
                    string tempZipPath = zipPath + ".tmp";

                    if (File.Exists(zipPath))
                    {
                        result.Skipped++;
                        continue;
                    }

                    TryDelete(tempZipPath);

                    using (ZipArchive archive = ZipFile.Open(tempZipPath, ZipArchiveMode.Create))
                    {
                        archive.CreateEntryFromFile(
                            filePath,
                            Path.GetFileName(filePath),
                            CompressionLevel.Optimal);
                    }

                    File.Move(tempZipPath, zipPath);
                    File.Delete(filePath);

                    result.Archived++;
                }
                catch (IOException)
                {
                    result.Skipped++;
                }
                catch (UnauthorizedAccessException)
                {
                    result.Skipped++;
                }
                catch (Exception ex)
                {
                    result.Skipped++;
                    result.Errors.Add(string.Format("{0} : {1}", filePath, ex.Message));
                }
            }

            return result;
        }

        private void TryDelete(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch
            {
            }
        }
        private bool TryGetLogDateFromPath(string rootPath, string filePath, out DateTime logDate)
        {
            logDate = DateTime.MinValue;

            if (string.IsNullOrWhiteSpace(rootPath) || string.IsNullOrWhiteSpace(filePath))
                return false;

            string relativePath;
            try
            {
                Uri rootUri = new Uri(AppendDirectorySeparator(rootPath));
                Uri fileUri = new Uri(filePath);
                relativePath = Uri.UnescapeDataString(
                    rootUri.MakeRelativeUri(fileUri).ToString())
                    .Replace('/', Path.DirectorySeparatorChar);
            }
            catch
            {
                return false;
            }

            string[] parts = relativePath.Split(Path.DirectorySeparatorChar);

            // 기대 경로 예:
            // 2026\04\11\Log.txt
            // Terminal\2026\04\11\Log.txt
            // Scenario\2026\04\11\Log.txt

            int yearIndex = -1;
            for (int i = 0; i <= parts.Length - 4; i++)
            {
                int year;
                int month;
                int day;

                if (int.TryParse(parts[i], out year) &&
                    int.TryParse(parts[i + 1], out month) &&
                    int.TryParse(parts[i + 2], out day))
                {
                    yearIndex = i;
                    break;
                }
            }

            if (yearIndex < 0)
                return false;

            int parsedYear = int.Parse(parts[yearIndex]);
            int parsedMonth = int.Parse(parts[yearIndex + 1]);
            int parsedDay = int.Parse(parts[yearIndex + 2]);

            try
            {
                logDate = new DateTime(parsedYear, parsedMonth, parsedDay);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string AppendDirectorySeparator(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return path;

            if (path.EndsWith(Path.DirectorySeparatorChar.ToString()))
                return path;

            return path + Path.DirectorySeparatorChar;
        }
    }

    public sealed class LogArchiveResult
    {
        public int Archived { get; set; }
        public int Skipped { get; set; }
        public List<string> Errors { get; private set; } = new List<string>();
    }
}