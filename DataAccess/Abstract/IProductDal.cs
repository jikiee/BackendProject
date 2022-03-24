using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Abstract
{
    public interface IProductDal
    {
        List<Product> GetAll(Expression<Func<Product, bool>> filter = null);
        Product Get(Expression<Func<Product, bool>> filter);
        void Add(Product product);
        void Update(Product product);
        void Delete(Product product);
    }
}
