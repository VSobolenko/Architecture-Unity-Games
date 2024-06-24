using Infrastructure.Services;

namespace UI.Services
{
public interface IWindowsService : IService
{
    void Open(WindowId id);
}
}