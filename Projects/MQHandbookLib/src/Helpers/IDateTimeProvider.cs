using System;

namespace MQHandbookLib.src.Helpers;

public interface IDateTimeProvider
{
    DateTime DateTimeNow { get; }
}
