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
    public class FluentBookAuthorConfig : IEntityTypeConfiguration<Fluent_BookAuthor>
    {
        public void Configure(EntityTypeBuilder<Fluent_BookAuthor> modelBuilder)
        {
            modelBuilder.HasKey(b => new { b.BookID, b.AuthorID });
            modelBuilder
                .HasOne(b => b.Fluent_Book)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(b => b.BookID);
            modelBuilder
                .HasOne(b => b.Fluent_Author)
                .WithMany(b => b.BookAuthors)
                .HasForeignKey(b => b.AuthorID);

        }
    }
}
