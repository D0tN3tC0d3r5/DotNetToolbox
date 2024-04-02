namespace Sophia.Data.Users;

public class UsersRepository(ApplicationDbContext dbContext)
    : EntityFrameworkRepository<UserData, User, string>(dbContext.Users) {
    protected override Expression<Func<User, UserData>> ProjectTo { get; }
        = input => Mapper.ToUserData(input);
    protected override Action<UserData, User> UpdateFrom { get; }
        = Mapper.UpdateUser;
    protected override Func<UserData, User> CreateFrom { get; }
        = Mapper.ToUser;
}
