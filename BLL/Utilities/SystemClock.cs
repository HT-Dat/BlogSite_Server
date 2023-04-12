namespace BLL.Utilities;

public class SystemClock : ISystemClock
{
    public DateTime UtcNow
    {
        get
        {
            return DateTime.UtcNow;
        }
    }
}