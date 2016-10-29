using System;
using System.Collections.Generic;
using System.IO;

namespace CipherManager.Core
{
    public class FileManager
    {
        private static FileManager instance;

        private static readonly object locker = new object();

        public string WorkDirectory
        {
            get;
            private set;

        }

        public string DataDirectory
        {
            get
            {
                return Path.Combine(this.WorkDirectory, "data");
            }
        }

        public List<CipherFile> CipherFileList
        {
            get;
            private set;
        }

        private FileManager()
        {
            this.WorkDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(this.DataDirectory))
            {
                Directory.CreateDirectory(this.DataDirectory);
            }
            this.CipherFileList = new List<CipherFile>();
            this.LoadAllCipherFile();
        }

        public static FileManager GetInstance()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new FileManager();
                    }
                }
            }
            return instance;
        }

        private void LoadAllCipherFile()
        {
            string[] files = Directory.GetFiles(this.DataDirectory);
            string[] array = files;
            for (int i = 0; i < array.Length; i++)
            {
                string fileName = array[i];
                CipherFile cipherFile = new CipherFile(fileName);
                if (cipherFile.CheckIdentification())
                {
                    this.CipherFileList.Add(cipherFile);
                }
            }
        }
    }
}
