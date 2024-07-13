using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaySkyTask.Domain.Entities;


namespace PaySkyTask.Infrastructure.Configuration;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {

        builder.UseTptMappingStrategy();

        builder.Property(T => T.FirstName)
            .HasColumnType("varchar")
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(T => T.LastName)
            .HasColumnType("varchar")
            .HasMaxLength(30);

        builder.Property(T => T.Gender)
            .HasColumnType("varchar")
            .HasMaxLength(20);

        builder.Property(T => T.DateOfBirth)
               .IsRequired();
    }

}
