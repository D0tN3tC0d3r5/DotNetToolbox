//namespace DotNetToolbox.Data.Repositories;

//public class RepositoryTests {
//    private readonly IRepositoryStrategy _strategy;
//    private readonly Repository<Entity> _repository;

//    public RepositoryTests() {
//        _strategy = Substitute.For<IRepositoryStrategy>();
//        _repository = new(_strategy);
//    }

//    private class Entity : IEntity<string> {
//        public string Id { get; set; } = default!;
//    }

//    [Fact]
//    public void Update_CallsExecuteFunctionOnStrategy() {
//        var mockModel = new Entity();

//        _repository.Update(mockModel);

//        _strategy.Received().ExecuteFunction("Update", mockModel);
//    }

//    [Fact]
//    public void AddOrUpdate_CallsExecuteFunctionOnStrategy() {
//        var mockModel = new Entity();

//        _repository.AddOrUpdate(mockModel);

//        _strategy.Received().ExecuteFunction("AddOrUpdate", mockModel);
//    }

//    [Fact]
//    public void Patch_CallsExecuteFunctionOnStrategy() {
//        const string key = "key";
//        void Update(Entity _) { }

//        _repository.Patch(i => i.Id == key, Update);

//        _strategy.Received().ExecuteFunction<(Expression<Func<Entity, bool>>, Action<Entity>), Entity?>("Patch", Arg.Any<(Expression<Func<Entity, bool>>, Action<Entity>)>());
//    }

//    [Fact]
//    public void CreateOrPatch_CallsExecuteFunctionOnStrategy() {
//        const string key = "key";
//        static void Update(Entity _) { }

//        _repository.PatchOrCreate(i => i.Id == key, Update);

//        _strategy.Received().ExecuteFunction<(Expression<Func<Entity, bool>>, Action<Entity>), Entity?>("PatchOrCreate", Arg.Any<(Expression<Func<Entity, bool>>, Action<Entity>)>());
//    }

//    [Fact]
//    public void Remove_CallsExecuteFunctionOnStrategy() {
//        var key = "key";

//        _repository.Remove(i => i.Id == key);

//        _strategy.Received().ExecuteFunction("Remove", Arg.Any<Expression<Func<Entity, bool>>>());
//    }
//}
