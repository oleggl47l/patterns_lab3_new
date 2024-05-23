using System.Text.Json;
using ChatApp.Core.Interfaces;
using ChatApp.Core.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace ChatApp.Core.Services;

public class DataSource : IDataSource {
    private readonly IDistributedCache _cache;

    public DataSource(IDistributedCache cache) {
        _cache = cache;
    }

    public async Task<UserConnection?> GetUserConnectionAsync(string connectionId) {
        var stringConnection = await _cache.GetStringAsync(connectionId);
        return stringConnection != null
            ? JsonSerializer.Deserialize<UserConnection>(stringConnection)
            : null;
    }

    public async Task SetUserConnectionAsync(string connectionId, UserConnection userConnection) {
        var stringConnection = JsonSerializer.Serialize(userConnection);
        await _cache.SetStringAsync(connectionId, stringConnection);
    }

    public async Task RemoveUserConnectionAsync(string connectionId) {
        await _cache.RemoveAsync(connectionId);
    }
}