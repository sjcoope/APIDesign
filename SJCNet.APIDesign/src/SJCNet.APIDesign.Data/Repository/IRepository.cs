using SJCNet.APIDesign.Model;
using System.Linq;
using System.Threading.Tasks;

namespace SJCNet.APIDesign.Data.Repository
{
    public interface IRepository<T> where T : class, IEntity
    {
        bool Add(T entity);

        bool Delete(T entity);

        bool Delete(int id);

        T Get(int id);

        IQueryable<T> Get();

        bool Update(T entity);
    }
}