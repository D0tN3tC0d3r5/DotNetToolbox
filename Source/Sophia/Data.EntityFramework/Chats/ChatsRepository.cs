namespace Sophia.Data.Chats;

public class ChatsRepository(ApplicationDbContext dbContext)
    : EntityFrameworkRepository<ChatData, ChatEntity, Guid>(dbContext.Chats) {
    protected override Expression<Func<ChatEntity, ChatData>> ProjectTo { get; }
        = input => Mapper.ToChatData(input);
    protected override Action<ChatData, ChatEntity> UpdateFrom { get; }
        = Mapper.UpdateChatEntity;
    protected override Func<ChatData, ChatEntity> CreateFrom { get; }
        = Mapper.ToChatEntity;
}
