namespace Sophia.WebApp.Data.World;

public class WorldRepository(ApplicationDbContext dbContext, DotNetToolbox.AI.Chats.World world)
    : IWorldRepository {
    public DotNetToolbox.AI.Chats.World GetWorld() {
        var result = new DotNetToolbox.AI.Chats.World();
        var entity = dbContext.Worlds.FirstOrDefault();

        if (entity is null) return result;
        result.UserName = result.UserName;
        result.Location = result.Location;
        result.CustomValues = result.CustomValues;
        result.Skills = result.Skills;
        return result;
    }

    public void UpdateWorld(DotNetToolbox.AI.Chats.World input) {
        var result = input.Validate();
        if (!result.IsSuccess) return;
        world.UserName = input.UserName;
        world.Location = input.Location;
        world.CustomValues = input.CustomValues;
        world.Skills = input.Skills;

        var entity = new WorldEntity {
            UserName = world.UserName,
            Location = world.Location,
            CustomValues = world.CustomValues.ToList(i => new InformationEntity {
                Value = i.Value,
                ValueTemplate = i.ValueTemplate,
                NullText = i.DefaultText,
            }),
            Skills = world.Skills.ToList(i => new SkillEntity {
                Name = i.Name,
                Description = i.Description,
                Arguments = i.Arguments.ToList(a => new ArgumentEntity {
                    Name = a.Name,
                    Description = a.Description,
                    IsRequired = a.IsRequired,
                    Type = a.Type,
                    Options = a.Options ?? [],
                }),
            }),
        };
        dbContext.Worlds.Update(entity);
        dbContext.SaveChanges();
    }
}
