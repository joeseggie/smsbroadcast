using System;
using System.Threading.Tasks;

namespace SmsBroadcast.TaskRunner
{
    public interface ISmsSender
    {
        Task SendMessageAsync(string broadcast, string from, string to, string message, Guid broadcastId);
    }
}
