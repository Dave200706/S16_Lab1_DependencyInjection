namespace ZombieParty.Models.Data
{
    public class ZombiePartyDbContextSeedStagingPerf : ZombiePartyDbContextSeedDev
    {
        public ZombiePartyDbContextSeedStagingPerf(ILogger<ZombiePartyDbContextSeed> logger, ZombiePartyDbContext context): base(logger, context)
        {
            this.numberZombieType = 1000;
            this.numberZombie = 1000;
            this.numberWeapon = 1000;

        }
    }
}
