// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Linq.Expressions;

public record LambdaConversionTypeMapper<TSource, TTarget>
    : ILambdaConversionTypeMapper {
    private readonly Func<TSource?, TTarget?>? _convert;

    public LambdaConversionTypeMapper(Func<TSource?, TTarget?>? convert = null) {
        _convert = convert;
        SourceType = typeof(TSource);
        TargetType = typeof(TTarget);
    }

    public Type SourceType { get; }
    public Type TargetType { get; }
    object? ILambdaConversionTypeMapper.Convert(object? input) => Convert((TSource?)input);

    public TTarget? Convert(TSource? input)
        => _convert != null
            ? _convert.Invoke(input)
            : throw new NotImplementedException();

    public void Deconstruct(out Type sourceType, out Type targetType, out Func<TSource?, TTarget?> convert) {
        convert = Convert;
        sourceType = SourceType;
        targetType = TargetType;
    }
}
