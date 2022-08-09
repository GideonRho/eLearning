using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPI.Contexts;
using WebAPI.Misc;
using WebAPI.Models.Database;
using WebAPI.Models.Database.Enums;

namespace WebAPI.Services
{
    public class ProductService
    {

        private readonly DatabaseContext _context;

        public ProductService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<List<string>> GenerateKeys(int amount, int duration)
        {
            return await GenerateKeys(amount, (s) => new ProductKey(s, duration));
        }
        
        public async Task<List<string>> GenerateKeys(int amount, DateTime expirationDate)
        {
            return await GenerateKeys(amount, (s) => new ProductKey(s, expirationDate));
        }
        
        private async Task<List<string>> GenerateKeys(int amount, Func<string, ProductKey> constructor)
        {
            
            var list = new List<string>();

            for(int i = 0; i < amount; i++) 
                list.Add(KeyGenerator.Generate());

            var keys = list.Select(constructor.Invoke).ToList();

            await _context.ProductKeys.AddRangeAsync(keys);
            await _context.SaveChangesAsync();
            
            return list;
        }

        public async Task<bool> RegisterKey(int userId, string key)
        {

            User user = await _context.Users.FindAsync(userId);
            if (user == null) return false;
            
            ProductKey productKey = await _context.ProductKeys
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Key == key && p.User == null);

            if (productKey == null) return false;
            
            productKey.User = user;
            productKey.Activate();
            if (user.Role == ERole.User) user.Role = ERole.Premium;
            
            _context.Update(user);
            _context.Update(productKey);
            await _context.SaveChangesAsync();

            return true;
        }
        
    }
}