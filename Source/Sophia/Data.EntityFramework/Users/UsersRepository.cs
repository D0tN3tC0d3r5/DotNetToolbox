﻿namespace Sophia.Data.Users;

public class UsersRepository(DataContext dataContext, ApplicationDbContext dbContext)
    : EntityFrameworkRepository<UserData, User, string>(dataContext, dbContext.Users) {
    protected override Expression<Func<User, UserData>> Project { get; }
        = input => Mapper.ToUserData(input);
    protected override Action<UserData, User> UpdateFrom { get; }
        = Mapper.UpdateUser;
    protected override Func<UserData, User> Create { get; }
        = Mapper.ToUser;
}