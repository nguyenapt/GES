using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Common.Factories
{
    public class DefaultAutofacServiceFactory<TService> : IServiceFactory<TService> where TService : class
    {
        protected readonly IEnumerable<Lazy<TService, Metadata>> _services;

        public DefaultAutofacServiceFactory(IEnumerable<Lazy<TService, Metadata>> services)
        {
            this._services = services;
        }

        public virtual TService Get(string key)
        {
            if (this._services == null) return null;
            var service = this._services.FirstOrDefault(s => s.Metadata.Key == key);

            return service?.Value;
        }
    }
}
