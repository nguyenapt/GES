﻿using GES.Inside.Data.DataContexts;
using System.Collections;
using System.Collections.Generic;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_TimeLineItemsRepository : IGenericRepository<I_TimelineItems>
    {
        I_TimelineItems GetById(long id);

    }
}