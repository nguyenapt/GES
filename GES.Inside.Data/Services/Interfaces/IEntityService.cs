using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Services.Interfaces
{

    public interface IEntityService<T> : IService
        where T : class 
    {
        void Add(T entity, bool isExecuteImmediate = false);
        void Delete(T entity, bool isExecuteImmediate = false);
        IEnumerable<T> GetAll();
        void Update(T entity, bool isExecuteImmediate = false);
        int Save();
    }
}
