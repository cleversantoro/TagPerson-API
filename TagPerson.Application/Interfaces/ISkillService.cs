using TagPerson.Application.DTOs;

namespace TagPerson.Application.Interfaces;

public interface ISkillService
{
    Task<IReadOnlyList<SkillGroupDto>> GetGroupParentsAsync(CancellationToken ct);
    Task<IReadOnlyList<SkillGroupDto>> GetGroupChildrenAsync(int parentId, CancellationToken ct);
    Task<IReadOnlyList<SkillFromGroupDto>> GetSkillsFromGroupAsync(int groupId, CancellationToken ct);
    Task<IReadOnlyList<SkillSpecializationSuggestionDto>> GetSpecializationSuggestionsAsync(int skillId, CancellationToken ct);
}
