using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using System;

namespace ConsoleUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ProductManager productManager = new ProductManager(new EfProductDal(), new CategoryManager(new EfCategoryDal()));

            foreach(var item in productManager.GetAll().Data)
            {
                Console.WriteLine(item.ProductName);
            }
        }
    }
}
