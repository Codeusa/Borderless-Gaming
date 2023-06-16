namespace BorderlessGaming.Logic.Models;

public partial class AppSettings {
    public static AppSettings CreateDefault() {
        return new AppSettings {
            Culture = BopConstants.DefaultCulture
        };
    }
}