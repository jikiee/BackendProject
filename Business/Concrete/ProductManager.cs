using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Add(Product product)
        {
            var result = BusinessRules.Run(CheckIfProductCountOfCategoryCorrect(product.CategoryId),
                                           CheckIfProductNameExists(product.ProductName));
            if (result != null)
            {
                return result;
            }
            _productDal.Add(product);
            return new SuccessResult("Product Added.."); 
        }

        public IResult Delete(Product product)
        {
            _productDal.Delete(product);
            return new SuccessResult("Product Deleted");
        }

        public IDataResult<List<Product>> GetAll()
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), "Products listed..");
        }

        public IDataResult<List<Product>> GetByCategoryId(int categoryId)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == categoryId));
        }

        public IDataResult<Product> GetById(int id)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == id));
        }

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult("Product Updated...");
        }

        public IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
            if (result > 15)
            {
                return new ErrorResult("Product count limit exceeded for this category");
            }
            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();
            if (result)
            {
                return new ErrorResult("Product name already exits");
            }
            return new SuccessResult();
        }
    }
}
