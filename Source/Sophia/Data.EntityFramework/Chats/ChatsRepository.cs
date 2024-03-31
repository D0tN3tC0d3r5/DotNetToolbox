namespace Sophia.Data.Chats;

public class ChatsRepository(DataContext dataContext, DbContext dbContext)
    : EntityFrameworkRepository<ChatData, Guid, ChatEntity, Guid>(dataContext, dbContext) {
    //protected override Expression<Func<ChatEntity, ChatData>> Project { get; } =
    //    input => Mapper.ToChatData(input);
    //protected override Action<ChatData, ChatEntity> UpdateFrom { get; }
    //    = Mapper.UpdateChatEntity;
}
