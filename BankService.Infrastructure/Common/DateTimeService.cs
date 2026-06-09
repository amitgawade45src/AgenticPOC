using BankService.Application.Interfaces;

namespace BankService.Infrastructure.Common;

/// <summary>
/// System-clock implementation of <see cref="IDateTimeService"/>.
/// </summary>
public sealed class DateTimeService : IDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
}
