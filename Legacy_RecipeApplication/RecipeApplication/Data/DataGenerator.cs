using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApplication.Data
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider, string userId)
        {
            using( var ctx = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (ctx.Recipes.Any())
                    return;

                
                ctx.Recipes.AddRange(new Recipe
                {
                    IsDeleted = false,
                    LastModified = DateTimeOffset.UtcNow,
                    Name = "Happy Meal",
                    CreatedById = userId,
                    TimeToCook = TimeSpan.FromMinutes(15),                    
                    Method = "Fast Food",
                    IsVegan= false,
                    IsVegetarian= false,
                    Ingredients = new List<Ingredient>{ new Ingredient { Name = "Meat", Quantity = 1, Unit = "Pice" }, new Ingredient { Name = "Bread", Quantity = 2, Unit = "Slice" } }
                });

                ctx.SaveChanges();
            }
        }
    }
}
