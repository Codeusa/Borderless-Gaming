using System;
using System.Windows.Forms;
using BorderlessGaming.Utilities;

namespace BorderlessGaming.Forms
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            CenterToScreen();
            InitializeComponent();
        }


        private void OpenBlog(object sender, LinkLabelLinkClickedEventArgs e) 
        {
            Tools.GotoSite("http://andrew.codeusa.net/");
        }

        private void OpenSteamGroup(object sender, LinkLabelLinkClickedEventArgs e) 
        {
            Tools.GotoSite("http://steamcommunity.com/groups/borderless-gaming/");
        }

        private void OpenOwnerGithub(object sender, EventArgs e)
        {
            Tools.GotoSite("https://github.com/Codeusa/");
        }

        private void OpenOwnerSteam(object sender, EventArgs e)
        {
            Tools.GotoSite("http://steamcommunity.com/id/deathstrokee/");
        }

        private void OpenDmxtGithub(object sender, EventArgs e)
        {
            Tools.GotoSite("https://github.com/dmxt/");
        }

        private void OpenImpulserGithub(object sender, EventArgs e)
        {
            Tools.GotoSite("https://github.com/Impulser/");
        }

        private void OpenStackOfPancakesGithub(object sender, EventArgs e)
        {
            Tools.GotoSite("https://github.com/Stack-of-Pancakes/");
        }

        private void OpenGithubRepo(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Tools.GotoSite("https://github.com/Codeusa/Borderless-Gaming");
        }
    }
}