public static class Choices
{
    private static bool _listenedToJordy;

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
}