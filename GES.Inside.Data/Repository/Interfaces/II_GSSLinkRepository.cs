using GES.Inside.Data.DataContexts;
using System.Collections;
using System.Collections.Generic;
using GES.Inside.Data.Models;
using System;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_GSSLinkRepository : IGenericRepository<I_GSSLink>
    {
        I_GSSLink GetById(Guid id);
    }
}
