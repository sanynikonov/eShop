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
    public class CartService : ICartService
    {
        private readonly ShopDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly AbstractValidator<CartDto> _validator;

        public CartService(IMapper mapper, ShopDbContext dbContext, AbstractValidator<CartDto> validator)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _validator = validator;
        }

        public async Task CreateAsync(CartDto cartDto)
        {
            await _validator.ValidateAsync(cartDto);
            await _dbContext.Carts.AddAsync(_mapper.Map<Cart>(cartDto));
            
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CartDto> GetByIdAsync(int id)
        {
            return _mapper.Map<CartDto>(await _dbContext.Carts.FindAsync(id));
        }

        public async Task<IEnumerable<CartDto>> GetAllAsync()
        {
            var carts = await _dbContext.Carts.ToListAsync();
            
            return _mapper.Map<IEnumerable<CartDto>>(carts);
        }

        public async Task UpdateAsync(CartDto cartDto)
        {
            await _validator.ValidateAsync(cartDto);
            _dbContext.Carts.Update(_mapper.Map<Cart>(cartDto));
            
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(CartDto cartDto)
        {
            _dbContext.Carts.Remove(_mapper.Map<Cart>(cartDto));
            
            await _dbContext.SaveChangesAsync();
        }
    }
}