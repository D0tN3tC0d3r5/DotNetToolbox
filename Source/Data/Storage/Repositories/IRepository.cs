namespace DotNetToolbox.Data.Repositories;

public partial interface IRepository;

public partial interface IRepository<TItem>
    : IRepository;
