using prjWebCsBennyStockWebApp.Models;

namespace prjWebCsBennyStockWebApp.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (!context.Restaurants.Any())
            {
                context.Restaurants.AddRange(
                    new Restaurant { Name = "Benny Boucherville", Address = "Boucherville" },
                    new Restaurant { Name = "Benny La Prairie", Address = "La Prairie" }
                );

                context.SaveChanges();
            }

            if (!context.Items.Any())
            {
                context.Items.AddRange(
                    new Item { Code = "BACON001", Name = "Bacon", Unit = "kg", Price = 12.99m },
                    new Item { Code = "FRITE001", Name = "Frites", Unit = "sac", Price = 8.50m },
                    new Item { Code = "FANTA001", Name = "Fanta", Unit = "caisse", Price = 15.00m }
                );

                context.SaveChanges();
            }

            if (!context.Stocks.Any())
            {
                var boucherville = context.Restaurants.First(r => r.Name == "Benny Boucherville");
                var laprairie = context.Restaurants.First(r => r.Name == "Benny La Prairie");

                var bacon = context.Items.First(i => i.Code == "BACON001");
                var frites = context.Items.First(i => i.Code == "FRITE001");
                var fanta = context.Items.First(i => i.Code == "FANTA001");

                context.Stocks.AddRange(
                    new Stock { RestaurantId = boucherville.Id, ItemId = bacon.Id, Quantity = 12 },
                    new Stock { RestaurantId = boucherville.Id, ItemId = frites.Id, Quantity = 8 },
                    new Stock { RestaurantId = boucherville.Id, ItemId = fanta.Id, Quantity = 5 },

                    new Stock { RestaurantId = laprairie.Id, ItemId = bacon.Id, Quantity = 20 },
                    new Stock { RestaurantId = laprairie.Id, ItemId = frites.Id, Quantity = 3 },
                    new Stock { RestaurantId = laprairie.Id, ItemId = fanta.Id, Quantity = 9 }
                );

                context.SaveChanges();
            }

            if (!context.Users.Any())
            {
                var boucherville = context.Restaurants.First(r => r.Name == "Benny Boucherville");
                var laprairie = context.Restaurants.First(r => r.Name == "Benny La Prairie");

                context.Users.AddRange(
                    new User
                    {
                        FullName = "Admin System",
                        Email = "admin@benny.com",
                        Password = "1234",
                        Role = "Admin",
                        RestaurantId = null
                    },
                    new User
                    {
                        FullName = "Manager Boucherville",
                        Email = "boucherville@benny.com",
                        Password = "1234",
                        Role = "Manager",
                        RestaurantId = boucherville.Id
                    },
                    new User
                    {
                        FullName = "Manager La Prairie",
                        Email = "laprairie@benny.com",
                        Password = "1234",
                        Role = "Manager",
                        RestaurantId = laprairie.Id
                    }
                );

                context.SaveChanges();
            }
        }
    }
}