using TagPerson.Domain.Entities;

namespace TagPerson.Application.Services;

/// <summary>
/// Serviço responsável pelo cálculo e validação da distribuição de pontos entre atributos.
/// </summary>
public sealed class AttributeCalculationService
{
    /// <summary>
    /// Calcula os pontos iniciais disponíveis para o personagem.
    /// </summary>
    public int CalculateInitialPoints(Race race)
    {
        const int basePoints = 15;
        
        // Humanos recebem 1 ponto extra
        if (race.Id == 1) // Ajuste o ID conforme sua base de dados
        {
            return basePoints + 1; // 16 pontos para humanos
        }
        
        return basePoints; // 15 pontos para outras raças
    }

    /// <summary>
    /// Obtém o valor inicial de um atributo específico (0 + bônus racial).
    /// </summary>
    public int GetInitialAttributeValue(AttributeType attributeType, Race race) => attributeType switch
    {
        AttributeType.Agilidade => race.BonusAgi,
        AttributeType.Aura => race.BonusAur,
        AttributeType.Carisma => race.BonusCar,
        AttributeType.Fisico => race.BonusFis,
        AttributeType.Forca => race.BonusFor,
        AttributeType.Intelecto => race.BonusInt,
        AttributeType.Percepcao => race.BonusPer,
        _ => throw new ArgumentOutOfRangeException(nameof(attributeType))
    };

    /// <summary>
    /// Calcula o valor máximo que um atributo pode alcançar (inicial + 4).
    /// </summary>
    public int GetMaximumAttributeValue(int initialValue) => initialValue + 4;

    /// <summary>
    /// Calcula o valor mínimo que um atributo pode alcançar (inicial - 2).
    /// </summary>
    public int GetMinimumAttributeValue(int initialValue) => initialValue - 2;

    /// <summary>
    /// Calcula o custo em pontos para aumentar um atributo de um valor para outro.
    /// </summary>
    /// <param name="fromValue">Valor atual do atributo</param>
    /// <param name="toValue">Valor desejado do atributo</param>
    /// <param name="initialValue">Valor inicial (com bônus racial) do atributo</param>
    /// <returns>Número de pontos necessários (pode ser negativo para redução)</returns>
    public int CalculateAttributeCost(int fromValue, int toValue, int initialValue)
    {
        if (fromValue == toValue)
            return 0;

        // Aumentando atributo
        if (toValue > fromValue)
        {
            return CalculateIncreaseCost(fromValue, toValue, initialValue);
        }

        // Reduzindo atributo
        return -CalculateDecreaseCost(fromValue, toValue);
    }

    /// <summary>
    /// Calcula o custo para aumentar um atributo.
    /// </summary>
    private int CalculateIncreaseCost(int fromValue, int toValue, int initialValue)
    {
        int cost = 0;

        for (int currentValue = fromValue; currentValue < toValue; currentValue++)
        {
            // Se estamos saindo de valores negativos, sempre custa 1 ponto
            if (currentValue < 0)
            {
                cost += 1;
            }
            else
            {
                // Para valores 0 ou positivos, custa o valor para o qual estamos indo + 1
                cost += currentValue + 1;
            }
        }

        return cost;
    }

    /// <summary>
    /// Calcula o ganho (em meios pontos) ao reduzir um atributo.
    /// Retorna 1 para cada 2 meios pontos (0.5 cada redução).
    /// </summary>
    private int CalculateDecreaseCost(int fromValue, int toValue)
    {
        // Cada redução rende 0.5 ponto
        // Como trabalhamos com inteiros, multiplicamos por 2 e depois dividimos
        int reductionLevels = fromValue - toValue;
        return reductionLevels; // Cada nível reduzido = 0.5 ponto = 1 unidade nesta escala
    }

    /// <summary>
    /// Calcula o ganho total em pontos (considerando que 2 reduções = 1 ponto).
    /// </summary>
    public decimal CalculateTotalGainFromReductions(int reductionUnits) => reductionUnits * 0.5m;

    /// <summary>
    /// Valida se um valor de atributo está dentro dos limites permitidos.
    /// </summary>
    public bool IsAttributeValueValid(int value, int initialValue)
    {
        int minimum = GetMinimumAttributeValue(initialValue);
        int maximum = GetMaximumAttributeValue(initialValue);
        return value >= minimum && value <= maximum;
    }

    /// <summary>
    /// Calcula o custo total de uma distribuição de atributos.
    /// </summary>
    public AttributeDistributionResult CalculateDistributionCost(
        Race race,
        Dictionary<AttributeType, int> desiredValues)
    {
        var result = new AttributeDistributionResult();
        int totalPointsNeeded = 0;
        decimal totalPointsGained = 0;

        foreach (var attributeType in Enum.GetValues<AttributeType>())
        {
            int initialValue = GetInitialAttributeValue(attributeType, race);
            int desiredValue = desiredValues.TryGetValue(attributeType, out var desired) ? desired : initialValue;

            // Validar limite máximo e mínimo
            if (!IsAttributeValueValid(desiredValue, initialValue))
            {
                result.Errors.Add($"{attributeType}: Valor {desiredValue} fora dos limites permitidos (mínimo {GetMinimumAttributeValue(initialValue)}, máximo {GetMaximumAttributeValue(initialValue)})");
                continue;
            }

            int cost = CalculateAttributeCost(initialValue, desiredValue, initialValue);
            
            if (cost > 0)
            {
                totalPointsNeeded += cost;
                result.PointsUsed.Add((attributeType, cost));
            }
            else if (cost < 0)
            {
                int reductionUnits = -cost;
                decimal gained = CalculateTotalGainFromReductions(reductionUnits);
                totalPointsGained += gained;
                result.PointsGained.Add((attributeType, gained));
            }

            result.FinalValues.Add(attributeType, desiredValue);
        }

        result.TotalPointsNeeded = totalPointsNeeded;
        result.TotalPointsGained = totalPointsGained;
        result.NetCost = totalPointsNeeded - (int)totalPointsGained;

        return result;
    }

    /// <summary>
    /// Valida se a distribuição de atributos é válida com os pontos disponíveis.
    /// </summary>
    public (bool isValid, string message) ValidateAttributeDistribution(
        Race race,
        Dictionary<AttributeType, int> desiredValues)
    {
        var result = CalculateDistributionCost(race, desiredValues);

        if (result.Errors.Any())
        {
            return (false, string.Join("; ", result.Errors));
        }

        int availablePoints = CalculateInitialPoints(race);
        
        if (result.NetCost > availablePoints)
        {
            return (false, $"Pontos insuficientes. Necessário: {result.NetCost}, Disponível: {availablePoints}");
        }

        return (true, "Distribuição de atributos válida");
    }
}

/// <summary>
/// Tipos de atributos disponíveis.
/// </summary>
public enum AttributeType
{
    Agilidade,
    Aura,
    Carisma,
    Fisico,
    Forca,
    Intelecto,
    Percepcao
}

/// <summary>
/// Resultado do cálculo de distribuição de atributos.
/// </summary>
public sealed class AttributeDistributionResult
{
    public Dictionary<AttributeType, int> FinalValues { get; } = new();
    public List<(AttributeType Type, int PointsUsed)> PointsUsed { get; } = new();
    public List<(AttributeType Type, decimal PointsGained)> PointsGained { get; } = new();
    public int TotalPointsNeeded { get; set; }
    public decimal TotalPointsGained { get; set; }
    public int NetCost { get; set; }
    public List<string> Errors { get; } = new();
    
    public bool IsValid => !Errors.Any();
}
