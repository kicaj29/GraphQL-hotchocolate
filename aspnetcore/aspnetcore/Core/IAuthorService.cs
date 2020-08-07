using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore.Core
{
    public interface IAuthorService
    {
        IQueryable<Author> GetAll();
        Author GetById(int id);
        List<Author> GetByIds(List<int> ids);
    }
}
