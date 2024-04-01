namespace Sophia.Data.Chats;

public class ChatsRepository(DataContext dataContext, ApplicationDbContext dbContext)
    : EntityFrameworkRepository<ChatData, ChatEntity, Guid>(dataContext, dbContext.Chats) {
    protected override Expression<Func<ChatEntity, ChatData>> Project { get; }
        = input => Mapper.ToChatData(input);
    protected override Action<ChatData, ChatEntity> UpdateFrom { get; }
        = Mapper.UpdateChatEntity;
    protected override Func<ChatData, ChatEntity> Create { get; }
        = Mapper.ToChatEntity;
}
