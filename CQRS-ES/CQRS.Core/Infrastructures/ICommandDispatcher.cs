using CQRS.Core.Commands;
using Microsoft.VisualBasic;

namespace CQRS.Core.Infrastructures
{
    public interface ICommandDispatcher
    {
        void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand;
        Task SendAsync(BaseCommand command);
    }
}