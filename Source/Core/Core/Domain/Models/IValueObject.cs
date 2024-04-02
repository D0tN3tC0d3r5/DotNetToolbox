namespace DotNetToolbox.Domain.Models;

public interface IValueObject;

public interface IValueObject<TValueObject>
    : IValueObject
    where TValueObject : IValueObject<TValueObject>;
