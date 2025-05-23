using Models;
using Repositories;

namespace Services.Adm
{
    public class AdmService : IAdmService
    {
        private readonly IGenericRepository<Users> _repository;
        private string _tableName = "Administradores";

        public AdmService(IGenericRepository<Users> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Users>> GetAllAdmsAsync()
        {
            return _repository.GetAllAsync(_tableName);
        }

        public Task<Users?> GetAdmByIdAsync(int id)
        {
            return _repository.GetByIdAsync(_tableName, id);
        }

        public Task<int> CreateAdmAsync(Users model)
        {
            return _repository.InsertAsync(_tableName, model);
        }

        public Task<int> UpdateAdmAsync(Users model)
        {
            return _repository.UpdateAsync(_tableName, model, model.Id);
        }

        public Task<int> DeleteAdmAsync(int id)
        {
            return _repository.DeleteAsync(_tableName, id);
        }
    }
}
