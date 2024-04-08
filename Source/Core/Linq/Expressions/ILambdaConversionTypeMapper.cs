// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Expressions;

public interface ILambdaConversionTypeMapper {
    Type SourceType { get; }
    Type TargetType { get; }
    object? Convert(object? input);
}
