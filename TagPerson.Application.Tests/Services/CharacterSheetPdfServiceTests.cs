using TagPerson.Application.DTOs;
using TagPerson.Application.Services;

namespace TagPerson.Application.Tests.Services;

public sealed class CharacterSheetPdfServiceTests
{
    [Fact]
    public void Generate_WithMinimalSheet_ReturnsPdfDocument()
    {
        var service = new CharacterSheetPdfService();
        var sheet = new CharacterSheetDto(
            42,
            "Lina",
            1,
            0,
            "Jogador",
            null,
            null,
            null,
            new SimpleLookupDto(1, "Humano"),
            new SimpleLookupDto(1, "Guerreiro"),
            null,
            null,
            new CharacterAttributesDto(1, 1, 1, 1, 1, 1, 1),
            new CharacterPointsDto(0, 0, 0, 0),
            new CharacterFeaturesDto(null, null, null, null, null, null, null, null),
            new CharacterCoinsDto(0, 0, 0),
            new DerivedStatsDto(),
            [],
            [],
            [],
            [],
            [],
            [],
            new CharacterPointBudgetDto(
                new PointAllocationDto(0, 0, 0),
                new PointAllocationDto(0, 0, 0),
                new PointAllocationDto(0, 0, 0),
                new PointAllocationDto(0, 0, 0),
                new PointAllocationDto(0, 0, 0)));

        var result = service.Generate(sheet);

        Assert.True(result.Length > 4);
        Assert.Equal("%PDF", System.Text.Encoding.ASCII.GetString(result, 0, 4));
    }
}
