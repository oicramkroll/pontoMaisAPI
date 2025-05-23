
using Models;

namespace Services.Adm
{
    public interface IAdmService
    {
        Task<IEnumerable<Users
        >> GetAllAdmsAsync();
        Task<Users?> GetAdmByIdAsync(int id);
        Task<int> CreateAdmAsync(Users model);
        Task<int> UpdateAdmAsync(Users model);
        Task<int> DeleteAdmAsync(int id);
    }
}
