using ChatApp.Core.Models;

namespace ChatApp.Core.Interfaces;

public interface IDataSource {
    Task<UserConnection?> GetUserConnectionAsync(string connectionId);
    Task SetUserConnectionAsync(string connectionId, UserConnection userConnection);
    Task RemoveUserConnectionAsync(string connectionId);
}