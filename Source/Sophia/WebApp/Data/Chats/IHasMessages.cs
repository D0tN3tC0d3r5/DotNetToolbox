
namespace Sophia.WebApp.Data.Chats;

public interface IHasMessages {
    string Id { get; set; }
    List<MessageEntity> Messages { get; set; }
}
