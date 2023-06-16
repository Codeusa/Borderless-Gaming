using CommandLine;

namespace BorderlessGaming.Logic.Misc
{
    public class StartupOptions
    {
        [Option('m', "minimize", HelpText = "Starts the application in a minimized state.")]
        public bool Minimize { get; set; }

        [Option('s', "silent", HelpText = "Starts the application silently.")]
        public bool Silent { get; set; }

        [Option('p', "steam", HelpText = "Used by the Steam client.")]
        public bool IsSteam { get; set; }
    }
}
