﻿namespace Sophia.Services.WorldServiceModel;

public class SkillData {
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<ArgumentData> Arguments { get; set; } = [];

    public Skill ToModel(IEnumerable<Skill> list) {
        var original = list.FirstOrDefault(i => i.Id == Id);
        return original is not null
                   ? GetUpdatedModel(original)
                   : new() {
                       Id = Id,
                       Name = Name,
                       Description = Description,
                       Arguments = Arguments.ToList(x => x.ToModel())
                   };
    }

    private Skill GetUpdatedModel(Skill skill) {
        skill.Name = Name;
        skill.Description = Description;
        skill.Arguments = Arguments.ToList(x => x.ToModel());
        return skill;
    }
}
