using System.Linq;
using SJCNet.APIDesign.Data.Repository;
using SJCNet.APIDesign.Model;
using System.Collections.Generic;

namespace SJCNet.APIDesign.Tests.Unit.Data
{
    public class MockRepository<T> : IRepository<T> where T : class, IEntity
    {
        private List<T> _data;

        public MockRepository(List<T> data)
        {
            _data = data;
        }

        public bool Add(T entity)
        {
            _data.Add(entity);
            return true;
        }

        public bool Delete(int id)
        {
            var match = _data.SingleOrDefault(i => i.Id == id);
            if(match == null)
            {
                return false;
            }

            return Delete(match);
        }

        public bool Delete(T entity)
        {
            return _data.Remove(entity);
        }

        public IQueryable<T> Get()
        {
            return _data.AsQueryable();
        }

        public T Get(int id)
        {
            return _data.SingleOrDefault(i => i.Id == id);
        }

        public bool Update(T entity)
        {
            var match = _data.SingleOrDefault(i => i.Id == entity.Id);
            if(match != null)
            {
                _data.Remove(match);
                _data.Add(entity);
                return true;
            }

            return false;
        }
    }
}
