
namespace RikaMod.Features;

public class ArtManager
{
    // 1 ~ Alpha based artwork
    // 0 ~ default artwork
    private static int _artvalue = 0;
    
    // This controls the artTint for the default artmode
    // Use 71b9f3 for a sky color
    private static string _hexcodeDefault = "71b9f3";

    // I am mostly using this to control Console.WriteLine happening or not.
    // Set to true if you want things in the console--set to false if you do not.
    private static bool _isplaytester = false;
    
    public static int ArtNumber
    {
        get { return _artvalue; }
        set { _artvalue = value; }
    }
    
    public static string ModeDefaultArtTint
    {
        get { return _hexcodeDefault; }
        set { _hexcodeDefault = value; }
    }

    public static bool IsPlayTester
    {
        get { return _isplaytester; }
        set { _isplaytester = value; }
    }
}