using Syracuse.Mobitheque.Core.Models;
using System.Threading.Tasks;

namespace Syracuse.Mobitheque.Core.Services.Files
{
    public interface IDepartmentService
    {
        Task<Department[]> GetDepartments();
        Task<Library[]> GetLibraries(bool refresh = false);
    }
}
