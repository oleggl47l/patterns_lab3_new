namespace ChatApp.Core.Interfaces;

public interface IChatClient {
    Task ReceiveMessage(string userName, string message);
}