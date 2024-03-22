
namespace Sophia.WebApp.Data.Providers;

public interface IHasModels {
    int Id { get; set; }
    List<ModelEntity> Models { get; set; }
}