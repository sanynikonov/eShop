﻿using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstraction;
using Business.Models;
using Data.Entities;
using Data.Implementation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Business.Implementation.Services
{
    public class ProductService : IProductService
    {
        private readonly ShopDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AbstractValidator<ProductDto> _validator;

        public ProductService(IMapper mapper, ShopDbContext dbContext, AbstractValidator<ProductDto> validator)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _validator = validator;
        }

        public async Task CreateAsync(ProductDto productDto)
        {
            await _validator.ValidateAsync(productDto);
            await _dbContext.Products.AddAsync(_mapper.Map<Product>(productDto));
            
            await _dbContext.SaveChangesAsync();
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            return _mapper.Map<ProductDto>(await _dbContext.Products.FindAsync(id));
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _dbContext.Products.ToListAsync();
            
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public async Task UpdateAsync(ProductDto productDto)
        {
            await _validator.ValidateAsync(productDto);
            _dbContext.Products.Update(_mapper.Map<Product>(productDto));
            
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProductDto productDto)
        {
            _dbContext.Products.Remove(_mapper.Map<Product>(productDto));
            
            await _dbContext.SaveChangesAsync();
        }
    }
}