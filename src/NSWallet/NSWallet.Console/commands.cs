using System;
using NSWallet.Shared;
using CommandLine;

namespace NSWallet.Console
{
        public class Options
        {            

                [Verb("checkpass", HelpText = "Password verification.")]
                public class CheckpassOptions 
                {
                    [Option('p', HelpText = "Password.")]
                    public string password { get; set; } =null;
                    
                    [Option('d', HelpText = "Path to database.")]
                    public string pathToDatabase { get; set; } = null;
                }
            
                [Verb("list", HelpText = "Show items.")]
                public class ShowItems 
                {
                    [Option('p', HelpText = "Password.")]
                    public string password { get; set; } =null;
                    
                    [Option('d', HelpText = "Path to database.")]
                    public string pathToDatabase { get; set; } = null;

                    [Option("index", HelpText = "Find item by index.")]
                    public string index { get; set; } = null;
                }
        }
}