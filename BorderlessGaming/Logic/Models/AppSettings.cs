namespace BorderlessGaming.Logic.Models;

public partial class AppSettings {
    public static AppSettings CreateDefault() {
        return new AppSettings {
            CheckForUpdates = true,
            Culture = BopConstants.DefaultCulture
        };
    }
}