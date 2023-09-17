using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiProyectoApi.Models;

namespace MiProyectoApi.Datos
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options) :base(options)
        {
            
        }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>() .HasData(
                new Customer()
                {
                    Id=1,
                    Name="Marcel",
                    Surname="Soto",
                    Photo="blablabla",
                    CreatedBy=1,
                    Date= DateTime.Now,
                },

                new Customer()
                {
                    Id=2,
                    Name="Alberto",
                    Surname="Gonzalez",
                    Photo="blablablaaaa",
                    CreatedBy=2,
                    Date= DateTime.Now,
                },

                new Customer()
                {
                    Id=3,
                    Name="Manuel",
                    Surname="Esquinso",
                    Photo="blablablaaaaaa",
                    CreatedBy=3,
                    Date= DateTime.Now,
                }

            );
        }
    }
}