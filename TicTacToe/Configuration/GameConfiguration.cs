using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;
using TicTacToe.Enums;
using TicTacToe.Models;

namespace TicTacToe.Configuration
{
    public class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasKey(g => g.Id);
            
            builder.Property(g => g.Board)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                        v => JsonSerializer.Deserialize<List<List<Cell>>>(v, (JsonSerializerOptions)null)
                    )
                    .HasColumnType("text");
        }
    }
}

