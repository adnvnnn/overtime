using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Data.Entities;

namespace Overtime.Web.Services;

public record AddressLine(int Id, string RecipientName, string PhoneNumber, string Province, string City, string District, string Address1, string? Address2, string PostalCode, bool IsDefault);

public class AddressService(IDbContextFactory<AppDbContext> dbFactory)
{
    public async Task<List<AddressLine>> GetUserAddressesAsync(int userId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Addresses
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.IsDefault)
            .ThenByDescending(a => a.CreatedAt)
            .Select(a => new AddressLine(
                a.Id, a.RecipientName, a.PhoneNumber, a.Province, a.City, a.District,
                a.Address1, a.Address2, a.PostalCode, a.IsDefault))
            .ToListAsync();
    }

    public async Task<AddressLine?> GetAddressAsync(int addressId, int userId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Addresses
            .Where(a => a.Id == addressId && a.UserId == userId)
            .Select(a => new AddressLine(
                a.Id, a.RecipientName, a.PhoneNumber, a.Province, a.City, a.District,
                a.Address1, a.Address2, a.PostalCode, a.IsDefault))
            .FirstOrDefaultAsync();
    }

    public async Task<int> AddAddressAsync(int userId, string recipientName, string phoneNumber, string province,
        string city, string district, string address1, string? address2, string postalCode, bool isDefault = false)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        if (isDefault)
        {
            var existing = await db.Addresses.Where(a => a.UserId == userId && a.IsDefault).ToListAsync();
            foreach (var addr in existing)
                addr.IsDefault = false;
        }

        var address = new Address
        {
            UserId = userId,
            RecipientName = recipientName,
            PhoneNumber = phoneNumber,
            Province = province,
            City = city,
            District = district,
            Address1 = address1,
            Address2 = address2,
            PostalCode = postalCode,
            IsDefault = isDefault || (await db.Addresses.CountAsync(a => a.UserId == userId) == 0),
            CreatedAt = DateTime.UtcNow,
        };

        db.Addresses.Add(address);
        await db.SaveChangesAsync();
        return address.Id;
    }

    public async Task UpdateAddressAsync(int addressId, int userId, string recipientName, string phoneNumber,
        string province, string city, string district, string address1, string? address2, string postalCode, bool isDefault = false)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var address = await db.Addresses.FirstOrDefaultAsync(a => a.Id == addressId && a.UserId == userId);
        if (address is null) return;

        if (isDefault && !address.IsDefault)
        {
            var existing = await db.Addresses.Where(a => a.UserId == userId && a.IsDefault).ToListAsync();
            foreach (var addr in existing)
                addr.IsDefault = false;
        }

        address.RecipientName = recipientName;
        address.PhoneNumber = phoneNumber;
        address.Province = province;
        address.City = city;
        address.District = district;
        address.Address1 = address1;
        address.Address2 = address2;
        address.PostalCode = postalCode;
        address.IsDefault = isDefault;

        await db.SaveChangesAsync();
    }

    public async Task DeleteAddressAsync(int addressId, int userId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var address = await db.Addresses.FirstOrDefaultAsync(a => a.Id == addressId && a.UserId == userId);
        if (address is null) return;

        db.Addresses.Remove(address);

        if (address.IsDefault)
        {
            var defaultAddr = await db.Addresses
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .FirstOrDefaultAsync();
            if (defaultAddr is not null)
                defaultAddr.IsDefault = true;
        }

        await db.SaveChangesAsync();
    }

    public async Task<AddressLine?> GetDefaultAddressAsync(int userId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Addresses
            .Where(a => a.UserId == userId && a.IsDefault)
            .Select(a => new AddressLine(
                a.Id, a.RecipientName, a.PhoneNumber, a.Province, a.City, a.District,
                a.Address1, a.Address2, a.PostalCode, a.IsDefault))
            .FirstOrDefaultAsync();
    }
}
