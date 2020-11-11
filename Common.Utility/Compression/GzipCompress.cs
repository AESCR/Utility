using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Common.Utility.Compression
{
    public static class GzipCompress
    {
        public static Encoding ZipArchivEncoding = Encoding.Default;

        /// <summary>
        /// 压缩归档为一个包
        /// </summary>
        /// <param name="files"> </param>
        /// <returns> </returns>
        public static byte[] Compress(IList<KeyValuePair<string, byte[]>> files)
        {
            byte[] resultBytes;
            using (MemoryStream compressedStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(compressedStream, ZipArchiveMode.Create, true, ZipArchivEncoding))
                {
                    for (int i = 0; i < files.Count; i++)
                    {
                        var kv = files[i];
                        var bytes = kv.Value;
                        var readmeEntry = archive.CreateEntry(kv.Key);
                        var zipA = readmeEntry.Open();
                        zipA.Write(bytes, 0, bytes.Length);
                        zipA.Flush();
                        zipA.Close();
                    }
                }
                // 设置当前流的位置为流的开始 流的关闭顺序非常重要
                compressedStream.Seek(0, SeekOrigin.Begin);
                resultBytes = new byte[compressedStream.Length];
                compressedStream.Read(resultBytes, 0, resultBytes.Length);
                compressedStream.Close();
                compressedStream.Dispose();
            }
            return resultBytes;
        }

        /// <summary>
        /// 压缩归档为一个包
        /// </summary>
        /// <param name="files"> </param>
        /// <returns> </returns>
        public static byte[] Compress(Dictionary<string, byte[]> files)
        {
            byte[] resultBytes;
            using (MemoryStream compressedStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(compressedStream, ZipArchiveMode.Create, true, ZipArchivEncoding))
                {
                    foreach (string key in files.Keys)
                    {
                        var kv = files[key];
                        var bytes = kv;
                        var readmeEntry = archive.CreateEntry(key);
                        var zipA = readmeEntry.Open();
                        zipA.Write(bytes, 0, bytes.Length);
                        zipA.Flush();
                        zipA.Close();
                    }
                }
                // 设置当前流的位置为流的开始 流的关闭顺序非常重要
                compressedStream.Seek(0, SeekOrigin.Begin);
                resultBytes = new byte[compressedStream.Length];
                compressedStream.Read(resultBytes, 0, resultBytes.Length);
                compressedStream.Close();
                compressedStream.Dispose();
            }
            return resultBytes;
        }
    }
}