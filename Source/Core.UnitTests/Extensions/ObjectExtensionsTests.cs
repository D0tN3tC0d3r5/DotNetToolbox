namespace DotNetToolbox.Extensions;

public class ObjectExtensionsTests {
    private class TestDataForPrimitives : TheoryData<object?, string> {
        public TestDataForPrimitives() {
            Add(null, "null");
            Add(true, "(Boolean)true");
            Add((byte)42, "(Byte)42");
            Add((sbyte)42, "(SByte)42");
            Add((char)42, "(Char)'*'");
            Add((short)42, "(Int16)42");
            Add((ushort)42, "(UInt16)42");
            Add(42, "(Int32)42");
            Add((uint)42, "(UInt32)42");
            Add((long)42, "(Int64)42");
            Add((ulong)42, "(UInt64)42");
            Add((float)42.7, "(Single)42.7");
            Add(42.7, "(Double)42.7");
            Add(42.7m, "(Decimal)42.7");
            Add(new DateTime(2001, 10, 12), "(DateTime)2001-10-12T00:00:00.0000000");
            Add(new DateTimeOffset(new DateTime(2001, 10, 12), TimeSpan.FromHours(-5)), "(DateTimeOffset)2001-10-12T00:00:00.0000000-05:00");
            Add(new DateOnly(2001, 10, 12), "(DateOnly)2001-10-12");
            Add(new TimeOnly(23, 15, 52), "(TimeOnly)23:15:52.0000000");
            Add(new TimeSpan(23, 15, 52), "(TimeSpan)23:15:52");
            Add(typeof(int), "(RuntimeType)Int32");
            Add(Guid.Parse("b6d3aec4-daca-4dca-ada7-cda51623ed50"), "(Guid)b6d3aec4-daca-4dca-ada7-cda51623ed50");
        }
    }
    [Theory]
    [ClassData(typeof(TestDataForPrimitives))]
    public void Dump_ForSimpleTypes_ReturnsString(object? value, string expectedText) {
        // Arrange & Act
        var result = value.Dump();

        // Assert
        result.Should().Be(expectedText);
    }

    [Fact]
    public void Dump_ForSimpleTypes_WithFullName_ReturnsString() {
        // Arrange & Act
        var result = "Value".Dump(opt => opt.ShowTypeFullName = true);

        // Assert
        result.Should().Be("(System.String)\"Value\"");
    }

    private class TestDataForCollections : TheoryData<object?, string> {
        public TestDataForCollections() {
            Add(new[] { 1, 2, 3 },
                """
                (Int32[])[
                  1,
                  2,
                  3
                ]
                """);
            Add(new List<int> { 1, 2, 3 },
                """
                (List<Int32>)[
                  1,
                  2,
                  3
                ]
                """);
            Add(new Dictionary<string, double> { ["A"] = 1.1, ["B"] = 2.2, ["C"] = 3.3 },
                """
                 (Dictionary<String, Double>)[
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

    private class TestClass(int intValue, string stringValue) {
        public int IntProperty { get; init; } = intValue;
        public string StringProperty { get; set; } = stringValue;
    }
    private record TestRecord(int IntProperty, string StringProperty) {
    }
    private struct TestStruct(int intValue, string stringValue) {
        public int IntProperty { get; set; } = intValue;
        public string StringProperty { get; } = stringValue;
    }
    private class TestDataForComplexTypes : TheoryData<object?, string, string> {
        public TestDataForComplexTypes() {
            Add(new TestClass(42, "Text"),
                """
                 (TestClass){
                   "IntProperty": (Int32)42,
                   "StringProperty": (String)"Text"
                 }
                 """,
                """{"IntProperty":42,"StringProperty":"Text"}"""
                );
            Add(new TestRecord(42, "Text"),
                """
                 (TestRecord){
                   "IntProperty": (Int32)42,
                   "StringProperty": (String)"Text"
                 }
                 """,
                """{"IntProperty":42,"StringProperty":"Text"}"""
                );
            Add(new TestStruct(42, "Text"),
                """
                 (TestStruct){
                   "IntProperty": (Int32)42,
                   "StringProperty": (String)"Text"
                 }
                 """,
                """{"IntProperty":42,"StringProperty":"Text"}"""
                );
            Add(new Dictionary<string, TestStruct> {
                ["A"] = new(42, "Text"),
                ["B"] = new(7, "Other"),
            },
                """
                  (Dictionary<String, TestStruct>)[
                    "A": {
                      "IntProperty": (Int32)42,
                      "StringProperty": (String)"Text"
                    },
                    "B": {
                      "IntProperty": (Int32)7,
                      "StringProperty": (String)"Other"
                    }
                  ]
                  """,
                """["A":{"IntProperty":42,"StringProperty":"Text"},"B":{"IntProperty":7,"StringProperty":"Other"}]"""
                );
            Add(new List<List<int>> { new() { 1, 2, 3 }, new() { 1, 2, 3 }, new() { 1, 2, 3 } },
                """
                (List<List<Int32>>)[
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
    public void Dump_ForComplexTypes_ReturnsString(object? value, string indentedText, string compactText) {
        // Arrange & Act
        var indented = value.Dump(opt => opt.ShowListIndexes = true);
        var compact = value.Dump(opt => {
                                     opt.IndentOutput = false;
                                     opt.ShowType = false;
                                 });

        // Assert
        indented.Should().Be(indentedText);
        compact.Should().Be(compactText);
    }

    [Fact]
    public void Dump_ForVeryComplexTypes_LimitMaxLevel_ReturnsString() {
        // Arrange & Act
        var result = CultureInfo.GetCultureInfo("en-CA").Dump(opt => opt.MaxLevel = 2);

        // Assert
        result.Should().Be("""
                           (CultureInfo){
                             "Parent": (CultureInfo)...,
                             "LCID": (Int32)4105,
                             "KeyboardLayoutId": (Int32)4105,
                             "Name": (String)"en-CA",
                             "DisplayName": (String)"English (Canada)",
                             "NativeName": (String)"English (Canada)",
                             "EnglishName": (String)"English (Canada)",
                             "TwoLetterISOLanguageName": (String)"en",
                             "ThreeLetterISOLanguageName": (String)"eng",
                             "ThreeLetterWindowsLanguageName": (String)"ENC",
                             "CompareInfo": (CompareInfo)...,
                             "TextInfo": (TextInfo)...,
                             "IsNeutralCulture": (Boolean)false,
                             "CultureTypes": (CultureTypes)...,
                             "NumberFormat": (NumberFormatInfo)...,
                             "DateTimeFormat": (DateTimeFormatInfo)...,
                             "Calendar": (Calendar)...,
                             "OptionalCalendars": (Calendar[])...,
                             "UseUserOverride": (Boolean)false,
                             "IsReadOnly": (Boolean)true
                           }
                           """);
    }

    [Fact]
    public void Dump_ForComplexTypes_WithFullName_ReturnsString() {
        // Arrange & Act
        var result = new TestClass(42, "Text").Dump(opt => opt.ShowTypeFullName = true);

        // Assert
        result.Should().Be("""
                           (DotNetToolbox.Extensions.ObjectExtensionsTests+TestClass){
                             "IntProperty": (System.Int32)42,
                             "StringProperty": (System.String)"Text"
                           }
                           """);
    }
}
