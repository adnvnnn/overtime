using System.Collections.Concurrent;

namespace Overtime.Web.Services;

// Simple in-memory sliding-window limiter (single-instance app, resets on restart — fine at
// this shop's scale). Used to cap OTP requests per IP so one client can't spam/enumerate
// phone numbers regardless of the existing per-phone cooldown.
public class RequestRateLimiter
{
    private readonly ConcurrentDictionary<string, List<DateTime>> _hits = new();

    public bool TryConsume(string key, int maxCount, TimeSpan window)
    {
        var now = DateTime.UtcNow;
        var timestamps = _hits.GetOrAdd(key, _ => []);

        lock (timestamps)
        {
            timestamps.RemoveAll(t => now - t > window);
            if (timestamps.Count >= maxCount)
                return false;

            timestamps.Add(now);
            return true;
        }
    }
}
