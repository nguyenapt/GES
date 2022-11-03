using GES.Inside.Data.Models.Auth;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IGesRolesRepository : IGenericRepository<GesRole>
    {
        GesRole GetById(string id);

        bool CheckPermission(string userId, string controller, int action);
    }
}
