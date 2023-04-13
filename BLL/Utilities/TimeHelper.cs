namespace BLL.Utilities;

public class TimeHelper : ITimeHelper
{
    private readonly ISystemClock _systemClock;

    public TimeHelper(ISystemClock systemClock)
    {
        _systemClock = systemClock;
    }

    public string CurrentNumericDate()
    {
        DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        TimeSpan timeSinceEpoch = _systemClock.UtcNow - unixEpoch;
        return ((long)timeSinceEpoch.TotalSeconds).ToString();
    }
}