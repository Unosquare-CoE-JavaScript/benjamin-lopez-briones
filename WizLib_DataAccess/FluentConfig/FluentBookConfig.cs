using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WizLib_Model.Models;

namespace WizLib_DataAccess.FluentConfig
{
    public class FluentBookConfig : IEntityTypeConfiguration<Fluent_Book>
    {
        public void Configure(EntityTypeBuilder<Fluent_Book> modelBuilder)
        {
            modelBuilder.HasKey(b => b.Book_ID);
            modelBuilder.Property(b => b.ISBN).IsRequired().HasMaxLength(15);
            modelBuilder.Property(b => b.Title).IsRequired();
            modelBuilder.Property(b => b.Price).IsRequired();
            modelBuilder.HasOne(b => b.BookDetail).WithOne(b => b.Book).HasForeignKey<Fluent_Book>("BookDetailID"); //1-1 relaton
            modelBuilder.HasOne(b => b.Publisher).WithMany(b => b.Fluent_Books).HasForeignKey(b => b.PublisherID); //many to 1
        }
    }
}
