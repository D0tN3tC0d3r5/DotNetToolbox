namespace DotNetToolbox.Collections.Generic;

public static class QueryableMethods {
    public static MethodInfo All { get; }

    public static MethodInfo AnyWithoutPredicate { get; }

    public static MethodInfo AnyWithPredicate { get; }

    public static MethodInfo AsQueryable { get; }

    public static MethodInfo Cast { get; }

    public static MethodInfo Concat { get; }

    public static MethodInfo Contains { get; }

    public static MethodInfo CountWithoutPredicate { get; }

    public static MethodInfo CountWithPredicate { get; }

    public static MethodInfo DefaultIfEmptyWithoutArgument { get; }

    public static MethodInfo DefaultIfEmptyWithArgument { get; }

    public static MethodInfo Distinct { get; }

    public static MethodInfo ElementAt { get; }

    public static MethodInfo ElementAtOrDefault { get; }

    public static MethodInfo Except { get; }

    public static MethodInfo FirstWithoutPredicate { get; }

    public static MethodInfo FirstWithPredicate { get; }

    public static MethodInfo FirstOrDefaultWithoutPredicate { get; }

    public static MethodInfo FirstOrDefaultWithPredicate { get; }

    public static MethodInfo GroupByWithKeySelector { get; }

    public static MethodInfo GroupByWithKeyElementSelector { get; }

    public static MethodInfo GroupByWithKeyElementResultSelector { get; }

    public static MethodInfo GroupByWithKeyResultSelector { get; }

    public static MethodInfo GroupJoin { get; }

    public static MethodInfo Intersect { get; }

    public static MethodInfo Join { get; }

    public static MethodInfo LastWithoutPredicate { get; }

    public static MethodInfo LastWithPredicate { get; }

    public static MethodInfo LastOrDefaultWithoutPredicate { get; }

    public static MethodInfo LastOrDefaultWithPredicate { get; }

    public static MethodInfo LongCountWithoutPredicate { get; }

    public static MethodInfo LongCountWithPredicate { get; }

    public static MethodInfo MaxWithoutSelector { get; }

    public static MethodInfo MaxWithSelector { get; }

    public static MethodInfo MinWithoutSelector { get; }

    public static MethodInfo MinWithSelector { get; }

    public static MethodInfo OfType { get; }

    public static MethodInfo OrderBy { get; }

    public static MethodInfo OrderByDescending { get; }

    public static MethodInfo Reverse { get; }

    public static MethodInfo Select { get; }

    public static MethodInfo SelectManyWithoutCollectionSelector { get; }

    public static MethodInfo SelectManyWithCollectionSelector { get; }

    public static MethodInfo SingleWithoutPredicate { get; }

    public static MethodInfo SingleWithPredicate { get; }

    public static MethodInfo SingleOrDefaultWithoutPredicate { get; }

    public static MethodInfo SingleOrDefaultWithPredicate { get; }

    public static MethodInfo Skip { get; }

    public static MethodInfo SkipWhile { get; }

    public static MethodInfo Take { get; }

    public static MethodInfo TakeWhile { get; }

    public static MethodInfo ThenBy { get; }

    public static MethodInfo ThenByDescending { get; }

    public static MethodInfo Union { get; }

    public static MethodInfo Where { get; }

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

    private static Dictionary<Type, MethodInfo> AverageWithoutSelectorMethods { get; }
    private static Dictionary<Type, MethodInfo> AverageWithSelectorMethods { get; }
    private static Dictionary<Type, MethodInfo> SumWithoutSelectorMethods { get; }
    private static Dictionary<Type, MethodInfo> SumWithSelectorMethods { get; }

    static QueryableMethods() {
        var queryableMethodGroups = typeof(Queryable)
                                   .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                                   .GroupBy(mi => mi.Name)
                                   .ToDictionary(e => e.Key, l => l.ToList());

        All = GetMethod(
                        nameof(Queryable.All), 1,
                        types =>
                        [
                            typeof(IQueryable<>).MakeGenericType(types[0]),
                            typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                        ]);

        AnyWithoutPredicate = GetMethod(
                                        nameof(Queryable.Any), 1,
                                        types => [typeof(IQueryable<>).MakeGenericType(types[0])]);

        AnyWithPredicate = GetMethod(
                                     nameof(Queryable.Any), 1,
                                     types =>
                                     [
                                         typeof(IQueryable<>).MakeGenericType(types[0]),
                                         typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                                     ]);

        AsQueryable = GetMethod(
                                nameof(Queryable.AsQueryable), 1,
                                types => [typeof(IEnumerable<>).MakeGenericType(types[0])]);

        Cast = GetMethod(nameof(Queryable.Cast), 1, types => [typeof(IQueryable)]);

        Concat = GetMethod(
                           nameof(Queryable.Concat), 1,
                           types => [typeof(IQueryable<>).MakeGenericType(types[0]), typeof(IEnumerable<>).MakeGenericType(types[0])]);

        Contains = GetMethod(
                             nameof(Queryable.Contains), 1,
                             types => [typeof(IQueryable<>).MakeGenericType(types[0]), types[0]]);

        CountWithoutPredicate = GetMethod(
                                          nameof(Queryable.Count), 1,
                                          types => [typeof(IQueryable<>).MakeGenericType(types[0])]);

        CountWithPredicate = GetMethod(
                                       nameof(Queryable.Count), 1,
                                       types =>
                                       [
                                           typeof(IQueryable<>).MakeGenericType(types[0]),
                                           typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                                       ]);

        DefaultIfEmptyWithoutArgument = GetMethod(
                                                  nameof(Queryable.DefaultIfEmpty), 1,
                                                  types => [typeof(IQueryable<>).MakeGenericType(types[0])]);

        DefaultIfEmptyWithArgument = GetMethod(
                                               nameof(Queryable.DefaultIfEmpty), 1,
                                               types => [typeof(IQueryable<>).MakeGenericType(types[0]), types[0]]);

        Distinct = GetMethod(nameof(Queryable.Distinct), 1, types => [typeof(IQueryable<>).MakeGenericType(types[0])]);

        ElementAt = GetMethod(
                              nameof(Queryable.ElementAt), 1,
                              types => [typeof(IQueryable<>).MakeGenericType(types[0]), typeof(int)]);

        ElementAtOrDefault = GetMethod(
                                       nameof(Queryable.ElementAtOrDefault), 1,
                                       types => [typeof(IQueryable<>).MakeGenericType(types[0]), typeof(int)]);

        Except = GetMethod(
                           nameof(Queryable.Except), 1,
                           types => [typeof(IQueryable<>).MakeGenericType(types[0]), typeof(IEnumerable<>).MakeGenericType(types[0])]);

        FirstWithoutPredicate = GetMethod(
                                          nameof(Queryable.First), 1, types => [typeof(IQueryable<>).MakeGenericType(types[0])]);

        FirstWithPredicate = GetMethod(
                                       nameof(Queryable.First), 1,
                                       types =>
                                       [
                                           typeof(IQueryable<>).MakeGenericType(types[0]),
                                           typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                                       ]);

        FirstOrDefaultWithoutPredicate = GetMethod(
                                                   nameof(Queryable.FirstOrDefault), 1,
                                                   types => [typeof(IQueryable<>).MakeGenericType(types[0])]);

        FirstOrDefaultWithPredicate = GetMethod(
                                                nameof(Queryable.FirstOrDefault), 1,
                                                types =>
                                                [
                                                    typeof(IQueryable<>).MakeGenericType(types[0]),
                                                    typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                                                ]);

        GroupByWithKeySelector = GetMethod(
                                           nameof(Queryable.GroupBy), 2,
                                           types =>
                                           [
                                               typeof(IQueryable<>).MakeGenericType(types[0]),
                                               typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1]))
                                           ]);

        GroupByWithKeyElementSelector = GetMethod(
                                                  nameof(Queryable.GroupBy), 3,
                                                  types =>
                                                  [
                                                      typeof(IQueryable<>).MakeGenericType(types[0]),
                                                      typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1])),
                                                      typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[2]))
                                                  ]);

        GroupByWithKeyElementResultSelector = GetMethod(
                                                        nameof(Queryable.GroupBy), 4,
                                                        types =>
                                                        [
                                                            typeof(IQueryable<>).MakeGenericType(types[0]),
                                                            typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1])),
                                                            typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[2])),
                                                            typeof(Expression<>).MakeGenericType(
                                                                                                 typeof(Func<,,>).MakeGenericType(
                                                                                                                                  types[1], typeof(IEnumerable<>).MakeGenericType(types[2]), types[3]))
                                                        ]);

        GroupByWithKeyResultSelector = GetMethod(
                                                 nameof(Queryable.GroupBy), 3,
                                                 types =>
                                                 [
                                                     typeof(IQueryable<>).MakeGenericType(types[0]),
                                                     typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1])),
                                                     typeof(Expression<>).MakeGenericType(
                                                                                          typeof(Func<,,>).MakeGenericType(
                                                                                                                           types[1], typeof(IEnumerable<>).MakeGenericType(types[0]), types[2]))
                                                 ]);

        GroupJoin = GetMethod(
                              nameof(Queryable.GroupJoin), 4,
                              types =>
                              [
                                  typeof(IQueryable<>).MakeGenericType(types[0]),
                                  typeof(IEnumerable<>).MakeGenericType(types[1]),
                                  typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[2])),
                                  typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[1], types[2])),
                                  typeof(Expression<>).MakeGenericType(
                                                                       typeof(Func<,,>).MakeGenericType(
                                                                                                        types[0], typeof(IEnumerable<>).MakeGenericType(types[1]), types[3]))
                              ]);

        Intersect = GetMethod(
                              nameof(Queryable.Intersect), 1,
                              types => [typeof(IQueryable<>).MakeGenericType(types[0]), typeof(IEnumerable<>).MakeGenericType(types[0])]);

        Join = GetMethod(
                         nameof(Queryable.Join), 4,
                         types =>
                         [
                             typeof(IQueryable<>).MakeGenericType(types[0]),
                             typeof(IEnumerable<>).MakeGenericType(types[1]),
                             typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[2])),
                             typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[1], types[2])),
                             typeof(Expression<>).MakeGenericType(typeof(Func<,,>).MakeGenericType(types[0], types[1], types[3]))
                         ]);

        LastWithoutPredicate = GetMethod(nameof(Queryable.Last), 1, types => [typeof(IQueryable<>).MakeGenericType(types[0])]);

        LastWithPredicate = GetMethod(
                                      nameof(Queryable.Last), 1,
                                      types =>
                                      [
                                          typeof(IQueryable<>).MakeGenericType(types[0]),
                                          typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                                      ]);

        LastOrDefaultWithoutPredicate = GetMethod(
                                                  nameof(Queryable.LastOrDefault), 1,
                                                  types => [typeof(IQueryable<>).MakeGenericType(types[0])]);

        LastOrDefaultWithPredicate = GetMethod(
                                               nameof(Queryable.LastOrDefault), 1,
                                               types =>
                                               [
                                                   typeof(IQueryable<>).MakeGenericType(types[0]),
                                                   typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                                               ]);

        LongCountWithoutPredicate = GetMethod(
                                              nameof(Queryable.LongCount), 1,
                                              types => [typeof(IQueryable<>).MakeGenericType(types[0])]);

        LongCountWithPredicate = GetMethod(
                                           nameof(Queryable.LongCount), 1,
                                           types =>
                                           [
                                               typeof(IQueryable<>).MakeGenericType(types[0]),
                                               typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                                           ]);

        MaxWithoutSelector = GetMethod(nameof(Queryable.Max), 1, types => [typeof(IQueryable<>).MakeGenericType(types[0])]);

        MaxWithSelector = GetMethod(
                                    nameof(Queryable.Max), 2,
                                    types =>
                                    [
                                        typeof(IQueryable<>).MakeGenericType(types[0]),
                                        typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1]))
                                    ]);

        MinWithoutSelector = GetMethod(nameof(Queryable.Min), 1, types => [typeof(IQueryable<>).MakeGenericType(types[0])]);

        MinWithSelector = GetMethod(
                                    nameof(Queryable.Min), 2,
                                    types =>
                                    [
                                        typeof(IQueryable<>).MakeGenericType(types[0]),
                                        typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1]))
                                    ]);

        OfType = GetMethod(nameof(Queryable.OfType), 1, types => [typeof(IQueryable)]);

        OrderBy = GetMethod(
                            nameof(Queryable.OrderBy), 2,
                            types =>
                            [
                                typeof(IQueryable<>).MakeGenericType(types[0]),
                                typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1]))
                            ]);

        OrderByDescending = GetMethod(
                                      nameof(Queryable.OrderByDescending), 2,
                                      types =>
                                      [
                                          typeof(IQueryable<>).MakeGenericType(types[0]),
                                          typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1]))
                                      ]);

        Reverse = GetMethod(nameof(Queryable.Reverse), 1, types => [typeof(IQueryable<>).MakeGenericType(types[0])]);

        Select = GetMethod(
                           nameof(Queryable.Select), 2,
                           types =>
                           [
                               typeof(IQueryable<>).MakeGenericType(types[0]),
                               typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1]))
                           ]);

        SelectManyWithoutCollectionSelector = GetMethod(
                                                        nameof(Queryable.SelectMany), 2,
                                                        types =>
                                                        [
                                                            typeof(IQueryable<>).MakeGenericType(types[0]),
                                                            typeof(Expression<>).MakeGenericType(
                                                                                                 typeof(Func<,>).MakeGenericType(
                                                                                                                                 types[0], typeof(IEnumerable<>).MakeGenericType(types[1])))
                                                        ]);

        SelectManyWithCollectionSelector = GetMethod(
                                                     nameof(Queryable.SelectMany), 3,
                                                     types =>
                                                     [
                                                         typeof(IQueryable<>).MakeGenericType(types[0]),
                                                         typeof(Expression<>).MakeGenericType(
                                                                                              typeof(Func<,>).MakeGenericType(
                                                                                                                              types[0], typeof(IEnumerable<>).MakeGenericType(types[1]))),
                                                         typeof(Expression<>).MakeGenericType(typeof(Func<,,>).MakeGenericType(types[0], types[1], types[2]))
                                                     ]);

        SingleWithoutPredicate = GetMethod(
                                           nameof(Queryable.Single), 1, types => [typeof(IQueryable<>).MakeGenericType(types[0])]);

        SingleWithPredicate = GetMethod(
                                        nameof(Queryable.Single), 1,
                                        types =>
                                        [
                                            typeof(IQueryable<>).MakeGenericType(types[0]),
                                            typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                                        ]);

        SingleOrDefaultWithoutPredicate = GetMethod(
                                                    nameof(Queryable.SingleOrDefault), 1,
                                                    types => [typeof(IQueryable<>).MakeGenericType(types[0])]);

        SingleOrDefaultWithPredicate = GetMethod(
                                                 nameof(Queryable.SingleOrDefault), 1,
                                                 types =>
                                                 [
                                                     typeof(IQueryable<>).MakeGenericType(types[0]),
                                                     typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                                                 ]);

        Skip = GetMethod(
                         nameof(Queryable.Skip), 1,
                         types => [typeof(IQueryable<>).MakeGenericType(types[0]), typeof(int)]);

        SkipWhile = GetMethod(
                              nameof(Queryable.SkipWhile), 1,
                              types =>
                              [
                                  typeof(IQueryable<>).MakeGenericType(types[0]),
                                  typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                              ]);

        Take = GetMethod(
                         nameof(Queryable.Take), 1,
                         types => [typeof(IQueryable<>).MakeGenericType(types[0]), typeof(int)]);

        TakeWhile = GetMethod(
                              nameof(Queryable.TakeWhile), 1,
                              types =>
                              [
                                  typeof(IQueryable<>).MakeGenericType(types[0]),
                                  typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                              ]);

        ThenBy = GetMethod(
                           nameof(Queryable.ThenBy), 2,
                           types =>
                           [
                               typeof(IOrderedQueryable<>).MakeGenericType(types[0]),
                               typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1]))
                           ]);

        ThenByDescending = GetMethod(
                                     nameof(Queryable.ThenByDescending), 2,
                                     types =>
                                     [
                                         typeof(IOrderedQueryable<>).MakeGenericType(types[0]),
                                         typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], types[1]))
                                     ]);

        Union = GetMethod(
                          nameof(Queryable.Union), 1,
                          types => [typeof(IQueryable<>).MakeGenericType(types[0]), typeof(IEnumerable<>).MakeGenericType(types[0])]);

        Where = GetMethod(
                          nameof(Queryable.Where), 1,
                          types =>
                          [
                              typeof(IQueryable<>).MakeGenericType(types[0]),
                              typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], typeof(bool)))
                          ]);

        var numericTypes = new[]
                           {
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

        AverageWithoutSelectorMethods = new Dictionary<Type, MethodInfo>();
        AverageWithSelectorMethods = new Dictionary<Type, MethodInfo>();
        SumWithoutSelectorMethods = new Dictionary<Type, MethodInfo>();
        SumWithSelectorMethods = new Dictionary<Type, MethodInfo>();

        foreach (var type in numericTypes) {
            AverageWithoutSelectorMethods[type] = GetMethod(
                                                            nameof(Queryable.Average), 0, types => [typeof(IQueryable<>).MakeGenericType(type)]);
            AverageWithSelectorMethods[type] = GetMethod(
                                                         nameof(Queryable.Average), 1,
                                                         types =>
                                                         [
                                                             typeof(IQueryable<>).MakeGenericType(types[0]),
                                                             typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], type))
                                                         ]);
            SumWithoutSelectorMethods[type] = GetMethod(
                                                        nameof(Queryable.Sum), 0, types => [typeof(IQueryable<>).MakeGenericType(type)]);
            SumWithSelectorMethods[type] = GetMethod(
                                                     nameof(Queryable.Sum), 1,
                                                     types =>
                                                     [
                                                         typeof(IQueryable<>).MakeGenericType(types[0]),
                                                         typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(types[0], type))
                                                     ]);
        }

        MethodInfo GetMethod(string name, int genericParameterCount, Func<Type[], Type[]> parameterGenerator)
            => queryableMethodGroups[name].Single(
                                                  mi => ((genericParameterCount == 0 && !mi.IsGenericMethod)
                                                      || (mi.IsGenericMethod && mi.GetGenericArguments().Length == genericParameterCount))
                                                     && mi.GetParameters().Select(e => e.ParameterType).SequenceEqual(
                                                                                                                      parameterGenerator(mi.IsGenericMethod ? mi.GetGenericArguments() : [])));
    }
}
