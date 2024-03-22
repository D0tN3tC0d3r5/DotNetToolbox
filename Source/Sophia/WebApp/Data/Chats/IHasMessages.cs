
namespace Sophia.WebApp.Data.Chats;

public interface IHasMessages {
    int Id { get; set; }
    List<MessageEntity> Messages { get; set; }
}