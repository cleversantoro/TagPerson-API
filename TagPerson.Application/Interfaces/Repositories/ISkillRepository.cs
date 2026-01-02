using TagPerson.Application.DTOs;
using TagPerson.Domain.Entities;

namespace TagPerson.Application.Interfaces.Repositories;

public interface ISkillRepository
{
    Task<IReadOnlyList<SkillGroup>> GetGroupParentsAsync(CancellationToken ct);
    Task<IReadOnlyList<SkillGroup>> GetGroupChildrenAsync(int parentId, CancellationToken ct);
    Task<IReadOnlyList<SkillFromGroupDto>> GetSkillsFromGroupAsync(int groupId, CancellationToken ct);
    Task<IReadOnlyList<SkillSpecializationSuggestionDto>> GetSpecializationSuggestionsAsync(int skillId, CancellationToken ct);
    Task<IReadOnlyList<SkillImprovedDto>> GetImprovedAsync(int skillId, CancellationToken ct);
}
