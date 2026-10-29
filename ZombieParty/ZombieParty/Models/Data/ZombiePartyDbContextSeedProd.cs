namespace ZombieParty.Models.Data
{
    public class ZombiePartyDbContextSeedProd: ZombiePartyDbContextSeed
    {
        public ZombiePartyDbContextSeedProd(ILogger<ZombiePartyDbContextSeed> logger, ZombiePartyDbContext context): base(logger, context)
        {

        }

        protected override List<ZombieType> GetZombieTypes()
        {
            return new List<ZombieType>()
            {
                new ZombieType(){TypeName = "Virus", Point=1},
                new ZombieType(){TypeName="Contact", Point=3},
                new ZombieType(){TypeName = "Fiction" , Point = 3},
                new ZombieType(){TypeName = "Légendaire", Point=5}
            };
        }
    }
}
