using System;
using System.Collections.Generic;
using NSWallet.Shared;
using CommandLine;

namespace NSWallet.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            const string startMessage  = "NSWallet, supported database version: {0}";
            System.Console.WriteLine(String.Format(startMessage,GConsts.DB_VERSION));
  
            Parser.Default.ParseArguments<Options,Options.CheckpassOptions,Options.ShowItems>(args)
                   .WithParsed<Options.CheckpassOptions>(pasVerification =>
                   {
                       try
                       {
                            CheckPass checkPassCommand = new CheckPass(pasVerification.password, pasVerification.pathToDatabase);
                            checkPassCommand.CheckPassword();
                       }
                       catch(Exception)
                       { 
                                
                       }
                    })
                   .WithParsed<Options.ShowItems>(showList =>
                   {
                            itemList listCommand = new itemList(showList.password, showList.pathToDatabase);
                            listCommand.showItems();
                   });
        }
    }
}