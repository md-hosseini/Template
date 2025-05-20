using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Domain.Common
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
