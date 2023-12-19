namespace DotNetToolbox;

public class ObjectExtensionsTests {
    [Theory]
    [ClassData(typeof(TestDataForPrimitives))]
    public void Dump_SimpleTypes_ReturnsString(object? value, string expectedText) {
        // Arrange & Act
        var result = value.Dump();

        //Assert
        result.Should().Be(expectedText);
    }

    [Fact]
    public void Dump_WithCustomFormatter_ReturnsString() {
        // Arrange & Act
        var result = new TestClass(42, "Text").Dump(opt => {
            opt.CustomFormatters[typeof(int)] = v => $"{v:0,000.000}";
            opt.CustomFormatters[typeof(string)] = _ => "It is a string.";
        });

        //Assert
        result.Should().Be(_customFormatterDump);
    }

    [Fact]
    public void Dump_ForSimpleTypes_WithFullName_ReturnsString() {
        // Arrange & Act
        var result = "Value".Dump(opt => opt.UseFullNames = true);

        // Assert
        result.Should().Be("<System.String> \"Value\"");
    }

    [Theory]
    [ClassData(typeof(TestDataForCollections))]
    public void Dump_ForCollections_ReturnsString(object? value, string expectedText) {
        // Arrange & Act
        var result = value.Dump();

        // Assert
        result.Should().Be(expectedText);
    }

    [Theory]
    [ClassData(typeof(TestDataForComplexTypes))]
    public void Dump_ComplexType_ReturnsString(object? value, Layout layout, bool indented, string expectedText) {
        // Arrange & Act
        var result = value.Dump(opt => {
            opt.Layout = layout;
            opt.Indented = indented;
        });

        // Assert
        result.Should().Be(expectedText);
    }

    [Fact]
    public void Dump_VeryComplexType_LimitMaxLevel_ReturnsString() {
        // Arrange & Act
        var result = CultureInfo.GetCultureInfo("en-CA").Dump(opt => opt.MaxLevel = 2);

        // Assert
        result.Should().Be(_cultureInfoDump);
    }

    [Fact]
    public void Dump_ComplexTypes_WithFullName_ReturnsString() {
        // Arrange & Act
        var result = new TestClass(42, "Text").Dump(opt => opt.UseFullNames = true);

        // Assert
        result.Should().Be(_fullNamedDump);
    }

    [Fact]
    public void Dump_ExtremelyComplexType_ReturnsString() {
        // Arrange & Act
        var result = typeof(int).Dump();

        // Assert
        result.Should().Be(_integerTypeDump);
    }

    [Fact]
    public void Dump_CustomType_ReturnsString() {
        // Arrange & Act
        var result = typeof(CustomClassWithGenerics<>).Dump();

        // Assert
        result.Should().Be(_customClassWithGenericsDump);
    }

#region Test Data

    private static readonly int[] _array = [1, 2, 3];
    private static readonly List<int> _list = [1, 2, 3];
    private static readonly List<List<int>> _listOfLists = [_list, _list, _list];

    [SuppressMessage("CodeQuality", "IDE[0079]:Remove unnecessary suppression", Justification = "<Pending>")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private class TestClass(int intValue, string stringValue) {
        public int IntProperty { get; init; } = intValue;
        public string StringProperty { get; set; } = stringValue;
    }

    [SuppressMessage("CodeQuality", "IDE[0079]:Remove unnecessary suppression", Justification = "<Pending>")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private class CustomClassWithGenerics<T>(T? value) {
        public const string Name = "CustomClassWithGenerics";
        public static readonly T Default = default!;
        public T Value { get; } = value ?? Default;
        public static T ConvertTo<T>(object? obj)
            => (T)Convert.ChangeType(obj, typeof(T))!;
        public delegate void OnConvertedHandler(object sender, ConvertedArgs e);
        public event OnConvertedHandler OnConverted;
        public class ConvertedArgs : EventArgs {
            public T Value { get; set; }
        }
    }

    [SuppressMessage("CodeQuality", "IDE[0079]:Remove unnecessary suppression", Justification = "<Pending>")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private class TestClassWithGeneric<T>(T value) {
        public T Property { get; set; } = value;
        public Func<T>? ConvertTo { get; set; }
    }

    [SuppressMessage("CodeQuality", "IDE[0079]:Remove unnecessary suppression", Justification = "<Pending>")]
    [SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Local")]
    private record TestRecord(int IntProperty, string StringProperty);

    [SuppressMessage("CodeQuality", "IDE[0079]:Remove unnecessary suppression", Justification = "<Pending>")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private struct TestStruct(int intValue, string stringValue) {
        public int IntProperty { get; set; } = intValue;
        public string StringProperty { get; } = stringValue;
    }

    private class TestDataForPrimitives : TheoryData<object?, string> {
        public TestDataForPrimitives() {
            Add(null, "null");
            Add(true, "<Boolean> true");
            Add((byte)42, "<Byte> 42");
            Add((sbyte)42, "<SByte> 42");
            Add((char)42, "<Char> '*'");
            Add((short)42, "<Int16> 42");
            Add((ushort)42, "<UInt16> 42");
            Add(42, "<Int32> 42");
            Add((uint)42, "<UInt32> 42");
            Add((long)42, "<Int64> 42");
            Add((ulong)42, "<UInt64> 42");
            Add((float)42.7, "<Single> 42.7");
            Add(42.7, "<Double> 42.7");
            Add(42.7m, "<Decimal> 42.7");
            Add("Text", "<String> \"Text\"");
            Add(new DateTime(2001, 10, 12), "<DateTime> 2001-10-12 00:00:00");
            Add(new DateTimeOffset(new DateTime(2001, 10, 12), TimeSpan.FromHours(-5)), "<DateTimeOffset> 2001-10-12 00:00:00 -05:00");
            Add(new DateOnly(2001, 10, 12), "<DateOnly> 2001-10-12");
            Add(new TimeOnly(23, 15, 52), "<TimeOnly> 23:15");
            Add(new TimeSpan(23, 15, 52), "<TimeSpan> 23:15:52");
            Add(Guid.Parse("b6d3aec4-daca-4dca-ada7-cda51623ed50"), "<Guid> b6d3aec4-daca-4dca-ada7-cda51623ed50");
        }
    }

    private class TestDataForCollections : TheoryData<object?, string> {
        public TestDataForCollections() {
            Add(_array, _arrayOfInt32Dump);
            Add(_list, _listOfInt32Dump);
            Add(new Dictionary<string, double> { ["A"] = 1.1, ["B"] = 2.2, ["C"] = 3.3 }, _dictionaryOfStringDoubleDump);
        }
    }
    private class TestDataForComplexTypes : TheoryData<object?, Layout, bool, string> {
        public TestDataForComplexTypes() {
            Add(new TestClass(42, "Text"), Layout.Typed, true, _testClassDump);
            Add(new TestClass(42, "Text"), Layout.Typed, false, _testClassCompactDump);
            Add(new TestClass(42, "Text"), Layout.Json, true, _testClassIndentedJsonDump);
            Add(new TestClass(42, "Text"), Layout.Json, false, _testClassCompactJsonDump);
            Add(new TestClassWithGeneric<int>(42), Layout.Typed, true, _testGenericClassWithInt32Dump);
            Add(new TestClassWithGeneric<double>(42), Layout.Typed, false, _testGenericClassDoubleCompactDump);
            Add(new TestRecord(42, "Text"), Layout.Typed, true, _testRecordDump);
            Add(new TestRecord(42, "Text"), Layout.Typed, false, _testRecordCompactDump);
            Add(new TestStruct(42, "Text"), Layout.Typed, true, _testStructDump);
            Add(new TestStruct(42, "Text"), Layout.Typed, false, _testStructCompactDump);
            Add(new Dictionary<string, TestStruct> {
                ["A"] = new(42, "Text"),
                ["B"] = new(7, "Other"),
            }, Layout.Typed, true, _testDictionaryDump);
            Add(new Dictionary<string, TestStruct> {
                ["A"] = new(42, "Text"),
                ["B"] = new(7, "Other"),
            }, Layout.Typed, false, _testDictionaryCompactDump);
            Add(_listOfLists, Layout.Typed, true, _listOfListsDump);
            Add(_listOfLists, Layout.Typed, false, _listOfListsCompactDump);
        }
    }


    private const string _arrayOfInt32Dump =
        """
        <Int32[]> [
            [0]: 1,
            [1]: 2,
            [2]: 3
        ]
        """;

    private const string _listOfInt32Dump =
        """
        <List<Int32>> [
            [0]: 1,
            [1]: 2,
            [2]: 3
        ]
        """;

    private const string _dictionaryOfStringDoubleDump =
        """
        <Dictionary<String, Double>> [
            ["A"]: 1.1,
            ["B"]: 2.2,
            ["C"]: 3.3
        ]
        """;

    private const string _listOfListsDump =
        """
        <List<List<Int32>>> [
            [0]: [
                [0]: 1,
                [1]: 2,
                [2]: 3
            ],
            [1]: [
                [0]: 1,
                [1]: 2,
                [2]: 3
            ],
            [2]: [
                [0]: 1,
                [1]: 2,
                [2]: 3
            ]
        ]
        """;

    private const string _listOfListsCompactDump =
        "<List<List<Int32>>>[[0]:[[0]:1,[1]:2,[2]:3],[1]:[[0]:1,[1]:2,[2]:3],[2]:[[0]:1,[1]:2,[2]:3]]";

    private const string _testClassDump =
        """
        <TestClass> {
            "IntProperty": <Int32> 42,
            "StringProperty": <String> "Text"
        }
        """;

    private const string _testClassCompactDump =
        """<TestClass>{"IntProperty":<Int32>42,"StringProperty":<String>"Text"}""";

    private const string _testClassIndentedJsonDump =
        """
        {
            "IntProperty": 42,
            "StringProperty": "Text"
        }
        """;

    private const string _testClassCompactJsonDump =
        """{"IntProperty":42,"StringProperty":"Text"}""";

    private const string _testGenericClassWithInt32Dump =
        """
        <ObjectExtensionsTests+TestClassWithGeneric<Int32>> {
            "Property": <Int32> 42,
            "ConvertTo": <Func<Int32>> null
        }
        """;

    private const string _testGenericClassDoubleCompactDump =
        """<ObjectExtensionsTests+TestClassWithGeneric<Double>>{"Property":<Double>42,"ConvertTo":<Func<Double>>null}""";

    private const string _testRecordDump =
        """
        <TestRecord> {
            "IntProperty": <Int32> 42,
            "StringProperty": <String> "Text"
        }
        """;

    private const string _testRecordCompactDump =
        """<TestRecord>{"IntProperty":<Int32>42,"StringProperty":<String>"Text"}""";

    private const string _testStructDump =
        """
        <TestStruct> {
            "IntProperty": <Int32> 42,
            "StringProperty": <String> "Text"
        }
        """;

    private const string _testStructCompactDump =
        """<TestStruct>{"IntProperty":<Int32>42,"StringProperty":<String>"Text"}""";

    private const string _testDictionaryDump =
        """
        <Dictionary<String, TestStruct>> [
            ["A"]: {
                "IntProperty": <Int32> 42,
                "StringProperty": <String> "Text"
            },
            ["B"]: {
                "IntProperty": <Int32> 7,
                "StringProperty": <String> "Other"
            }
        ]
        """;

    private const string _testDictionaryCompactDump =
        """<Dictionary<String, TestStruct>>[["A"]:{"IntProperty":<Int32>42,"StringProperty":<String>"Text"},["B"]:{"IntProperty":<Int32>7,"StringProperty":<String>"Other"}]""";

    private const string _cultureInfoDump =
        """
        <CultureInfo> {
            "LCID": <Int32> 4105,
            "KeyboardLayoutId": <Int32> 4105,
            "Name": <String> "en-CA",
            "IetfLanguageTag": <String> "en-CA",
            "DisplayName": <String> "English (Canada)",
            "NativeName": <String> "English (Canada)",
            "EnglishName": <String> "English (Canada)",
            "TwoLetterISOLanguageName": <String> "en",
            "ThreeLetterISOLanguageName": <String> "eng",
            "ThreeLetterWindowsLanguageName": <String> "ENC",
            "IsNeutralCulture": <Boolean> false,
            "CultureTypes": <CultureTypes> SpecificCultures, InstalledWin32Cultures,
            "UseUserOverride": <Boolean> false,
            "IsReadOnly": <Boolean> true
        }
        """;

    private const string _fullNamedDump =
        """
        <DotNetToolbox.ObjectExtensionsTests+TestClass> {
            "IntProperty": <System.Int32> 42,
            "StringProperty": <System.String> "Text"
        }
        """;

    private const string _customFormatterDump =
        """
        <TestClass> {
            "IntProperty": <Int32> 0,042.000,
            "StringProperty": <String> It is a string.
        }
        """;

    private const string _customClassWithGenericsDump =
        """
        <RuntimeType> {
            "IsCollectible": <Boolean> false,
            "FullName": <String> "DotNetToolbox.ObjectExtensionsTests+CustomClassWithGenerics`1",
            "AssemblyQualifiedName": <String> "DotNetToolbox.ObjectExtensionsTests+CustomClassWithGenerics`1, DotnetToolbox.ObjectDumper.UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
            "Namespace": <String> "DotNetToolbox",
            "GUID": <Guid> 93476c5f-16f1-34c0-a15f-9c0601fff3c4,
            "IsEnum": <Boolean> false,
            "IsConstructedGenericType": <Boolean> false,
            "IsGenericType": <Boolean> true,
            "IsGenericTypeDefinition": <Boolean> true,
            "IsSZArray": <Boolean> false,
            "ContainsGenericParameters": <Boolean> true,
            "StructLayoutAttribute": <Attribute> StructLayoutAttribute,
            "IsFunctionPointer": <Boolean> false,
            "IsUnmanagedFunctionPointer": <Boolean> false,
            "Name": <String> "CustomClassWithGenerics`1",
            "DeclaringType": <Type> ObjectExtensionsTests,
            "Assembly": <Assembly> DotnetToolbox.ObjectDumper.UnitTests,
            "BaseType": <Type> Object,
            "IsByRefLike": <Boolean> false,
            "IsGenericParameter": <Boolean> false,
            "IsTypeDefinition": <Boolean> true,
            "IsSecurityCritical": <Boolean> true,
            "IsSecuritySafeCritical": <Boolean> false,
            "IsSecurityTransparent": <Boolean> false,
            "MemberType": <MemberTypes> NestedType,
            "MetadataToken": <Int32> 33554438,
            "Module": <Module> DotnetToolbox.ObjectDumper.UnitTests.dll,
            "ReflectedType": <Type> ObjectExtensionsTests,
            "GenericTypeParameters": <Type[]> [
                [0]: T
            ],
            "DeclaredConstructors": <IEnumerable<ConstructorInfo>> [
                [0]: <Constructor> .ctor(T value)
            ],
            "DeclaredEvents": <IEnumerable<EventInfo>> [
                [0]: <Event> OnConvertedHandler OnConverted
            ],
            "DeclaredFields": <IEnumerable<FieldInfo>> [
                [0]: <Field> T Default,
                [1]: <Field> String Name
            ],
            "DeclaredMembers": <IEnumerable<MemberInfo>> [
                [0]: <Method> T get_Value(),
                [1]: <Method> T ConvertTo(Object obj),
                [2]: <Method> Void add_OnConverted(OnConvertedHandler value),
                [3]: <Method> Void remove_OnConverted(OnConvertedHandler value),
                [4]: <Constructor> .ctor(T value),
                [5]: <Property> T Value,
                [6]: <Event> OnConvertedHandler OnConverted,
                [7]: <Field> T Default,
                [8]: <Field> String Name
            ],
            "DeclaredMethods": <IEnumerable<MethodInfo>> [
                [0]: <Method> T get_Value(),
                [1]: <Method> T ConvertTo(Object obj),
                [2]: <Method> Void add_OnConverted(OnConvertedHandler value),
                [3]: <Method> Void remove_OnConverted(OnConvertedHandler value)
            ],
            "DeclaredNestedTypes": <IEnumerable<TypeInfo>> [
            ],
            "DeclaredProperties": <IEnumerable<PropertyInfo>> [
                [0]: <Property> T Value
            ],
            "ImplementedInterfaces": <IEnumerable<Type>> [
            ],
            "IsInterface": <Boolean> false,
            "IsNested": <Boolean> true,
            "IsArray": <Boolean> false,
            "IsByRef": <Boolean> false,
            "IsPointer": <Boolean> false,
            "IsGenericTypeParameter": <Boolean> false,
            "IsGenericMethodParameter": <Boolean> false,
            "IsVariableBoundArray": <Boolean> false,
            "HasElementType": <Boolean> false,
            "GenericTypeArguments": <Type[]> [
            ],
            "Attributes": <TypeAttributes> NestedPrivate, BeforeFieldInit,
            "IsAbstract": <Boolean> false,
            "IsImport": <Boolean> false,
            "IsSealed": <Boolean> false,
            "IsSpecialName": <Boolean> false,
            "IsClass": <Boolean> true,
            "IsNestedAssembly": <Boolean> false,
            "IsNestedFamANDAssem": <Boolean> false,
            "IsNestedFamily": <Boolean> false,
            "IsNestedFamORAssem": <Boolean> false,
            "IsNestedPrivate": <Boolean> true,
            "IsNestedPublic": <Boolean> false,
            "IsNotPublic": <Boolean> false,
            "IsPublic": <Boolean> false,
            "IsAutoLayout": <Boolean> true,
            "IsExplicitLayout": <Boolean> false,
            "IsLayoutSequential": <Boolean> false,
            "IsAnsiClass": <Boolean> true,
            "IsAutoClass": <Boolean> false,
            "IsUnicodeClass": <Boolean> false,
            "IsCOMObject": <Boolean> false,
            "IsContextful": <Boolean> false,
            "IsMarshalByRef": <Boolean> false,
            "IsPrimitive": <Boolean> false,
            "IsValueType": <Boolean> false,
            "IsSignatureType": <Boolean> false,
            "IsSerializable": <Boolean> false,
            "IsVisible": <Boolean> false,
            "CustomAttributes": <IEnumerable<CustomAttributeData>> [
                [0]: NullableAttribute
            ]
        }
        """;

    private static readonly string _integerTypeDump =
        """
        <RuntimeType> {
            "IsCollectible": <Boolean> false,
            "FullName": <String> "System.Int32",
            "AssemblyQualifiedName": <String> "System.Int32, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e",
            "Namespace": <String> "System",
            "GUID": <Guid> bf6391d7-4c57-3a00-9c4b-e40608e6a569,
            "IsEnum": <Boolean> false,
            "IsConstructedGenericType": <Boolean> false,
            "IsGenericType": <Boolean> false,
            "IsGenericTypeDefinition": <Boolean> false,
            "IsSZArray": <Boolean> false,
            "ContainsGenericParameters": <Boolean> false,
            "StructLayoutAttribute": <Attribute> StructLayoutAttribute,
            "IsFunctionPointer": <Boolean> false,
            "IsUnmanagedFunctionPointer": <Boolean> false,
            "Name": <String> "Int32",
            "DeclaringType": <Type> null,
            "Assembly": <Assembly> System.Private.CoreLib,
            "BaseType": <Type> ValueType,
            "IsByRefLike": <Boolean> false,
            "IsGenericParameter": <Boolean> false,
            "IsTypeDefinition": <Boolean> true,
            "IsSecurityCritical": <Boolean> true,
            "IsSecuritySafeCritical": <Boolean> false,
            "IsSecurityTransparent": <Boolean> false,
            "MemberType": <MemberTypes> TypeInfo,
            "MetadataToken": <Int32> 33554772,
            "Module": <Module> System.Private.CoreLib.dll,
            "ReflectedType": <Type> null,
            "GenericTypeParameters": <Type[]> [
            ],
            "DeclaredConstructors": <IEnumerable<ConstructorInfo>> [
            ],
            "DeclaredEvents": <IEnumerable<EventInfo>> [
            ],
            "DeclaredFields": <IEnumerable<FieldInfo>> [
                [0]: <Field> Int32 MaxValue,
                [1]: <Field> Int32 MinValue
            ],
            "DeclaredMembers": <IEnumerable<MemberInfo>> [
                [0]: <Method> Int32 CompareTo(Object value),
                [1]: <Method> Int32 CompareTo(Int32 value),
                [2]: <Method> Boolean Equals(Object obj),
                [3]: <Method> Boolean Equals(Int32 obj),
                [4]: <Method> Int32 GetHashCode(),
                [5]: <Method> String ToString(),
                [6]: <Method> String ToString(String format),
                [7]: <Method> String ToString(IFormatProvider provider),
                [8]: <Method> String ToString(String format, IFormatProvider provider),
                [9]: <Method> Boolean TryFormat(Span<Char> destination, Int32& charsWritten, ReadOnlySpan<Char> format, IFormatProvider provider),
                [10]: <Method> Boolean TryFormat(Span<Byte> utf8Destination, Int32& bytesWritten, ReadOnlySpan<Char> format, IFormatProvider provider),
                [11]: <Method> Int32 Parse(String s),
                [12]: <Method> Int32 Parse(String s, NumberStyles style),
                [13]: <Method> Int32 Parse(String s, IFormatProvider provider),
                [14]: <Method> Int32 Parse(String s, NumberStyles style, IFormatProvider provider),
                [15]: <Method> Int32 Parse(ReadOnlySpan<Char> s, NumberStyles style, IFormatProvider provider),
                [16]: <Method> Boolean TryParse(String s, Int32& result),
                [17]: <Method> Boolean TryParse(ReadOnlySpan<Char> s, Int32& result),
                [18]: <Method> Boolean TryParse(ReadOnlySpan<Byte> utf8Text, Int32& result),
                [19]: <Method> Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, Int32& result),
                [20]: <Method> Boolean TryParse(ReadOnlySpan<Char> s, NumberStyles style, IFormatProvider provider, Int32& result),
                [21]: <Method> TypeCode GetTypeCode(),
                [22]: <Method> ValueTuple<Int32, Int32> DivRem(Int32 left, Int32 right),
                [23]: <Method> Int32 LeadingZeroCount(Int32 value),
                [24]: <Method> Int32 PopCount(Int32 value),
                [25]: <Method> Int32 RotateLeft(Int32 value, Int32 rotateAmount),
                [26]: <Method> Int32 RotateRight(Int32 value, Int32 rotateAmount),
                [27]: <Method> Int32 TrailingZeroCount(Int32 value),
                [28]: <Method> Boolean IsPow2(Int32 value),
                [29]: <Method> Int32 Log2(Int32 value),
                [30]: <Method> Int32 Clamp(Int32 value, Int32 min, Int32 max),
                [31]: <Method> Int32 CopySign(Int32 value, Int32 sign),
                [32]: <Method> Int32 Max(Int32 x, Int32 y),
                [33]: <Method> Int32 Min(Int32 x, Int32 y),
                [34]: <Method> Int32 Sign(Int32 value),
                [35]: <Method> Int32 Abs(Int32 value),
                [36]: <Method> Int32 CreateChecked(TOther value),
                [37]: <Method> Int32 CreateSaturating(TOther value),
                [38]: <Method> Int32 CreateTruncating(TOther value),
                [39]: <Method> Boolean IsEvenInteger(Int32 value),
                [40]: <Method> Boolean IsNegative(Int32 value),
                [41]: <Method> Boolean IsOddInteger(Int32 value),
                [42]: <Method> Boolean IsPositive(Int32 value),
                [43]: <Method> Int32 MaxMagnitude(Int32 x, Int32 y),
                [44]: <Method> Int32 MinMagnitude(Int32 x, Int32 y),
                [45]: <Method> Boolean TryParse(String s, IFormatProvider provider, Int32& result),
                [46]: <Method> Int32 Parse(ReadOnlySpan<Char> s, IFormatProvider provider),
                [47]: <Method> Boolean TryParse(ReadOnlySpan<Char> s, IFormatProvider provider, Int32& result),
                [48]: <Method> Int32 Parse(ReadOnlySpan<Byte> utf8Text, NumberStyles style, IFormatProvider provider),
                [49]: <Method> Boolean TryParse(ReadOnlySpan<Byte> utf8Text, NumberStyles style, IFormatProvider provider, Int32& result),
                [50]: <Method> Int32 Parse(ReadOnlySpan<Byte> utf8Text, IFormatProvider provider),
                [51]: <Method> Boolean TryParse(ReadOnlySpan<Byte> utf8Text, IFormatProvider provider, Int32& result),
                [52]: <Field> Int32 MaxValue,
                [53]: <Field> Int32 MinValue
            ],
            "DeclaredMethods": <IEnumerable<MethodInfo>> [
                [0]: <Method> Int32 CompareTo(Object value),
                [1]: <Method> Int32 CompareTo(Int32 value),
                [2]: <Method> Boolean Equals(Object obj),
                [3]: <Method> Boolean Equals(Int32 obj),
                [4]: <Method> Int32 GetHashCode(),
                [5]: <Method> String ToString(),
                [6]: <Method> String ToString(String format),
                [7]: <Method> String ToString(IFormatProvider provider),
                [8]: <Method> String ToString(String format, IFormatProvider provider),
                [9]: <Method> Boolean TryFormat(Span<Char> destination, Int32& charsWritten, ReadOnlySpan<Char> format, IFormatProvider provider),
                [10]: <Method> Boolean TryFormat(Span<Byte> utf8Destination, Int32& bytesWritten, ReadOnlySpan<Char> format, IFormatProvider provider),
                [11]: <Method> Int32 Parse(String s),
                [12]: <Method> Int32 Parse(String s, NumberStyles style),
                [13]: <Method> Int32 Parse(String s, IFormatProvider provider),
                [14]: <Method> Int32 Parse(String s, NumberStyles style, IFormatProvider provider),
                [15]: <Method> Int32 Parse(ReadOnlySpan<Char> s, NumberStyles style, IFormatProvider provider),
                [16]: <Method> Boolean TryParse(String s, Int32& result),
                [17]: <Method> Boolean TryParse(ReadOnlySpan<Char> s, Int32& result),
                [18]: <Method> Boolean TryParse(ReadOnlySpan<Byte> utf8Text, Int32& result),
                [19]: <Method> Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, Int32& result),
                [20]: <Method> Boolean TryParse(ReadOnlySpan<Char> s, NumberStyles style, IFormatProvider provider, Int32& result),
                [21]: <Method> TypeCode GetTypeCode(),
                [22]: <Method> ValueTuple<Int32, Int32> DivRem(Int32 left, Int32 right),
                [23]: <Method> Int32 LeadingZeroCount(Int32 value),
                [24]: <Method> Int32 PopCount(Int32 value),
                [25]: <Method> Int32 RotateLeft(Int32 value, Int32 rotateAmount),
                [26]: <Method> Int32 RotateRight(Int32 value, Int32 rotateAmount),
                [27]: <Method> Int32 TrailingZeroCount(Int32 value),
                [28]: <Method> Boolean IsPow2(Int32 value),
                [29]: <Method> Int32 Log2(Int32 value),
                [30]: <Method> Int32 Clamp(Int32 value, Int32 min, Int32 max),
                [31]: <Method> Int32 CopySign(Int32 value, Int32 sign),
                [32]: <Method> Int32 Max(Int32 x, Int32 y),
                [33]: <Method> Int32 Min(Int32 x, Int32 y),
                [34]: <Method> Int32 Sign(Int32 value),
                [35]: <Method> Int32 Abs(Int32 value),
                [36]: <Method> Int32 CreateChecked(TOther value),
                [37]: <Method> Int32 CreateSaturating(TOther value),
                [38]: <Method> Int32 CreateTruncating(TOther value),
                [39]: <Method> Boolean IsEvenInteger(Int32 value),
                [40]: <Method> Boolean IsNegative(Int32 value),
                [41]: <Method> Boolean IsOddInteger(Int32 value),
                [42]: <Method> Boolean IsPositive(Int32 value),
                [43]: <Method> Int32 MaxMagnitude(Int32 x, Int32 y),
                [44]: <Method> Int32 MinMagnitude(Int32 x, Int32 y),
                [45]: <Method> Boolean TryParse(String s, IFormatProvider provider, Int32& result),
                [46]: <Method> Int32 Parse(ReadOnlySpan<Char> s, IFormatProvider provider),
                [47]: <Method> Boolean TryParse(ReadOnlySpan<Char> s, IFormatProvider provider, Int32& result),
                [48]: <Method> Int32 Parse(ReadOnlySpan<Byte> utf8Text, NumberStyles style, IFormatProvider provider),
                [49]: <Method> Boolean TryParse(ReadOnlySpan<Byte> utf8Text, NumberStyles style, IFormatProvider provider, Int32& result),
                [50]: <Method> Int32 Parse(ReadOnlySpan<Byte> utf8Text, IFormatProvider provider),
                [51]: <Method> Boolean TryParse(ReadOnlySpan<Byte> utf8Text, IFormatProvider provider, Int32& result)
            ],
            "DeclaredNestedTypes": <IEnumerable<TypeInfo>> [
            ],
            "DeclaredProperties": <IEnumerable<PropertyInfo>> [
            ],
            "ImplementedInterfaces": <IEnumerable<Type>> [
                [0]: IComparable,
                [1]: IConvertible,
                [2]: ISpanFormattable,
                [3]: IFormattable,
                [4]: IComparable<Int32>,
                [5]: IEquatable<Int32>,
                [6]: IBinaryInteger<Int32>,
                [7]: IBinaryNumber<Int32>,
                [8]: IBitwiseOperators<Int32, Int32, Int32>,
                [9]: INumber<Int32>,
                [10]: IComparisonOperators<Int32, Int32, Boolean>,
                [11]: IEqualityOperators<Int32, Int32, Boolean>,
                [12]: IModulusOperators<Int32, Int32, Int32>,
                [13]: INumberBase<Int32>,
                [14]: IAdditionOperators<Int32, Int32, Int32>,
                [15]: IAdditiveIdentity<Int32, Int32>,
                [16]: IDecrementOperators<Int32>,
                [17]: IDivisionOperators<Int32, Int32, Int32>,
                [18]: IIncrementOperators<Int32>,
                [19]: IMultiplicativeIdentity<Int32, Int32>,
                [20]: IMultiplyOperators<Int32, Int32, Int32>,
                [21]: ISpanParsable<Int32>,
                [22]: IParsable<Int32>,
                [23]: ISubtractionOperators<Int32, Int32, Int32>,
                [24]: IUnaryPlusOperators<Int32, Int32>,
                [25]: IUnaryNegationOperators<Int32, Int32>,
                [26]: IUtf8SpanFormattable,
                [27]: IUtf8SpanParsable<Int32>,
                [28]: IShiftOperators<Int32, Int32, Int32>,
                [29]: IMinMaxValue<Int32>,
                [30]: ISignedNumber<Int32>
            ],
            "IsInterface": <Boolean> false,
            "IsNested": <Boolean> false,
            "IsArray": <Boolean> false,
            "IsByRef": <Boolean> false,
            "IsPointer": <Boolean> false,
            "IsGenericTypeParameter": <Boolean> false,
            "IsGenericMethodParameter": <Boolean> false,
            "IsVariableBoundArray": <Boolean> false,
            "HasElementType": <Boolean> false,
            "GenericTypeArguments": <Type[]> [
            ],
            "Attributes": <TypeAttributes> Public, SequentialLayout, Sealed, Serializable, BeforeFieldInit,
            "IsAbstract": <Boolean> false,
            "IsImport": <Boolean> false,
            "IsSealed": <Boolean> true,
            "IsSpecialName": <Boolean> false,
            "IsClass": <Boolean> false,
            "IsNestedAssembly": <Boolean> false,
            "IsNestedFamANDAssem": <Boolean> false,
            "IsNestedFamily": <Boolean> false,
            "IsNestedFamORAssem": <Boolean> false,
            "IsNestedPrivate": <Boolean> false,
            "IsNestedPublic": <Boolean> false,
            "IsNotPublic": <Boolean> false,
            "IsPublic": <Boolean> true,
            "IsAutoLayout": <Boolean> false,
            "IsExplicitLayout": <Boolean> false,
            "IsLayoutSequential": <Boolean> true,
            "IsAnsiClass": <Boolean> true,
            "IsAutoClass": <Boolean> false,
            "IsUnicodeClass": <Boolean> false,
            "IsCOMObject": <Boolean> false,
            "IsContextful": <Boolean> false,
            "IsMarshalByRef": <Boolean> false,
            "IsPrimitive": <Boolean> true,
            "IsValueType": <Boolean> true,
            "IsSignatureType": <Boolean> false,
            "TypeInitializer": <ConstructorInfo> null,
            "IsSerializable": <Boolean> true,
            "IsVisible": <Boolean> true,
            "CustomAttributes": <IEnumerable<CustomAttributeData>> [
                [0]: SerializableAttribute,
                [1]: IsReadOnlyAttribute,
                [2]: TypeForwardedFromAttribute
            ]
        }
        """;

    #endregion
}
