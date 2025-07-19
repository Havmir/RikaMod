
namespace RikaMod.Features;

public class ArtManager
{
    // 1 ~ Alpha based artwork
    // 0 ~ default artwork
    private static int _artvalue = 0;
    
    // This controls the artTint for the default artmode
    // Use 71b9f3 for a sky color
    private static string _hexcodeDefault = "71b9f3";
    
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
    
}