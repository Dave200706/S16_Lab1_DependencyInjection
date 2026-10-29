using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

namespace ZombieParty.Models.Data
{
    public class ZombiePartyDbContextSeed : IZombiePartyDbContextSeed
    {
        private ILogger<ZombiePartyDbContextSeed> logger;
        protected ZombiePartyDbContext context;

        public ZombiePartyDbContextSeed(ILogger<ZombiePartyDbContextSeed> logger, ZombiePartyDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public async Task SeedAsync(int retry = 0)
        {
            var retryForAvailability = retry;
            try
            {
                if (context.Database.IsSqlServer())
                {
                    context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability >= 10) throw;
                retryForAvailability++;
                logger.LogError(ex.Message);
                await this.SeedAsync(retryForAvailability);
                throw;
            }
            try
            {
                var zombieTypes = GetZombieTypes();
                if (await context.ZombieType.CountAsync() != zombieTypes.Count)
                {
                    context.ZombieType.RemoveRange(context.ZombieType);
                    await context.ZombieType.AddRangeAsync(zombieTypes);
                    await context.SaveChangesAsync();
                }
                var zombies = GetZombies();
                if (await context.Zombie.CountAsync() != zombies.Count)
                {
                    context.Zombie.RemoveRange(context.Zombie);
                    await context.Zombie.AddRangeAsync(zombies);
                    await context.SaveChangesAsync();
                }
                var weapons = GetWeapons();
                if (await context.Weapon.CountAsync() != weapons.Count)
                {
                    context.Weapon.RemoveRange(context.Weapon);
                    await context.Weapon.AddRangeAsync(weapons);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
        }
        protected virtual List<ZombieType> GetZombieTypes() => new List<ZombieType>();
        protected virtual List<Zombie> GetZombies() => new List<Zombie>();
        protected virtual List<Weapon> GetWeapons() => new List<Weapon>();
    }

}
