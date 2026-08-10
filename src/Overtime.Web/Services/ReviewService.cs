using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Data.Entities;

namespace Overtime.Web.Services;

public record ReviewLine(int Id, int UserId, string ReviewerName, int Rating, string? Comment, DateTime CreatedAt);

public class ReviewService(IDbContextFactory<AppDbContext> dbFactory)
{
    public async Task<List<ReviewLine>> GetProductReviewsAsync(string productId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var reviews = await db.Reviews
            .Where(r => r.ProductId == productId)
            .Include(r => r.User)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return reviews.Select(ToLine).ToList();
    }

    public async Task<(double Average, int Count)> GetRatingSummaryAsync(string productId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var ratings = await db.Reviews.Where(r => r.ProductId == productId).Select(r => r.Rating).ToListAsync();
        return ratings.Count == 0 ? (0, 0) : (ratings.Average(), ratings.Count);
    }

    public async Task<ReviewLine?> GetUserReviewAsync(string productId, int userId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var review = await db.Reviews.Include(r => r.User)
            .FirstOrDefaultAsync(r => r.ProductId == productId && r.UserId == userId);
        return review is null ? null : ToLine(review);
    }

    public async Task SubmitReviewAsync(string productId, int userId, int rating, string? comment)
    {
        rating = Math.Clamp(rating, 1, 5);

        await using var db = await dbFactory.CreateDbContextAsync();
        var existing = await db.Reviews.FirstOrDefaultAsync(r => r.ProductId == productId && r.UserId == userId);
        if (existing is not null)
        {
            existing.Rating = rating;
            existing.Comment = comment;
            existing.CreatedAt = DateTime.UtcNow;
        }
        else
        {
            db.Reviews.Add(new Review
            {
                ProductId = productId,
                UserId = userId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.UtcNow,
            });
        }

        await db.SaveChangesAsync();
    }

    private static ReviewLine ToLine(Review review) =>
        new(review.Id, review.UserId, DisplayName(review.User), review.Rating, review.Comment, review.CreatedAt);

    private static string DisplayName(User? user)
    {
        if (user is null)
            return "کاربر";
        if (!string.IsNullOrWhiteSpace(user.FullName))
            return user.FullName;

        var phone = user.PhoneNumber;
        var suffix = phone.Length >= 4 ? phone[^4..] : phone;
        return $"کاربر …{suffix}";
    }
}
