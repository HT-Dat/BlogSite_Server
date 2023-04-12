namespace BLL.Utilities;

public interface ISystemClock
{
    DateTime UtcNow { get; }
}