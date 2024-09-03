namespace WinPrayersApp.Models;

public enum BackdropType
{
    Default,
    Mica,
    MicaAlt,
    Acrylic
}

public static class BackdropTypes
{
    public static BackdropType Default => BackdropType.Default;
    public static BackdropType Mica => BackdropType.Mica;
    public static BackdropType Acrylic => BackdropType.Acrylic;
}
