using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSWallet
{
    public interface IFile
    {
        string GetInternalFilePath();
		string GetInternalDirPath();
		string CheckOldFile();
        string GetOldDBDirectory();
        string GetBackupPath();
        IEnumerable<string> GetFilePaths(string path);
        IEnumerable<string> GetFileNames(string path);
        long GetFileSize(string path);
		bool FileExists(string path);
		bool DirectoryExists(string path);
		void RemoveFile(string path);
        void CreateFile(string path);
        void WriteInFile(string path, string content);
        void MoveFile(string pathFrom, string pathTo);
        string ReadFromFile(string path);
        List<string> ReadZip(string path);
		bool Unzip(string pathFrom, string pathTo);
        bool CreateZip(string pathSourceFolder, string pathDestinationFolder, string fileName);
        byte[] GetBytesFromFile(string path);
		void CopyFile(string srcFilename, string destFilename);
        bool RemoveDirectoryWithContents(string path);
        string GetTempFolder();
        void CreateFolder(string path);
    }
}

