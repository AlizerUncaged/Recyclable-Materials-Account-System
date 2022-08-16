using System.Collections.Generic;

namespace Recyclable_Materials
{
    public interface ICrudService<T>
    {
        int Id { get; set; }
        
        void Create();

        IEnumerable<T> Read();

        void Update(IEnumerable<T> contents);

        void Delete(T obj);
    }
}