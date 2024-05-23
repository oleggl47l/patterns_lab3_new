using ChatApp.Core.Interfaces;
using ChatApp.Core.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Web.Hubs;

public class ChatHub(IDataSource dataSource) : Hub<IChatClient> {
    public async Task JoinChat(UserConnection connection) {
        await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);
        await dataSource.SetUserConnectionAsync(Context.ConnectionId, connection);

        Console.WriteLine($"{connection.UserName} joined {connection.ChatRoom}");


        await Clients
            .Group(connection.ChatRoom)
            .ReceiveMessage("Admin", $"{connection.UserName} joined the chat");
    }

    public async Task SendMessage(string message) {
        var connection = await dataSource.GetUserConnectionAsync(Context.ConnectionId);

        if (connection != null) {
            await Clients
                .Group(connection.ChatRoom)
                .ReceiveMessage(connection.UserName, message);
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception) {
        var connection = await dataSource.GetUserConnectionAsync(Context.ConnectionId);

        if (connection != null) {
            await dataSource.RemoveUserConnectionAsync(Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.ChatRoom);

            await Clients.Group(connection.ChatRoom)
                .ReceiveMessage("Admin", $"{connection.UserName} left the chat");
        }
    }
}