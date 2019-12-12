public static class Choices
{
    private static bool _listenedToJordy;
    private static bool _askedAboutDay;

    public static bool listenedToJordy 
    {
        get 
        {
            return _listenedToJordy;
        }
        set 
        {
            _listenedToJordy = value;
        }
    }

    public static bool askedAboutDay
    {
        get {
            return _askedAboutDay;
        }
        set
        {
            _askedAboutDay = value;
        }
    }
}