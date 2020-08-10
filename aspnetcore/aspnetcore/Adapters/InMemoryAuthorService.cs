using aspnetcore.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspnetcore.Adapters
{
    public class InMemoryAuthorService : IAuthorService
    {
        private IList<Author> _authors;

        public InMemoryAuthorService()
        {
            _authors = new List<Author>()
            {
                new Author() { Id = 1, Name = "Fabio", Surname = "Rossi", Country = "PL"},
                new Author() { Id = 2, Name = "Paolo", Surname = "Verdi", Country = "PL"},
                new Author() { Id = 3, Name = "Carlo", Surname = "Bianchi", Country = "PL"},
                new Author() { Id = 4, Name = "Adam", Surname = "Bonec", Country = "USA"}
            };
        }

        public IQueryable<Author> GetAll()
        {
            return _authors.AsQueryable();
        }

        public Author GetById(int id)
        {
            return _authors.FirstOrDefault(a => a.Id == id);
        }

        public List<Author> GetByIds(List<int> ids)
        {
            return _authors.Where(a => ids.Contains(a.Id)).ToList();
        }

        public ILookup<string, Author> GroupByCountry(IReadOnlyList<string> countries)
        {
            var res = _authors.ToLookup(a => a.Country);
            return res;
        }
    }
}
