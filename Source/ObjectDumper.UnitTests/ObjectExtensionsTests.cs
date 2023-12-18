namespace DotNetToolbox;

public class ObjectExtensionsTests {
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
    [Theory]
    [ClassData(typeof(TestDataForPrimitives))]
    public void Dump_ForSimpleTypes_ReturnsString(object? value, string expectedText) {
        // Arrange & Act
        var result = value.Dump();

        //Assert
        result.Should().Be(expectedText);
    }

    [Fact]
    public void Dump_ForSimpleTypes_WithFullName_ReturnsString() {
        // Arrange & Act
        var result = "Value".Dump(opt => opt.ShowFullNames = true);

        // Assert
        result.Should().Be("<System.String> \"Value\"");
    }

    private class TestDataForCollections : TheoryData<object?, string> {
        public TestDataForCollections() {
            Add(new[] { 1, 2, 3 },
                """
                <Int32[]> [
                    1,
                    2,
                    3
                ]
                """);
            Add(new List<int> { 1, 2, 3 },
                """
                <List<Int32>> [
                    1,
                    2,
                    3
                ]
                """);
            Add(new Dictionary<string, double> { ["A"] = 1.1, ["B"] = 2.2, ["C"] = 3.3 },
                """
                <Dictionary<String, Double>> [
                    "A": 1.1,
                    "B": 2.2,
                    "C": 3.3
                ]
                """);
        }
    }
    [Theory]
    [ClassData(typeof(TestDataForCollections))]
    public void Dump_ForCollections_ReturnsString(object? value, string expectedText) {
        // Arrange & Act
        var result = value.Dump();

        // Assert
        result.Should().Be(expectedText);
    }

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private class TestClass(int intValue, string stringValue) {
        public int IntProperty { get; init; } = intValue;
        public string StringProperty { get; set; } = stringValue;
    }

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private class TestClassWithGeneric<T>(int intValue, string stringValue) {
        public int IntProperty { get; init; } = intValue;
        public string StringProperty { get; set; } = stringValue;
        public Func<T>? ConvertTo { get; set; }
    }

    [SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Local")]
    private record TestRecord(int IntProperty, string StringProperty);

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private struct TestStruct(int intValue, string stringValue) {
        public int IntProperty { get; set; } = intValue;
        public string StringProperty { get; } = stringValue;
    }
    private class TestDataForComplexTypes : TheoryData<object?, string, string> {
        public TestDataForComplexTypes() {
            Add(new TestClass(42, "Text"),
                """
                <TestClass> {
                    "IntProperty": <Int32> 42,
                    "StringProperty": <String> "Text"
                }
                """,
                """{"IntProperty":42,"StringProperty":"Text"}"""
                );
            Add(new TestClassWithGeneric<int>(42, "Text"),
                """
                <ObjectExtensionsTests+TestClassWithGeneric<Int32>> {
                    "IntProperty": <Int32> 42,
                    "StringProperty": <String> "Text",
                    "ConvertTo": <Func<Int32>> null
                }
                """,
                """
                {"IntProperty":42,"StringProperty":"Text","ConvertTo":null}
                """
               );
            Add(new TestRecord(42, "Text"),
                """
                <TestRecord> {
                    "IntProperty": <Int32> 42,
                    "StringProperty": <String> "Text"
                }
                """,
                """{"IntProperty":42,"StringProperty":"Text"}"""
               );
            Add(new TestStruct(42, "Text"),
                """
                <TestStruct> {
                    "IntProperty": <Int32> 42,
                    "StringProperty": <String> "Text"
                }
                """,
                """{"IntProperty":42,"StringProperty":"Text"}"""
                );
            Add(new Dictionary<string, TestStruct> {
                ["A"] = new(42, "Text"),
                ["B"] = new(7, "Other"),
            },
                """
                <Dictionary<String, TestStruct>> [
                    "A": {
                        "IntProperty": <Int32> 42,
                        "StringProperty": <String> "Text"
                    },
                    "B": {
                        "IntProperty": <Int32> 7,
                        "StringProperty": <String> "Other"
                    }
                ]
                """,
                """["A":{"IntProperty":42,"StringProperty":"Text"},"B":{"IntProperty":7,"StringProperty":"Other"}]"""
                );
            Add(new List<List<int>> { new() { 1, 2, 3 }, new() { 1, 2, 3 }, new() { 1, 2, 3 } },
                """
                <List<List<Int32>>> [
                    0: [
                        0: 1,
                        1: 2,
                        2: 3
                    ],
                    1: [
                        0: 1,
                        1: 2,
                        2: 3
                    ],
                    2: [
                        0: 1,
                        1: 2,
                        2: 3
                    ]
                ]
                """,
                "[[1,2,3],[1,2,3],[1,2,3]]"
                );
        }
    }
    [Theory]
    [ClassData(typeof(TestDataForComplexTypes))]
    public void Dump_ComplexType_ReturnsString(object? value, string indentedText, string compactText) {
        // Arrange & Act
        var indented = value.Dump(opt => opt.ShowItemIndex = true);
        var compact = value.Dump(opt => {
            opt.Indented = false;
            opt.Layout = Layout.Json;
        });

        // Assert
        indented.Should().Be(indentedText);
        compact.Should().Be(compactText);
    }

    [Fact]
    public void Dump_VeryComplexType_LimitMaxLevel_ReturnsString() {
        // Arrange & Act
        var result = CultureInfo.GetCultureInfo("en-CA").Dump(opt => opt.MaxLevel = 2);

        // Assert
        result.Should().Be("""
                            <CultureInfo> {
                                "DefaultThreadCurrentCulture": <CultureInfo> null,
                                "LCID": <Int32> 4105,
                                "KeyboardLayoutId": <Int32> 4105,
                                "Name": <String> "en-CA",
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
                            """);
    }

    [Fact]
    public void Dump_ComplexTypes_WithFullName_ReturnsString() {
        // Arrange & Act
        var result = new TestClass(42, "Text").Dump(opt => opt.ShowFullNames = true);

        // Assert
        result.Should().Be("""
                           <DotNetToolbox.ObjectExtensionsTests+TestClass> {
                               "IntProperty": <System.Int32> 42,
                               "StringProperty": <System.String> "Text"
                           }
                           """);
    }

    [Fact]
    public void Dump_ExtremelyComplexType_ReturnsString() {
        // Arrange & Act
        var result = typeof(int).Dump();

        // Assert
        result.Should().Be("""
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
                                    Int32 MaxValue,
                                    Int32 MinValue
                                ],
                                "DeclaredMembers": <IEnumerable<MemberInfo>> [
                                    Int32 CompareTo(Object value),
                                    Int32 CompareTo(Int32 value),
                                    Boolean Equals(Object obj),
                                    Boolean Equals(Int32 obj),
                                    Int32 GetHashCode(),
                                    String ToString(),
                                    String ToString(String format),
                                    String ToString(IFormatProvider provider),
                                    String ToString(String format, IFormatProvider provider),
                                    Boolean TryFormat(Span<Char> destination, Int32& charsWritten, ReadOnlySpan<Char> format, IFormatProvider provider),
                                    Boolean TryFormat(Span<Byte> utf8Destination, Int32& bytesWritten, ReadOnlySpan<Char> format, IFormatProvider provider),
                                    Int32 Parse(String s),
                                    Int32 Parse(String s, NumberStyles style),
                                    Int32 Parse(String s, IFormatProvider provider),
                                    Int32 Parse(String s, NumberStyles style, IFormatProvider provider),
                                    Int32 Parse(ReadOnlySpan<Char> s, NumberStyles style, IFormatProvider provider),
                                    Boolean TryParse(String s, Int32& result),
                                    Boolean TryParse(ReadOnlySpan<Char> s, Int32& result),
                                    Boolean TryParse(ReadOnlySpan<Byte> utf8Text, Int32& result),
                                    Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, Int32& result),
                                    Boolean TryParse(ReadOnlySpan<Char> s, NumberStyles style, IFormatProvider provider, Int32& result),
                                    TypeCode GetTypeCode(),
                                    ValueTuple<Int32, Int32> DivRem(Int32 left, Int32 right),
                                    Int32 LeadingZeroCount(Int32 value),
                                    Int32 PopCount(Int32 value),
                                    Int32 RotateLeft(Int32 value, Int32 rotateAmount),
                                    Int32 RotateRight(Int32 value, Int32 rotateAmount),
                                    Int32 TrailingZeroCount(Int32 value),
                                    Boolean IsPow2(Int32 value),
                                    Int32 Log2(Int32 value),
                                    Int32 Clamp(Int32 value, Int32 min, Int32 max),
                                    Int32 CopySign(Int32 value, Int32 sign),
                                    Int32 Max(Int32 x, Int32 y),
                                    Int32 Min(Int32 x, Int32 y),
                                    Int32 Sign(Int32 value),
                                    Int32 Abs(Int32 value),
                                    Int32 CreateChecked(TOther value),
                                    Int32 CreateSaturating(TOther value),
                                    Int32 CreateTruncating(TOther value),
                                    Boolean IsEvenInteger(Int32 value),
                                    Boolean IsNegative(Int32 value),
                                    Boolean IsOddInteger(Int32 value),
                                    Boolean IsPositive(Int32 value),
                                    Int32 MaxMagnitude(Int32 x, Int32 y),
                                    Int32 MinMagnitude(Int32 x, Int32 y),
                                    Boolean TryParse(String s, IFormatProvider provider, Int32& result),
                                    Int32 Parse(ReadOnlySpan<Char> s, IFormatProvider provider),
                                    Boolean TryParse(ReadOnlySpan<Char> s, IFormatProvider provider, Int32& result),
                                    Int32 Parse(ReadOnlySpan<Byte> utf8Text, NumberStyles style, IFormatProvider provider),
                                    Boolean TryParse(ReadOnlySpan<Byte> utf8Text, NumberStyles style, IFormatProvider provider, Int32& result),
                                    Int32 Parse(ReadOnlySpan<Byte> utf8Text, IFormatProvider provider),
                                    Boolean TryParse(ReadOnlySpan<Byte> utf8Text, IFormatProvider provider, Int32& result),
                                    Int32 System.Numerics.IAdditiveIdentity<System.Int32,System.Int32>.AdditiveIdentity,
                                    Int32 System.Numerics.IBinaryNumber<System.Int32>.AllBitsSet,
                                    Int32 System.Numerics.IMinMaxValue<System.Int32>.MinValue,
                                    Int32 System.Numerics.IMinMaxValue<System.Int32>.MaxValue,
                                    Int32 System.Numerics.IMultiplicativeIdentity<System.Int32,System.Int32>.MultiplicativeIdentity,
                                    Int32 System.Numerics.INumberBase<System.Int32>.One,
                                    Int32 System.Numerics.INumberBase<System.Int32>.Radix,
                                    Int32 System.Numerics.INumberBase<System.Int32>.Zero,
                                    Int32 System.Numerics.ISignedNumber<System.Int32>.NegativeOne,
                                    Boolean System.IBinaryIntegerParseAndFormatInfo<System.Int32>.IsSigned,
                                    Int32 System.IBinaryIntegerParseAndFormatInfo<System.Int32>.MaxDigitCount,
                                    Int32 System.IBinaryIntegerParseAndFormatInfo<System.Int32>.MaxHexDigitCount,
                                    Int32 System.IBinaryIntegerParseAndFormatInfo<System.Int32>.MaxValueDiv10,
                                    String System.IBinaryIntegerParseAndFormatInfo<System.Int32>.OverflowMessage,
                                ],
                                "DeclaredMethods": <IEnumerable<MethodInfo>> [
                                ],
                                "DeclaredNestedTypes": <IEnumerable<TypeInfo>> [
                                ],
                                "DeclaredProperties": <IEnumerable<PropertyInfo>> [
                                ],
                                "ImplementedInterfaces": <IEnumerable<Type>> [
                                    IComparable,
                                    IConvertible,
                                    ISpanFormattable,
                                    IFormattable,
                                    IComparable<Int32>,
                                    IEquatable<Int32>,
                                    IBinaryInteger<Int32>,
                                    IBinaryNumber<Int32>,
                                    IBitwiseOperators<Int32, Int32, Int32>,
                                    INumber<Int32>,
                                    IComparisonOperators<Int32, Int32, Boolean>,
                                    IEqualityOperators<Int32, Int32, Boolean>,
                                    IModulusOperators<Int32, Int32, Int32>,
                                    INumberBase<Int32>,
                                    IAdditionOperators<Int32, Int32, Int32>,
                                    IAdditiveIdentity<Int32, Int32>,
                                    IDecrementOperators<Int32>,
                                    IDivisionOperators<Int32, Int32, Int32>,
                                    IIncrementOperators<Int32>,
                                    IMultiplicativeIdentity<Int32, Int32>,
                                    IMultiplyOperators<Int32, Int32, Int32>,
                                    ISpanParsable<Int32>,
                                    IParsable<Int32>,
                                    ISubtractionOperators<Int32, Int32, Int32>,
                                    IUnaryPlusOperators<Int32, Int32>,
                                    IUnaryNegationOperators<Int32, Int32>,
                                    IUtf8SpanFormattable,
                                    IUtf8SpanParsable<Int32>,
                                    IShiftOperators<Int32, Int32, Int32>,
                                    IMinMaxValue<Int32>,
                                    ISignedNumber<Int32>,
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
                                    SerializableAttribute,
                                    IsReadOnlyAttribute,
                                    TypeForwardedFromAttribute
                                ]
                            }
                            """);
    }
}
