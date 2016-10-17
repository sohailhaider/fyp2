
using Microsoft.AspNet.SignalR;
namespace SignalRChat
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.addNewMessageToPage(name, message);
        }
        public void Send(string id, string name, string message)
        {
            Clients.Group(id).addNewMessageToPage(name, message);
        }
        public void Join(string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);
        }
    }
}