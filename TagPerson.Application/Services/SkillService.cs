using TagPerson.Application.DTOs;
using TagPerson.Application.Interfaces;
using TagPerson.Application.Interfaces.Repositories;

namespace TagPerson.Application.Services;

public sealed class SkillService : ISkillService
{
    private readonly ISkillRepository _repo;

    public SkillService(ISkillRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyList<SkillGroupDto>> GetGroupParentsAsync(CancellationToken ct)
    {
        var items = await _repo.GetGroupParentsAsync(ct);
        return items.Select(x => new SkillGroupDto(x.Id, x.Name, x.ParentId)).ToList();
    }

    public async Task<IReadOnlyList<SkillGroupDto>> GetGroupChildrenAsync(int parentId, CancellationToken ct)
    {
        var items = await _repo.GetGroupChildrenAsync(parentId, ct);
        return items.Select(x => new SkillGroupDto(x.Id, x.Name, x.ParentId)).ToList();
    }

    public async Task<IReadOnlyList<SkillFromGroupDto>> GetSkillsFromGroupAsync(int groupId, CancellationToken ct)
    {
        return await _repo.GetSkillsFromGroupAsync(groupId, ct);
    }

    public async Task<IReadOnlyList<SkillSpecializationSuggestionDto>> GetSpecializationSuggestionsAsync(int skillId, CancellationToken ct)
    {
        return await _repo.GetSpecializationSuggestionsAsync(skillId, ct);
    }

    public async Task<IReadOnlyList<SkillImprovedDto>> GetImprovedAsync(int skillId, CancellationToken ct)
    {
        return await _repo.GetImprovedAsync(skillId, ct);
    }

}
