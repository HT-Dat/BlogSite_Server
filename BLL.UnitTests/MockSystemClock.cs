using System;
using BLL.Utilities;

namespace BLL.UnitTests;

public class MockSystemClock : ISystemClock
{
    public DateTime UtcNow { get; set; }
}