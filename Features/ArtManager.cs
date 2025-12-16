
namespace RikaMod.Features;

public class ArtManager
{
    // 1 ~ Alpha based artwork | No longer works after 01/08/2025 due to me swapping out a lot of new cards and refusing to spend the time to make new artwork for each.
    // 0 ~ default artwork
    private static int _artvalue; // DO NOT CHANGE FROM 0!
    
    // This controls the artTint for the default artmode
    // Use 71b9f3 for a sky color
    private static string _hexcodeDefault = "71b9f3";

    // I am mostly using this to control Console.WriteLine happening or not.
    // Set to true if you want things in the console--set to false if you do not.
    private static bool _isplaytester = true;

    // This controls the name of the Kite-ing / Temp Strafe Status
    // Your options are "temp strafe" or "kite-ing"
    private static string _kiteingDisplayName = "temp strafe";

    // This is mostly used to switch if I am logging any artifact procs or card draws or not. Probally should set this to false when you release this.
    // Set to true if you want things log and set to fasle if you do not.
    private static bool _privatelogALotOfThings = false;
    
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
    
    public static string KiteingDisplayName
    {
        get { return _kiteingDisplayName; }
        set { _kiteingDisplayName = value; }
    }

    public static bool LogALotOfThings
    {
        get { return _privatelogALotOfThings; }
        set { _privatelogALotOfThings = value; }
    }
}