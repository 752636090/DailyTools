using System.Collections;
using System.IO;
using System;

namespace Common
{
    public class IOUtil
    {
        /// <summary>
        /// 读取文本文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetFileText(string filePath)
        {
            string content = string.Empty;

            if (!File.Exists(filePath))
            {
                return content;
            }

            using (StreamReader sr = File.OpenText(filePath))
            {
                content = sr.ReadToEnd();
            }
            return content;
        }

        public static string[] GetFileTextArr(FileInfo file)
        {
            return GetFileTextArr(GetFileText(file.FullName));
        }

        public static string[] GetFileTextArr(string text)
        {
            return text.Replace("\r\n", "\n").Split("\n");
        }

        #region CreateTextFile 创建文本文件
        /// <summary>
        /// 创建文本文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        public static void CreateTextFile(string filePath, string content)
        {
            string directory = filePath.Substring(0, filePath.LastIndexOf("\\"));
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            DeleteFile(filePath);

            using (FileStream fs = File.Create(filePath))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(content.ToString());
                }
            }
        }
        #endregion

        #region DeleteFile 删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        #endregion

        #region CopyDirectory 拷贝文件夹
        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        public static void CopyDirectory(string sourceDirName, string destDirName)
        {
            try
            {
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                    File.SetAttributes(destDirName, File.GetAttributes(sourceDirName));

                }

                if (destDirName[destDirName.Length - 1] != Path.DirectorySeparatorChar)
                    destDirName = destDirName + Path.DirectorySeparatorChar;

                string[] files = Directory.GetFiles(sourceDirName);
                foreach (string file in files)
                {
                    if (File.Exists(destDirName + Path.GetFileName(file)))
                        continue;
                    FileInfo fileInfo = new FileInfo(file);
                    if (fileInfo.Extension.Equals(".meta", StringComparison.CurrentCultureIgnoreCase)
                        || fileInfo.Extension.Equals(".manifest", StringComparison.CurrentCultureIgnoreCase)
                        )
                        continue;

                    File.Copy(file, destDirName + Path.GetFileName(file), true);
                    File.SetAttributes(destDirName + Path.GetFileName(file), FileAttributes.Normal);
                }

                string[] dirs = Directory.GetDirectories(sourceDirName);
                foreach (string dir in dirs)
                {
                    CopyDirectory(dir, destDirName + Path.GetFileName(dir));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetFileBuffer 读取本地文件到byte数组
        /// <summary>
        /// 读取本地文件到byte数组
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static byte[] GetFileBuffer(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            byte[] buffer = null;

            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
            }
            return buffer;
        }
        #endregion

        public static bool IsFileInUse(string fileName)
        {
            bool inUse = true;

            FileStream fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                inUse = false;
            }
            catch
            {
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return inUse;//true表示正在使用,false没有使用  
        }

        public static void UnsetReadOnly(string folderPath)
        {
            string[] folderPathes = Directory.GetDirectories(folderPath, "*.*", SearchOption.AllDirectories);
            string[] filePathes = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories);
            foreach (var dirPath in folderPathes)
            {
                DirectoryInfo dir = new(dirPath);
                dir.Attributes = FileAttributes.Normal & FileAttributes.Directory;
            }
            foreach (var filePath in filePathes)
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }
        }
    }
}