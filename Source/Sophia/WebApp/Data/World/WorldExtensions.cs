namespace Sophia.WebApp.Data.World;

public static class WorldExtensions {
    public static WorldEntity ToEntity(this DotNetToolbox.AI.Shared.World input)
        => new() {
                     DateTime = input.DateTime,
                     Location = input.Location,
                     UserProfile = input.UserProfile,
                 };

    public static SkillEntity ToEntity(this Skill input)
        => new() {
                     Id = input.Id,
                     Name = input.Name,
                     Description = input.Description,
                 };

    public static ArgumentEntity ToEntity(this Argument input)
        => new() {
                     Name = input.Name,
                     Description = input.Description,
                     Type = input.Type,
                     Options = input.Options,
                     IsRequired = input.IsRequired,
                 };

    public static InformationEntity ToEntity(this Information input)
        => new() {
                     DefaultText = input.DefaultText,
                     Value = input.Value,
                     ValueTemplate = input.ValueTemplate,
                 };
}
