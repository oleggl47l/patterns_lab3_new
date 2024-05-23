using ChatApp.Core.Interfaces;
using ChatApp.Core.Models;

namespace ChatApp.Core.Services;

public class DataProxy(IDataSource dataSource) : IDataSource {
    private readonly Dictionary<string, UserConnection> _cache = new();

    public async Task<UserConnection?> GetUserConnectionAsync(string connectionId) {
        if (_cache.ContainsKey(connectionId))
            return _cache[connectionId];

        var connection = await dataSource.GetUserConnectionAsync(connectionId);
        
        if (connection != null)
            _cache[connectionId] = connection;

        return connection;
    }

    public async Task SetUserConnectionAsync(string connectionId, UserConnection userConnection) {
        _cache[connectionId] = userConnection;
        await dataSource.SetUserConnectionAsync(connectionId, userConnection);
    }

    public async Task RemoveUserConnectionAsync(string connectionId) {
        _cache.Remove(connectionId);
        await dataSource.RemoveUserConnectionAsync(connectionId);
    }
}