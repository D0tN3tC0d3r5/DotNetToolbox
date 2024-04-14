// ReSharper disable once CheckNamespace - Intended to be in this namespace

namespace System.Linq.Expressions;

public record LambdaConversionTypeMapper(Type SourceType, Type TargetType, Func<object, object>? Convert = null)
    : ILambdaConversionTypeMapper {
    public LambdaConversionTypeMapper(Expression expression, Type targetType, Func<object, object>? convert = null)
        : this(expression.Type, targetType, convert) {
        SourceType = expression.Type;
    }

    public void Deconstruct(out Type sourceType, out Type targetType, out Func<object, object>? convert) {
        convert = Convert;
        sourceType = SourceType;
        targetType = TargetType;
    }
}

public record LambdaConversionTypeMapper<TSource, TTarget>
    : LambdaConversionTypeMapper {

    public LambdaConversionTypeMapper(Func<TSource, TTarget>? convert = null)
        : base(typeof(TSource), typeof(TTarget), convert is null ? null : s => convert((TSource)s)!) {
    }
}
