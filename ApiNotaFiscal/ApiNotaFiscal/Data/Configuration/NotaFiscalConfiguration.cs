using ApiNotaFiscal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiNotaFiscal.Data.Configuration;

public class NotaFiscalConfiguration : IEntityTypeConfiguration<NotaFiscal>
{
    public void Configure(EntityTypeBuilder<NotaFiscal> builder)
    {
        builder.ToTable("NotasFiscal");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Numero).HasColumnType("int").IsRequired();
        builder.Property(p => p.DataEmissao).HasColumnType("datetime").IsRequired();
        builder.Property(p => p.Valor).HasColumnType("decimal(10, 2)").IsRequired();
    }
}