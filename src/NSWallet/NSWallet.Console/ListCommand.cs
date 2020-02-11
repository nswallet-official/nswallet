using System;
using System.Collections.Generic;
using NSWallet.Shared;
using CommandLine;

namespace NSWallet.Console
{
    class itemList
	{
        private string password;
        private string pathToDatabase;
        public void showItems()
        {
            CheckPass checkPassCommand = new CheckPass(password, pathToDatabase);
            if(checkPassCommand.CheckPassword())
            {
                var items = BL.GetItems();
                TR.InitTR(GetType().Namespace);
                int fieldPadding;
                foreach(var item in items)
                {
                    if(!item.Folder)
                    {
                        System.Console.WriteLine(item.Name);
                        foreach(var field in item.Fields)
                        {
                            string printField = String.Format("{0}: {1}",field.Label, field.HumanReadableValue);
                            fieldPadding = printField.Length + 4;
                            System.Console.WriteLine(printField.PadLeft(fieldPadding));
                        }
                    }
                }
            }
        }
        public itemList(string password, string path)
        {
            this.pathToDatabase = path;
            this.password = password;
        }
    }
}