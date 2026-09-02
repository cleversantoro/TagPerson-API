using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;

namespace TagPerson.Application.Services;

public sealed class CharacterSheetPdfService : ICharacterSheetPdfService
{
    public byte[] Generate(CharacterSheetDto sheet)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        return Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Margin(24);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(style => style.FontSize(9));
                page.Header().Text($"Ficha de Personagem - {sheet.Name}").Bold().FontSize(18);
                page.Content().Column(column =>
                {
                    column.Spacing(10);
                    AddIdentification(column, sheet);
                    AddAttributes(column, sheet);
                    AddBudget(column, sheet.Budget);
                    AddDerived(column, sheet);
                    AddItems(column, "Perícias", sheet.Skills.Select(item => $"{item.Name} (Nível {item.Level ?? 0})"));
                    AddItems(column, "Técnicas de Combate", sheet.Combat.Select(item => $"{item.CombatName ?? "Sem nome"} (Nível {item.Level ?? 0})"));
                    AddItems(column, "Magias", sheet.Spells.Select(item => $"{item.Name} (Nível {item.Level ?? 0})"));
                    AddItems(column, "Equipamentos", sheet.Equipments.Select(item => $"{item.Name} x{item.Qty ?? 0}"));
                    AddItems(column, "Caracterizações", sheet.Characterizations.Select(item => $"{item.Name} (Nível {item.Level ?? 0})"));
                    AddFeatures(column, sheet);
                });
                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("TagPerson - página ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        }).GeneratePdf();
    }

    private static void AddIdentification(ColumnDescriptor column, CharacterSheetDto sheet)
    {
        AddSection(column, "Identificação", new[]
        {
            $"Jogador: {sheet.Player ?? ""}",
            $"Raça: {sheet.Race?.Name ?? ""}",
            $"Profissão: {sheet.Profession?.Name ?? ""}",
            $"Nível: {sheet.Level ?? 0}",
            $"Experiência: {sheet.Experience ?? 0}",
            $"Especialização: {sheet.Specialization?.Name ?? ""}",
            $"Divindade: {sheet.Deity?.Name ?? ""}",
            $"Classe social: {sheet.ClassSocial?.Name ?? ""}"
        });
    }

    private static void AddAttributes(ColumnDescriptor column, CharacterSheetDto sheet)
    {
        AddSection(column, "Atributos", new[]
        {
            $"Agilidade: {sheet.Attributes.Agi ?? 0}",
            $"Percepção: {sheet.Attributes.Per ?? 0}",
            $"Intelecto: {sheet.Attributes.Int ?? 0}",
            $"Aura: {sheet.Attributes.Aur ?? 0}",
            $"Carisma: {sheet.Attributes.Car ?? 0}",
            $"Força: {sheet.Attributes.For ?? 0}",
            $"Físico: {sheet.Attributes.Fis ?? 0}"
        });
    }

    private static void AddDerived(ColumnDescriptor column, CharacterSheetDto sheet)
    {
        var stats = sheet.Derived;
        AddSection(column, "Valores Derivados", new[]
        {
            $"Resistência Física: {stats.ResistenciaFisica}",
            $"Resistência Mágica: {stats.ResistenciaMagica}",
            $"Velocidade: {stats.Velocidade}",
            $"Karma: {stats.Karma}",
            $"Defesa Ativa: {stats.DefesaAtiva}",
            $"Defesa Passiva: {stats.DefesaPassiva}",
            $"Absorção: {stats.Absorcao}",
            $"Pontos de Magia: {stats.PontosMagia}",
            $"Energia Física Máxima: {stats.MaxEf}"
        });
    }

    private static void AddBudget(ColumnDescriptor column, CharacterPointBudgetDto budget)
    {
        AddSection(column, "Orçamento de Pontos", new[]
        {
            Allocation("Atributos", budget.Attributes),
            Allocation("Perícias", budget.Skills),
            Allocation("Armas", budget.Weapons),
            Allocation("Combate", budget.Combat),
            Allocation("Magia", budget.Magic)
        });
    }

    private static string Allocation(string name, PointAllocationDto allocation) =>
        $"{name}: {allocation.Used} usados de {allocation.Granted}; restantes: {allocation.Remaining}";

    private static void AddFeatures(ColumnDescriptor column, CharacterSheetDto sheet)
    {
        AddSection(column, "Características e Moedas", new[]
        {
            $"Idade: {sheet.Features.Age ?? 0}",
            $"Altura: {sheet.Features.Height ?? 0}",
            $"Peso: {sheet.Features.Weight ?? 0}",
            $"Olhos: {sheet.Features.Eyes ?? ""}",
            $"Cabelo: {sheet.Features.Hair ?? ""}",
            $"Pele: {sheet.Features.Skin ?? ""}",
            $"Aparência: {sheet.Features.Appearance ?? ""}",
            $"História: {sheet.Features.History ?? ""}",
            $"Moedas: {sheet.Coins.Copper ?? 0} cobre, {sheet.Coins.Silver ?? 0} prata, {sheet.Coins.Gold ?? 0} ouro"
        });
    }

    private static void AddItems(ColumnDescriptor column, string title, IEnumerable<string> items)
    {
        var values = items.ToList();
        AddSection(column, title, values.Count == 0 ? new[] { "Nenhum item registrado." } : values);
    }

    private static void AddSection(ColumnDescriptor column, string title, IEnumerable<string> values)
    {
        column.Item().PaddingTop(4).Text(title).Bold().FontSize(12);
        column.Item().Border(1).BorderColor(Colors.Grey.Lighten1).Padding(6).Column(content =>
        {
            foreach (var value in values)
            {
                content.Item().Text(value);
            }
        });
    }
}