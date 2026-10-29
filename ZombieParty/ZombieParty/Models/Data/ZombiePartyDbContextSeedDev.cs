using Bogus;

namespace ZombieParty.Models.Data
{
    public class ZombiePartyDbContextSeedDev: ZombiePartyDbContextSeed
    {
        protected int numberZombieType = 15;
        protected int numberZombie = 10;
        protected int numberWeapon = 50;

        public ZombiePartyDbContextSeedDev(ILogger<ZombiePartyDbContextSeed> logger, ZombiePartyDbContext context) : base(logger, context)
        {
            Randomizer.Seed = new Random(2546);
        }

        protected override List<ZombieType> GetZombieTypes()
        {
            var zombieType = new Faker<ZombieType>()
                .RuleFor(x => x.Point, (f, o) => f.Random.Int(2, 5))
                .RuleFor(x => x.TypeName, (f, o) => f.Hacker.Noun());
            return zombieType.Generate(numberZombieType);
        }

        protected override List<Zombie> GetZombies()
        {
            var listZombieTypeIds = this.context.ZombieType.Select(x => x.Id).ToList();
            var zombie = new Faker<Zombie>()
                .RuleFor(x => x.Point, (f, o) => f.Random.Int(1, 10))
                .RuleFor(x => x.ZombieTypeId, (f, o) => f.Random.CollectionItem(listZombieTypeIds))
                .RuleFor(x => x.Image, (f, o) => null)
                .RuleFor(x => x.Name, (f, o) => f.Name.FirstName())
                .RuleFor(x => x.Force, (f, o) => f.Random.Int(0, 10000))
                .RuleFor(x => x.ShortDesc, (f, o) => f.Lorem.Text());
            return zombie.Generate(numberZombie);
        }

        protected override List<Weapon> GetWeapons()
        {
            var weapons = new List<string>() {
            "Saber",
            "Sword",
            "Knife",
            "shuriken",
            "Foil",
            "Bow",
            "Crossbow"
        };

            var weapon = new Faker<Weapon>()
                .RuleFor(x => x.Name, (f, o) => f.Random.CollectionItem(weapons) + " " + f.Company.CompanyName())
                .RuleFor(x => x.Force, (f, o) => f.Random.Decimal(0, 10000))
                .RuleFor(x => x.Description, (f, o) => f.Company.CatchPhrase())
                .RuleFor(x => x.QtyBought, (f, o) => f.Random.Int(0, 200))
                .RuleFor(x => x.Qty, (f, o) => f.Random.Int(0, 200))
                .RuleFor(x => x.Price, (f, o) => decimal.Parse(f.Commerce.Price()))
                .RuleFor(x => x.Lifespan, (f, o) => f.Random.Decimal(0, 5000))
                .RuleFor(x => x.Resistance, (f, o) => f.Random.Decimal(0, 2000))
                .RuleFor(x => x.UpdatedDate, (f, o) => f.Date.Between(new DateTime(2020, 01, 01), new DateTime(2022, 07, 01)))
                .RuleFor(x => x.CreatedDate, (f, o) => f.Date.Past(3, o.UpdatedDate))
                .RuleFor(x => x.Image, (f, o) => f.Image.PicsumUrl());

            return weapon.Generate(numberWeapon);
        }
    }

}

