using System;
using System.Reflection;
using System.Windows.Forms;
using BorderlessGaming.Logic.System;

namespace BorderlessGaming.Forms
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutFormLoad(object sender, EventArgs e)
        {
            // removed .Version.ToString(2) in favor of just .ToString() here so we can see the build number now
            versionLabel.Text = "Borderless Gaming " + Assembly.GetExecutingAssembly().GetName().Version;
            _copyrightLabel.Text = "Copyright © 2014-" + DateTime.Now.Year + " Andrew Sampson";
        }

        #region Project and Maintainer Links

        private void OpenBlog(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Tools.GotoSite("http://blog.andrew.im/");
        }

        private void OpenSteamGroup(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Tools.GotoSite("http://steamcommunity.com/app/388080/");
        }

        private void OpenOwnerGithub(object sender, EventArgs e)
        {
            Tools.GotoSite("https://github.com/Codeusa/");
        }

        private void OpenOwnerSteam(object sender, EventArgs e)
        {
            Tools.GotoSite("http://steamcommunity.com/id/Andrewmd5/");
        }

        private void OpenGithubRepo(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Tools.GotoSite("https://github.com/Codeusa/Borderless-Gaming");
        }

        private void _impulserNameTag_Click(object sender, EventArgs e)
        {
            Tools.GotoSite("https://www.indiegogo.com/projects/the-mad-scientist-scholarship/x/3590458");
        }

        #endregion

        #region Contributers

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

        private void OpenMadpewGithub(object sender, EventArgs e)
        {
            Tools.GotoSite("https://github.com/madpew/");
        }

        private void OpenPsouza4Github(object sender, EventArgs e)
        {
            Tools.GotoSite("https://github.com/psouza4/");
        }

        private void OpenPsouza4Steam(object sender, EventArgs e)
        {
            Tools.GotoSite("http://steamcommunity.com/id/psouza4/");
        }

        private void OpenSecretOnlineGithub(object sender, EventArgs e)
        {
            Tools.GotoSite("https://github.com/SecretOnline/");
        }

        #endregion
    }
}