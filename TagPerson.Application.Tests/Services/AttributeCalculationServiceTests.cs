using TagPerson.Application.Services;
using TagPerson.Domain.Entities;

namespace TagPerson.Application.Tests.Services;

public sealed class AttributeCalculationServiceTests
{
    private readonly AttributeCalculationService _service = new();

    [Fact]
    public void ValidateAttributeDistribution_WithinBudgetAndLimits_IsValid()
    {
        var race = new Race { Id = 2 };
        var values = Enum.GetValues<AttributeType>().ToDictionary(type => type, _ => 0);
        values[AttributeType.Agilidade] = 2;

        var result = _service.ValidateAttributeDistribution(race, values);

        Assert.True(result.isValid);
    }

    [Fact]
    public void ValidateAttributeDistribution_AboveRacialMaximum_IsInvalid()
    {
        var race = new Race { Id = 2, BonusAgi = 1 };
        var values = Enum.GetValues<AttributeType>().ToDictionary(type => type, _ => 0);
        values[AttributeType.Agilidade] = 6;

        var result = _service.ValidateAttributeDistribution(race, values);

        Assert.False(result.isValid);
        Assert.Contains("fora dos limites", result.message);
    }

    [Fact]
    public void ValidateAttributeDistribution_AboveAvailableBudget_IsInvalid()
    {
        var race = new Race { Id = 2 };
        var values = Enum.GetValues<AttributeType>().ToDictionary(type => type, _ => 4);

        var result = _service.ValidateAttributeDistribution(race, values);

        Assert.False(result.isValid);
        Assert.Contains("Pontos insuficientes", result.message);
    }
}
