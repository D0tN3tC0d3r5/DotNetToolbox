namespace Sophia.Data.Users;

public class UsersDbSet(ApplicationDbContext dbContext)
    : UserRepository {
    public override async Task<UserData?> FindByKey(string key, CancellationToken ct = default) {
        var entity = await dbContext.Users
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(i => i.Id == key, ct);
        return entity == null
            ? null
            : Mapper.ToUserData(entity);
    }
}
