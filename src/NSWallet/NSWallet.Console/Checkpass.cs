using System;
using NSWallet.Shared;
using CommandLine;

namespace NSWallet.Console
{
    class CheckPass
	{
        private string password;
        private string pathToDatabase;
        public bool CheckPassword()
        {
            if(password!=null&&pathToDatabase!=null)
            {
                if(compareDBVersions()==false)
                {
                    return false;
                }

                BL.InitAPI(pathToDatabase, null);
                bool ok = BL.CheckPassword(password);
                if(ok)
                {
                    System.Console.WriteLine("Correct password\n");
                    return true;
                }
                else
                {
                    System.Console.WriteLine("Wrong password\n");
                    return false;
                }
            }
            else
            {
               System.Console.WriteLine("Error. Usage: 'checkpass --help'");
                return false;
            }
        }
        public bool compareDBVersions()
        {
           bool ok = BL.CheckDBVersion(pathToDatabase);
            if(ok)
            {
                return true;
            }
            else
            {
                System.Console.WriteLine("This version of the database is not supported.");
                return false;
            }
        }
        public CheckPass(string password, string pathToDatabase)
        {
            this.password = password;
            this.pathToDatabase = pathToDatabase;
        }
    }
}