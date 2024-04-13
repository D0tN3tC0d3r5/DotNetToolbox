// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Expressions;

public interface ILambdaConversionTypeMapper {
    Type TargetType { get; }
    Type SourceType { get; }
    Func<object?, object?>? Convert { get; }
}
