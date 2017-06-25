using CommandLine;
using CommandLine.Text;

namespace BorderlessGaming.Logic.System
{
    public class StartupOptions
    {
        [Option('m', "minimize", DefaultValue = false, HelpText = "Starts the application in a minimized state.")]
        public bool Minimize { get; set; }

        [Option('s', "silent", DefaultValue = false, HelpText = "Starts the application silently.")]
        public bool Silent { get; set; }

        [Option('p', "steam", DefaultValue = false, HelpText = "Used by the Steam client.")]
        public bool IsSteam { get; set; }


        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
