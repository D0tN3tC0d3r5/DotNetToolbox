// ReSharper disable once CheckNamespace - This is intended
namespace System.Linq.Async;

public static class QueryableMethods {
    private static Dictionary<Type, MethodInfo> AverageWithoutSelectorMethods { get; } = [];
    private static Dictionary<Type, MethodInfo> AverageWithSelectorMethods { get; } = [];
    private static Dictionary<Type, MethodInfo> SumWithoutSelectorMethods { get; } = [];
    private static Dictionary<Type, MethodInfo> SumWithSelectorMethods { get; } = [];

    private static readonly Dictionary<string, List<MethodInfo>> _queryableMethodGroups = typeof(Queryable)
                                                                                         .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                                                                                         .GroupBy(mi => mi.Name)
                                                                                         .ToDictionary(e => e.Key, l => l.ToList());

    private static MethodInfo GetMethod(string name, int genericParameterCount, Func<Type[], Type[]> parameterGenerator)
        => _queryableMethodGroups[name].Single(mi => ((genericParameterCount == 0 && !mi.IsGenericMethod)
                                                  || (mi.IsGenericMethod && mi.GetGenericArguments().Length == genericParameterCount))
                                                 && mi.GetParameters().Select(e => e.ParameterType).SequenceEqual(parameterGenerator(mi.IsGenericMethod ? mi.GetGenericArguments() : [])));

    public static bool IsAverageWithoutSelector(MethodInfo methodInfo)
        => AverageWithoutSelectorMethods.ContainsValue(methodInfo);
    public static bool IsAverageWithSelector(MethodInfo methodInfo)
        => methodInfo.IsGenericMethod
        && AverageWithSelectorMethods.ContainsValue(methodInfo.GetGenericMethodDefinition());
    public static bool IsSumWithoutSelector(MethodInfo methodInfo)
        => SumWithoutSelectorMethods.ContainsValue(methodInfo);
    public static bool IsSumWithSelector(MethodInfo methodInfo)
        => methodInfo.IsGenericMethod
        && SumWithSelectorMethods.ContainsValue(methodInfo.GetGenericMethodDefinition());
    public static MethodInfo GetAverageWithoutSelector(Type type)
        => AverageWithoutSelectorMethods[type];
    public static MethodInfo GetAverageWithSelector(Type type)
        => AverageWithSelectorMethods[type];
    public static MethodInfo GetSumWithoutSelector(Type type)
        => SumWithoutSelectorMethods[type];
    public static MethodInfo GetSumWithSelector(Type type)
        => SumWithSelectorMethods[type];

    public static MethodInfo All { get; } = GetMethod(nameof(Queryable.All), 1,
                        types => [
                            typeof(IQueryable<>).MakeGenericType(types[0]),
                            typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                        ]);

    public static MethodInfo AnyWithoutPredicate { get; } = GetMethod(nameof(Queryable.Any), 1,
                                        types => [typeof(IQueryable<>).MakeGenericType(types[0])]);
    public static MethodInfo AnyWithPredicate { get; } = GetMethod(nameof(Queryable.Any), 1,
                                     types => [
                                         typeof(IQueryable<>).MakeGenericType(types[0]),
                                         typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                                     ]);
    public static MethodInfo AsQueryable { get; } = GetMethod(nameof(Queryable.AsQueryable), 1,
                                types => [typeof(IEnumerable<>).MakeGenericType(types[0])]);
    public static MethodInfo Cast { get; } = GetMethod(nameof(Queryable.Cast), 1, types => [typeof(IQueryable)]);
    public static MethodInfo Concat { get; } = GetMethod(nameof(Queryable.Concat), 1,
                           types => [typeof(IQueryable<>).MakeGenericType(types[0]), typeof(IEnumerable<>).MakeGenericType(types[0])]);
    public static MethodInfo Contains { get; } = GetMethod(nameof(Queryable.Contains), 1,
                             types => [typeof(IQueryable<>).MakeGenericType(types[0]), types[0]]);
    public static MethodInfo CountWithoutPredicate { get; } = GetMethod(nameof(Queryable.Count), 1,
                                          types => [typeof(IQueryable<>).MakeGenericType(types[0])]);
    public static MethodInfo CountWithPredicate { get; } = GetMethod(nameof(Queryable.Count), 1,
                                       types => [
                                           typeof(IQueryable<>).MakeGenericType(types[0]),
                                           typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                                       ]);
    public static MethodInfo DefaultIfEmptyWithoutArgument { get; } = GetMethod(nameof(Queryable.DefaultIfEmpty), 1,
                                                  types => [typeof(IQueryable<>).MakeGenericType(types[0])]);
    public static MethodInfo DefaultIfEmptyWithArgument { get; } = GetMethod(nameof(Queryable.DefaultIfEmpty), 1,
                                               types => [typeof(IQueryable<>).MakeGenericType(types[0]), types[0]]);
    public static MethodInfo Distinct { get; } = GetMethod(nameof(Queryable.Distinct), 1, types => [typeof(IQueryable<>).MakeGenericType(types[0])]);
    public static MethodInfo ElementAt { get; } = GetMethod(nameof(Queryable.ElementAt), 1,
                              types => [typeof(IQueryable<>).MakeGenericType(types[0]), typeof(int)]);
    public static MethodInfo ElementAtOrDefault { get; } = GetMethod(nameof(Queryable.ElementAtOrDefault), 1,
                                       types => [typeof(IQueryable<>).MakeGenericType(types[0]), typeof(int)]);
    public static MethodInfo Except { get; } = GetMethod(nameof(Queryable.Except), 1,
                           types => [typeof(IQueryable<>).MakeGenericType(types[0]), typeof(IEnumerable<>).MakeGenericType(types[0])]);
    public static MethodInfo FirstWithoutPredicate { get; } = GetMethod(nameof(Queryable.First), 1, types => [typeof(IQueryable<>).MakeGenericType(types[0])]);
    public static MethodInfo FirstWithPredicate { get; } = GetMethod(nameof(Queryable.First), 1,
                                       types => [
                                                    typeof(IQueryable<>).MakeGenericType(types[0]),
                                                    typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                                                ]);
    public static MethodInfo FirstOrDefaultWithoutPredicate { get; } = GetMethod(nameof(Queryable.FirstOrDefault), 1,
                                                   types => [typeof(IQueryable<>).MakeGenericType(types[0])]);
    public static MethodInfo FirstOrDefaultWithPredicate { get; } = GetMethod(nameof(Queryable.FirstOrDefault), 1,
                                                types => [
                                                             typeof(IQueryable<>).MakeGenericType(types[0]),
                                                             typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                                                         ]);
    public static MethodInfo GroupByWithKeySelector { get; } = GetMethod(nameof(Queryable.GroupBy), 2,
                                           types => [
                                               typeof(IQueryable<>).MakeGenericType(types[0]),
                                               typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1]))
                                           ]);
    public static MethodInfo GroupByWithKeyElementSelector { get; } = GetMethod(nameof(Queryable.GroupBy), 3,
                                                  types => [
                                                      typeof(IQueryable<>).MakeGenericType(types[0]),
                                                      typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1])),
                                                      typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[2]))
                                                  ]);
    public static MethodInfo GroupByWithKeyElementResultSelector { get; } = GetMethod(nameof(Queryable.GroupBy), 4,
                                                        types => [
                                                            typeof(IQueryable<>).MakeGenericType(types[0]),
                                                            typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1])),
                                                            typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[2])),
                                                            typeof(Expression<>).MakeGenericType(typeof(Func<,,>).MakeGenericType(types[1], typeof(IEnumerable<>).MakeGenericType(types[2]), types[3]))
                                                        ]);
    public static MethodInfo GroupByWithKeyResultSelector { get; } = GetMethod(nameof(Queryable.GroupBy), 3,
                                                 types => [
                                                     typeof(IQueryable<>).MakeGenericType(types[0]),
                                                     typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1])),
                                                     typeof(Expression<>).MakeGenericType(typeof(Func<,,>).MakeGenericType(types[1], typeof(IEnumerable<>).MakeGenericType(types[0]), types[2]))
                                                 ]);
    public static MethodInfo GroupJoin { get; } = GetMethod(nameof(Queryable.GroupJoin), 4,
                              types => [
                                  typeof(IQueryable<>).MakeGenericType(types[0]),
                                  typeof(IEnumerable<>).MakeGenericType(types[1]),
                                  typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[2])),
                                  typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[1], types[2])),
                                  typeof(Expression<>).MakeGenericType(typeof(Func<,,>).MakeGenericType(types[0], typeof(IEnumerable<>).MakeGenericType(types[1]), types[3]))
                              ]);
    public static MethodInfo Intersect { get; } = GetMethod(nameof(Queryable.Intersect), 1,
                              types => [typeof(IQueryable<>).MakeGenericType(types[0]), typeof(IEnumerable<>).MakeGenericType(types[0])]);
    public static MethodInfo Join { get; } = GetMethod(nameof(Queryable.Join), 4,
                         types => [
                             typeof(IQueryable<>).MakeGenericType(types[0]),
                             typeof(IEnumerable<>).MakeGenericType(types[1]),
                             typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[2])),
                             typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[1], types[2])),
                             typeof(Expression<>).MakeGenericType(typeof(Func<,,>).MakeGenericType(types[0], types[1], types[3]))
                         ]);
    public static MethodInfo LastWithoutPredicate { get; } = GetMethod(nameof(Queryable.Last), 1, types => [typeof(IQueryable<>).MakeGenericType(types[0])]);
    public static MethodInfo LastWithPredicate { get; } = GetMethod(nameof(Queryable.Last), 1,
                                      types => [
                                          typeof(IQueryable<>).MakeGenericType(types[0]),
                                          typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                                      ]);
    public static MethodInfo LastOrDefaultWithoutPredicate { get; } = GetMethod(nameof(Queryable.LastOrDefault), 1,
                                                  types => [typeof(IQueryable<>).MakeGenericType(types[0])]);
    public static MethodInfo LastOrDefaultWithPredicate { get; } = GetMethod(nameof(Queryable.LastOrDefault), 1,
                                               types => [
                                                   typeof(IQueryable<>).MakeGenericType(types[0]),
                                                   typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                                               ]);
    public static MethodInfo LongCountWithoutPredicate { get; } = GetMethod(nameof(Queryable.LongCount), 1,
                                              types => [typeof(IQueryable<>).MakeGenericType(types[0])]);
    public static MethodInfo LongCountWithPredicate { get; } = GetMethod(nameof(Queryable.LongCount), 1,
                                           types => [
                                               typeof(IQueryable<>).MakeGenericType(types[0]),
                                               typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                                           ]);
    public static MethodInfo MaxWithoutSelector { get; } = GetMethod(nameof(Queryable.Max), 1, types => [typeof(IQueryable<>).MakeGenericType(types[0])]);
    public static MethodInfo MaxWithSelector { get; } = GetMethod(nameof(Queryable.Max), 2,
                                    types => [
                                        typeof(IQueryable<>).MakeGenericType(types[0]),
                                        typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1]))
                                    ]);
    public static MethodInfo MinWithoutSelector { get; } = GetMethod(nameof(Queryable.Min), 1, types => [typeof(IQueryable<>).MakeGenericType(types[0])]);
    public static MethodInfo MinWithSelector { get; } = GetMethod(nameof(Queryable.Min), 2,
                                    types => [
                                        typeof(IQueryable<>).MakeGenericType(types[0]),
                                        typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1]))
                                    ]);
    public static MethodInfo OfType { get; } = GetMethod(nameof(Queryable.OfType), 1, types => [typeof(IQueryable)]);
    public static MethodInfo OrderBy { get; } = GetMethod(nameof(Queryable.OrderBy), 2,
                            types => [
                                typeof(IQueryable<>).MakeGenericType(types[0]),
                                typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1]))
                            ]);
    public static MethodInfo OrderByDescending { get; } = GetMethod(nameof(Queryable.OrderByDescending), 2,
                                      types => [
                                          typeof(IQueryable<>).MakeGenericType(types[0]),
                                          typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1]))
                                      ]);
    public static MethodInfo Reverse { get; } = GetMethod(nameof(Queryable.Reverse), 1, types => [typeof(IQueryable<>).MakeGenericType(types[0])]);
    public static MethodInfo Select { get; } = GetMethod(nameof(Queryable.Select), 2,
                           types => [
                               typeof(IQueryable<>).MakeGenericType(types[0]),
                               typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1]))
                           ]);
    public static MethodInfo SelectManyWithoutCollectionSelector { get; } = GetMethod(nameof(Queryable.SelectMany), 2,
                                                        types => [
                                                            typeof(IQueryable<>).MakeGenericType(types[0]),
                                                            typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(IEnumerable<>).MakeGenericType(types[1])))
                                                        ]);
    public static MethodInfo SelectManyWithCollectionSelector { get; } = GetMethod(nameof(Queryable.SelectMany), 3,
                                                     types => [
                                                         typeof(IQueryable<>).MakeGenericType(types[0]),
                                                         typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(IEnumerable<>).MakeGenericType(types[1]))),
                                                         typeof(Expression<>).MakeGenericType(typeof(Func<,,>).MakeGenericType(types[0], types[1], types[2]))
                                                     ]);
    public static MethodInfo SingleWithoutPredicate { get; } = GetMethod(nameof(Queryable.Single), 1, types => [typeof(IQueryable<>).MakeGenericType(types[0])]);
    public static MethodInfo SingleWithPredicate { get; } = GetMethod(nameof(Queryable.Single), 1,
                                        types => [
                                            typeof(IQueryable<>).MakeGenericType(types[0]),
                                            typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                                        ]);
    public static MethodInfo SingleOrDefaultWithoutPredicate { get; } = GetMethod(nameof(Queryable.SingleOrDefault), 1,
                                                    types => [typeof(IQueryable<>).MakeGenericType(types[0])]);
    public static MethodInfo SingleOrDefaultWithPredicate { get; } = GetMethod(nameof(Queryable.SingleOrDefault), 1,
                                                 types => [
                                                     typeof(IQueryable<>).MakeGenericType(types[0]),
                                                     typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                                                 ]);
    public static MethodInfo Skip { get; } = GetMethod(nameof(Queryable.Skip), 1,
                         types => [typeof(IQueryable<>).MakeGenericType(types[0]), typeof(int)]);
    public static MethodInfo SkipWhile { get; } = GetMethod(nameof(Queryable.SkipWhile), 1,
                              types => [
                                  typeof(IQueryable<>).MakeGenericType(types[0]),
                                  typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                              ]);
    public static MethodInfo Take { get; } = GetMethod(nameof(Queryable.Take), 1,
                         types => [typeof(IQueryable<>).MakeGenericType(types[0]), typeof(int)]);
    public static MethodInfo TakeWhile { get; } = GetMethod(nameof(Queryable.TakeWhile), 1,
                              types => [
                                  typeof(IQueryable<>).MakeGenericType(types[0]),
                                  typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                              ]);
    public static MethodInfo ThenBy { get; } = GetMethod(nameof(Queryable.ThenBy), 2,
                           types => [
                               typeof(IOrderedQueryable<>).MakeGenericType(types[0]),
                               typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1]))
                           ]);
    public static MethodInfo ThenByDescending { get; } = GetMethod(nameof(Queryable.ThenByDescending), 2,
                                     types => [
                                         typeof(IOrderedQueryable<>).MakeGenericType(types[0]),
                                         typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1]))
                                     ]);
    public static MethodInfo Union { get; } = GetMethod(nameof(Queryable.Union), 1,
                          types => [typeof(IQueryable<>).MakeGenericType(types[0]), typeof(IEnumerable<>).MakeGenericType(types[0])]);
    public static MethodInfo Where { get; } = GetMethod(nameof(Queryable.Where), 1,
                          types => [
                              typeof(IQueryable<>).MakeGenericType(types[0]),
                              typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                          ]);

    static QueryableMethods() {
        var numericTypes = new[] {
                               typeof(int),
                               typeof(int?),
                               typeof(long),
                               typeof(long?),
                               typeof(float),
                               typeof(float?),
                               typeof(double),
                               typeof(double?),
                               typeof(decimal),
                               typeof(decimal?)
                           };
        foreach (var type in numericTypes) {
            AverageWithoutSelectorMethods[type] = GetMethod(nameof(Queryable.Average), 0, types => [typeof(IQueryable<>).MakeGenericType(type)]);
            AverageWithSelectorMethods[type] = GetMethod(nameof(Queryable.Average), 1,
                                                         types => [
                                                             typeof(IQueryable<>).MakeGenericType(types[0]),
                                                             typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], type))
                                                         ]);
            SumWithoutSelectorMethods[type] = GetMethod(nameof(Queryable.Sum), 0, types => [typeof(IQueryable<>).MakeGenericType(type)]);
            SumWithSelectorMethods[type] = GetMethod(nameof(Queryable.Sum), 1,
                                                     types => [
                                                         typeof(IQueryable<>).MakeGenericType(types[0]),
                                                         typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], type))
                                                     ]);
        }
    }
}
