using Bogus;
using Microsoft.EntityFrameworkCore;
using PeopleIncApi.Models;

namespace PeopleIncApi.Data
{
    public class Context : DbContext
    {//bkp
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<Pessoa> Pessoas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Config do Banco
            modelBuilder.Entity<Pessoa>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Pessoa>().ToTable("Pessoa");

            //modelBuilder.Entity<Pessoa>().HasData(GenerateRandomPeople(10));


            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        }

        //private Pessoa[] GenerateRandomPeople(int count)
        //{
        //    var faker = new Faker<Pessoa>()
        //                    .RuleFor(p => p.Id, f => f.Random.Long(1, 100))
        //                    .RuleFor(p => p.Nome, f => f.Name.FirstName())
        //                    .RuleFor(p => p.Idade, f => f.Random.Int(20, 55))
        //                    .RuleFor(p => p.Email, f => f.Internet.Email());

        //    return faker.Generate(count).ToArray();

        //}
    }
}