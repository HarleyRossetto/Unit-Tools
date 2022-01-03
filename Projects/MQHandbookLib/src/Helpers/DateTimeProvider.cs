using System;

namespace MQHandbookLib.src.Helpers;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime DateTimeNow => DateTime.Now;
}
