using System.Diagnostics.CodeAnalysis;

namespace DotNetToolbox.Extensions;

public class ObjectExtensionsTests {
    private class TestDataForPrimitives : TheoryData<object?, string> {
        public TestDataForPrimitives() {
            Add(null, "null");
            Add(true, "true");
            Add((byte)42, "42");
            Add((sbyte)42, "42");
            Add((char)42, "'*'");
            Add((short)42, "42");
            Add((ushort)42, "42");
            Add(42, "42");
            Add((uint)42, "42");
            Add((long)42, "42");
            Add((ulong)42, "42");
            Add((float)42.7, "42.7");
            Add(42.7, "42.7");
            Add(42.7m, "42.7");
            Add(new DateTime(2001, 10, 12), "2001-10-12T00:00:00.0000000");
            Add(new DateTimeOffset(new DateTime(2001, 10, 12), TimeSpan.FromHours(-5)), "2001-10-12T00:00:00.0000000-05:00");
            Add(new DateOnly(2001, 10, 12), "2001-10-12");
            Add(new TimeOnly(23, 15, 52), "23:15:52.0000000");
            Add(new TimeSpan(23, 15, 52), "23:15:52");
            Add(typeof(int), "System.Int32");
            Add(Guid.Parse("b6d3aec4-daca-4dca-ada7-cda51623ed50"), "b6d3aec4-daca-4dca-ada7-cda51623ed50");
            Add(CultureInfo.GetCultureInfo("en-CA"), "en-CA");
        }
    }
    [Theory]
    [ClassData(typeof(TestDataForPrimitives))]
    public void Dump_ForSimpleTypes_ValueString(object? value, string expectedText) {
        // Arrange & Act
        var result = value.Dump();

        // Assert
        result.Should().Be(expectedText);
    }

    private class TestDataForCollections : TheoryData<object?, string> {
        public TestDataForCollections() {
            Add(new[] { 1, 2, 3 },
                """
                [
                  1,
                  2,
                  3
                ]
                """);
            Add(new List<int> { 1, 2, 3 },
                """
                [
                  1,
                  2,
                  3
                ]
                """);
            Add(new Dictionary<string, double> { ["A"] = 1.1, ["B"] = 2.2, ["C"] = 3.3 },
                """
                 [
                   "A": 1.1,
                   "B": 2.2,
                   "C": 3.3
                 ]
                 """);
        }
    }
    [Theory]
    [ClassData(typeof(TestDataForCollections))]
    public void Dump_ForCollections_ValueString(object? value, string expectedText) {
        // Arrange & Act
        var result = value.Dump();

        // Assert
        result.Should().Be(expectedText);
    }

    private class TestClass(int intValue, string stringValue) {
        public int IntProperty { get; set; } = intValue;
        public string StringProperty { get; set; } = stringValue;
    }
    private record TestRecord(int IntProperty, string StringProperty) {
    }
    private struct TestStruct(int intValue, string stringValue) {
        public int IntProperty { get; set; } = intValue;
        public string StringProperty { get; set; } = stringValue;
    }
    private class TestDataForComplexTypes : TheoryData<object?, string, string> {
        public TestDataForComplexTypes() {
            Add(new TestClass(42, "Text"),
                """
                 {
                   "IntProperty": 42,
                   "StringProperty": "Text"
                 }
                 """,
                """{"IntProperty":42,"StringProperty":"Text"}"""
                );
            Add(new TestRecord(42, "Text"),
                """
                 {
                   "IntProperty": 42,
                   "StringProperty": "Text"
                 }
                 """,
                """{"IntProperty":42,"StringProperty":"Text"}"""
                );
            Add(new TestStruct(42, "Text"),
                """
                 {
                   "IntProperty": 42,
                   "StringProperty": "Text"
                 }
                 """,
                """{"IntProperty":42,"StringProperty":"Text"}"""
                );
            Add(new Dictionary<string, TestStruct> {
                ["A"] = new(42, "Text"),
                ["B"] = new(7, "Other"),
            },
                """
                  [
                    "A": {
                      "IntProperty": 42,
                      "StringProperty": "Text"
                    },
                    "B": {
                      "IntProperty": 7,
                      "StringProperty": "Other"
                    }
                  ]
                  """,
                """["A":{"IntProperty":42,"StringProperty":"Text"},"B":{"IntProperty":7,"StringProperty":"Other"}]"""
                );
            Add(new List<List<int>> { new() { 1, 2, 3 }, new() { 1, 2, 3 }, new() { 1, 2, 3 } },
                """
                [
                  [
                    1,
                    2,
                    3
                  ],
                  [
                    1,
                    2,
                    3
                  ],
                  [
                    1,
                    2,
                    3
                  ]
                ]
                """,
                "[0:[0:1,1:2,2:3],1:[0:1,1:2,2:3],2:[0:1,1:2,2:3]]"
                );
        }
    }
    [Theory]
    [ClassData(typeof(TestDataForComplexTypes))]
    public void Dump_ForComplexTypes_ValueString(object? value, string indentedText, string compactText) {
        // Arrange & Act
        var indented = value.Dump();
        var compact = value.Dump(opt => {
                                     opt.IndentOutput = false;
                                     opt.ShowListIndexes = true;
                                 });

        // Assert
        indented.Should().Be(indentedText);
        compact.Should().Be(compactText);
    }
}
