namespace DotNetToolbox;

public class ObjectExtensionsTests {
    [Fact]
    public void JsonDumpBuilderOptions_CopyConstructor_CreateOptions() {
        // Arrange
        var options1 = new JsonDumpBuilderOptions();

        // Act
        var options2 = options1 with { };

        //Assert
        options2.Should().NotBeSameAs(options1);
    }

    [Fact]
    public void DumpBuilderOptions_CopyConstructor_CreateOptions() {
        // Arrange
        var options1 = new DumpBuilderOptions();

        // Act
        var options2 = options1 with { };

        //Assert
        options2.Should().NotBeSameAs(options1);
    }

    [Theory]
    [ClassData(typeof(TestDataForPrimitives))]
    public void Dump_SimpleTypes_ReturnsString(object? value, string expectedText) {
        // Arrange & Act
        var result = value.Dump();

        //Assert
        result.Should().Be(expectedText);
    }

    [Theory]
    [ClassData(typeof(TestDataForComplexType))]
    public void Dump_ForCollections_ReturnsString(object? value, string expectedText) {
        // Arrange & Act
        var result = value.Dump();

        // Assert
        result.Should().Be(expectedText);
    }

    private static readonly JsonSerializerOptions _indentedJson = new() {
        WriteIndented = true,
    };
    [Theory]
    [ClassData(typeof(TestDataForJson))]
    public void Dump_AsJson_ReturnsString(object? subject) {
        // Arrange
        var expectedText = JsonSerializer.Serialize(subject, _indentedJson);

        //Act
        var result = subject.DumpAsJson();

        // Assert
        result.Should().Be(expectedText);
    }

    [Theory]
    [ClassData(typeof(TestDataForJson))]
    public void Dump_AsNotIndentedJson_ReturnsString(object? subject) {
        // Arrange
        var expectedText = JsonSerializer.Serialize(subject);

        //Act
        var result = subject.DumpAsJson(opt => opt.Indented = false);

        // Assert
        result.Should().Be(expectedText);
    }

    [Fact]
    public void Dump_WithCustomIndentSize_ReturnsString() {
        // Arrange & Act
        var result = _listOfLists.Dump(opt => opt.IndentSize = 2);

        //Assert
        result.Should().Be(_listOfListsDump2Spaces);
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
    public void Dump_WithTabs_ReturnsString() {
        // Arrange & Act
        var result = new TestClass(42, "Text").Dump(opt => opt.UseTabs = true);

        //Assert
        result.Should().Be(_testWithTabs);
    }

    [Fact]
    public void Dump_SpecialElements_ReturnsString() {
        // Arrange & Act
        _listOfObjects.Add(_listOfObjects);
        var result = _listOfObjects.Dump();

        // Assert
        result.Should().Be(_listOfObjectsDump);
    }

    [Theory]
    [ClassData(typeof(TestDataForNotIndented))]
    public void Dump_NotIndented_ReturnsString(object? value, string expectedText) {
        // Arrange & Act
        var result = value.Dump(opt => opt.Indented = false);

        // Assert
        result.Should().Be(expectedText);
    }

    [Theory]
    [ClassData(typeof(TestDataForFullName))]
    public void Dump_WithFullName_ReturnsString(object? value, string expectedText) {
        // Arrange & Act
        var result = value.Dump(opt => opt.UseFullNames = true);

        // Assert
        result.Should().Be(expectedText);
    }

    [Theory]
    [ClassData(typeof(TestDataForMaxDepth))]
    public void Dump_VeryComplexType_LimitMaxLevel_ReturnsString(byte maxLevel, string expectedText) {
        // Arrange & Act
        var result = CultureInfo.GetCultureInfo("en-CA").Dump(opt => opt.MaxDepth = maxLevel);

        // Assert
        result.Should().Match(expectedText);
    }

    [Theory]
    [InlineData(typeof(int), _integerTypeDump)]
    [InlineData(typeof(CustomClass<>), _customClassTypeDump)]
    public void Dump_ExtremelyComplexType_ReturnsString(object value, string expectedText) {
        // Arrange & Act
        var result = value.Dump();

        // Assert
        result.Should().Be(expectedText);
    }

    [Theory]
    [InlineData(typeof(int))]
    public void Dump_NotSupportedTypes_AsJson_ReturnsString(object value) {
        // Arrange & Act
        var action = () => value.DumpAsJson();

        // Assert
        action.Should().Throw<NotSupportedException>();
    }

    #region Test Data

    #region Type defnitions

    [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private class TestClass(int intValue, string stringValue) {
        public int IntProperty { get; init; } = intValue;
        public string StringProperty { get; set; } = stringValue;
    }

    public interface ICustomClass<out T> {
        T Value { get; }
    };

    [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private class CustomClass<T>(T? value) : ICustomClass<T> {
        [SuppressMessage("Roslynator", "RCS1158:Static member in generic type should use a type parameter", Justification = "<Pending>")]
        public const string Name = "CustomClass";

        // ReSharper disable once MemberCanBePrivate.Local
        public static readonly T Default = default!;

        public T Value { get; } = value ?? Default;
        public TValue ConvertTo<TValue>(object? obj) {
            var result = (TValue)Convert.ChangeType(obj, typeof(TValue))!;
            OnConverted?.Invoke(this, new() { Value = result });
            return result;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        // ReSharper disable once UnusedType.Local
        public delegate void EventHandler<in TConvertedArgs>(object sender, TConvertedArgs e);
        // ReSharper disable once EventNeverSubscribedTo.Local
        public event EventHandler<ConvertedArgs> OnConverted = (_, _) => { };
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        public class ConvertedArgs : EventArgs {
            public object? Value { get; set; }
        }
    }

    [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private class TestClassWithGeneric<T>(T value) {
        public T Property { get; set; } = value;
        public Func<T>? ConvertTo { get; set; }
    }

    [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
    [SuppressMessage("ReSharper", "NotAccessedPositionalProperty.Local")]
    private record TestRecord(int IntProperty, string StringProperty);

    [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>")]
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private struct TestStruct(int intValue, string stringValue) {
        public int IntProperty { get; set; } = intValue;
        public string StringProperty { get; } = stringValue;
    }

    #endregion

    private static readonly int[] _array = [1, 2, 3];
    private static readonly List<int> _list = [1, 2, 3];
    private static readonly Stack<int> _stack = new([1, 2, 3]);
    private static readonly int[][] _multiDimensionArray = [_array, _array, _array];
    private static readonly List<List<int>> _listOfLists = [_list, _list, _list];
    private static readonly List<object?> _listOfObjects = [null, _list];
    private static readonly Dictionary<string, double> _dictionary = new() { ["A"] = 1.1, ["B"] = 2.2, ["C"] = 3.3 };

    private class TestDataForPrimitives : TheoryData<object?, string> {
        public TestDataForPrimitives() {
            Add(null, "null");
            Add(true, "<Boolean> true");
            Add((nint)42, "<IntPtr> 42");
            Add((nuint)42, "<UIntPtr> 42");
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
            Add(new DateTime(2001, 10, 12), $"<DateTime> {new DateOnly(2001, 10, 12).ToString(CultureInfo.CurrentCulture)} 00:00:00");
            Add(new DateTimeOffset(new DateTime(2001, 10, 12), TimeSpan.FromHours(-5)), $"<DateTimeOffset> {new DateOnly(2001, 10, 12).ToString(CultureInfo.CurrentCulture)} 00:00:00 -05:00");
            Add(new DateOnly(2001, 10, 12), $"<DateOnly> {new DateOnly(2001, 10, 12).ToString(CultureInfo.CurrentCulture)}");
            Add(new TimeOnly(23, 15, 52), "<TimeOnly> 23:15");
            Add(new TimeSpan(23, 15, 52), "<TimeSpan> 23:15:52");
            Add(Guid.Parse("b6d3aec4-daca-4dca-ada7-cda51623ed50"), "<Guid> b6d3aec4-daca-4dca-ada7-cda51623ed50");
        }
    }
    private class TestDataForFullName : TheoryData<object?, string> {
        public TestDataForFullName() {
            Add(new TestClass(42, "Text"), _fullNamedTestClassDump);
            Add(typeof(CustomClass<>), _fullNamedCustomClassTypeDump);
        }
    }
    private class TestDataForJson : TheoryData<object?> {
        public TestDataForJson() {
            Add(null);
            Add(true);
            Add('A');
            Add(Guid.NewGuid());
            Add(new DateTimeOffset(new DateTime(2001, 10, 12), TimeSpan.FromHours(-5)));
            Add(new DateTime(2001, 10, 12));
            Add(new DateOnly(2001, 10, 12));
            Add(new TimeOnly(23, 15, 52));
            Add(new TimeSpan(23, 15, 52));
            Add(new List<int>([1, 2, 3]));
            var dict = new Dictionary<string, TestClass> {
                ["One"] = new(42, "Test"),
                ["Two"] = new(7, "Other"),
            };
            Add(dict);
            Add(new CustomClass<int>(42));
            Add(new TestClass(42, "Text"));
        }
    }
    private class TestDataForComplexType : TheoryData<object?, string> {
        public TestDataForComplexType() {
            Add(_array, _arrayDump);
            Add(_list, _listDump);
            Add(_stack, _stackDump);
            Add(_dictionary, _dictionaryOfStringDoubleDump);
            Add(new TestClass(42, "Text"), _testClassDump);
            Add(new TestClassWithGeneric<int>(42), _testGenericClassWithInt32Dump);
            Add(new TestRecord(42, "Text"), _testRecordDump);
            Add(new TestStruct(42, "Text"), _testStructDump);
            Add(new Dictionary<string, TestStruct> {
                ["A"] = new(42, "Text"),
                ["B"] = new(7, "Other"),
            }, _testDictionaryDump);
            Add(_listOfLists, _listOfListsDump);
            Add(_multiDimensionArray, _multiDimensionArrayDump);
        }
    }
    private class TestDataForMaxDepth : TheoryData<byte, string> {
        public TestDataForMaxDepth() {
            Add(0, _cultureInfoDumpLvl0);
            Add(1, _cultureInfoDumpLvl1);
            Add(2, _cultureInfoDumpLvl2);
            Add(3, _cultureInfoDumpLvl3);
        }
    }
    private class TestDataForNotIndented : TheoryData<object?, string> {
        public TestDataForNotIndented() {
            Add(new TestClass(42, "Text"), _testClassCompactDump);
            Add(new TestClassWithGeneric<double>(42), _testGenericClassDoubleCompactDump);
            Add(new TestRecord(42, "Text"), _testRecordCompactDump);
            Add(new TestStruct(42, "Text"), _testStructCompactDump);
            Add(new Dictionary<string, TestStruct> {
                ["A"] = new(42, "Text"),
                ["B"] = new(7, "Other"),
            }, _testDictionaryCompactDump);
            Add(_listOfLists, _listOfListsCompactDump);
        }
    }

    private const string _arrayDump = """
        <Int32[]> [
            1,
            2,
            3
        ]
        """;
    private const string _listDump = """
        <List<Int32>> [
            1,
            2,
            3
        ]
        """;
    private const string _stackDump = """
         <Stack<Int32>> [
             3,
             2,
             1
         ]
         """;
    private const string _dictionaryOfStringDoubleDump = """
        <Dictionary<String, Double>> [
            ["A"] = 1.1,
            ["B"] = 2.2,
            ["C"] = 3.3
        ]
        """;
    private const string _multiDimensionArrayDump = """
        <Int32[][]> [
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
        """;
    private const string _listOfListsDump = """
        <List<List<Int32>>> [
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
        """;
    private const string _listOfListsDump2Spaces = """
        <List<List<Int32>>> [
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
        """;
    private const string _listOfListsCompactDump = """
        <List<List<Int32>>>[[1,2,3],[1,2,3],[1,2,3]]
        """;
    private const string _listOfObjectsDump = """
        <List<Object>> [
            null,
            [
                1,
                2,
                3
            ],
            #CircularReference#
        ]
        """;
    private const string _testClassDump = """
        <TestClass> {
            "IntProperty": <Int32> 42,
            "StringProperty": <String> "Text"
        }
        """;
    private const string _testWithTabs = """
        <TestClass> {
        	"IntProperty": <Int32> 42,
        	"StringProperty": <String> "Text"
        }
        """;
    private const string _testClassCompactDump = """
         <TestClass>{"IntProperty":<Int32>42,"StringProperty":<String>"Text"}
         """;
    private const string _testGenericClassWithInt32Dump = """
        <ObjectExtensionsTests+TestClassWithGeneric<Int32>> {
            "Property": <Int32> 42,
            "ConvertTo": <Func<Int32>> null
        }
        """;
    private const string _testGenericClassDoubleCompactDump = """
        <ObjectExtensionsTests+TestClassWithGeneric<Double>>{"Property":<Double>42,"ConvertTo":<Func<Double>>null}
        """;
    private const string _testRecordDump = """
        <TestRecord> {
            "IntProperty": <Int32> 42,
            "StringProperty": <String> "Text"
        }
        """;
    private const string _testRecordCompactDump = """
        <TestRecord>{"IntProperty":<Int32>42,"StringProperty":<String>"Text"}
        """;
    private const string _testStructDump = """
        <TestStruct> {
            "IntProperty": <Int32> 42,
            "StringProperty": <String> "Text"
        }
        """;
    private const string _testStructCompactDump = """
        <TestStruct>{"IntProperty":<Int32>42,"StringProperty":<String>"Text"}
        """;
    private const string _testDictionaryDump = """
        <Dictionary<String, TestStruct>> [
            ["A"] = {
                "IntProperty": <Int32> 42,
                "StringProperty": <String> "Text"
            },
            ["B"] = {
                "IntProperty": <Int32> 7,
                "StringProperty": <String> "Other"
            }
        ]
        """;
    private const string _testDictionaryCompactDump = """
        <Dictionary<String, TestStruct>>[["A"]={"IntProperty":<Int32>42,"StringProperty":<String>"Text"},["B"]={"IntProperty":<Int32>7,"StringProperty":<String>"Other"}]
        """;
    private const string _cultureInfoDumpLvl0 = """
        <CultureInfo> ...
        """;
    private const string _cultureInfoDumpLvl1 = """
        <CultureInfo> {
            "Parent": <CultureInfo> ...,
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
            "CompareInfo": <CompareInfo> ...,
            "TextInfo": <TextInfo> ...,
            "IsNeutralCulture": <Boolean> false,
            "CultureTypes": <CultureTypes> SpecificCultures,*
            "NumberFormat": <NumberFormatInfo> ...,
            "DateTimeFormat": <DateTimeFormatInfo> ...,
            "Calendar": <Calendar> ...,
            "OptionalCalendars": <Calendar[]> ...,
            "UseUserOverride": <Boolean> false,
            "IsReadOnly": <Boolean> true
        }
        """;
    private const string _cultureInfoDumpLvl2 = """
        <CultureInfo> {
            "Parent": <CultureInfo> {
                "Parent": <CultureInfo> ...,
                "LCID": <Int32> 9,
                "KeyboardLayoutId": <Int32> 9,
                "Name": <String> "en",
                "IetfLanguageTag": <String> "en",
                "DisplayName": <String> "English",
                "NativeName": <String> "English",
                "EnglishName": <String> "English",
                "TwoLetterISOLanguageName": <String> "en",
                "ThreeLetterISOLanguageName": <String> "eng",
                "ThreeLetterWindowsLanguageName": <String> "ENU",
                "CompareInfo": <CompareInfo> ...,
                "TextInfo": <TextInfo> ...,
                "IsNeutralCulture": <Boolean> true,
                "CultureTypes": <CultureTypes> NeutralCultures,*
                "NumberFormat": <NumberFormatInfo> ...,
                "DateTimeFormat": <DateTimeFormatInfo> ...,
                "Calendar": <Calendar> ...,
                "OptionalCalendars": <Calendar[]> ...,
                "UseUserOverride": <Boolean> false,
                "IsReadOnly": <Boolean> false
            },
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
            "CompareInfo": <CompareInfo> {
                "Name": <String> "en-CA",
                "Version": <SortVersion> ...,
                "LCID": <Int32> 4105
            },
            "TextInfo": <TextInfo> {
                "ANSICodePage": <Int32> 1252,
                "OEMCodePage": <Int32> 850,
                "MacCodePage": <Int32> 10000,
                "EBCDICCodePage": <Int32> 37,
                "LCID": <Int32> 4105,
                "CultureName": <String> "en-CA",
                "IsReadOnly": <Boolean> true,
                "ListSeparator": <String> ",",
                "IsRightToLeft": <Boolean> false
            },
            "IsNeutralCulture": <Boolean> false,
            "CultureTypes": <CultureTypes> SpecificCultures,*
            "NumberFormat": <NumberFormatInfo> {
                "CurrencyDecimalDigits": <Int32> 2,
                "CurrencyDecimalSeparator": <String> ".",
                "IsReadOnly": <Boolean> true,
                "CurrencyGroupSizes": <Int32[]> ...,
                "NumberGroupSizes": <Int32[]> ...,
                "PercentGroupSizes": <Int32[]> ...,
                "CurrencyGroupSeparator": <String> ",",
                "CurrencySymbol": <String> "$",
                "NaNSymbol": <String> "NaN",
                "CurrencyNegativePattern": <Int32> 1,
                "NumberNegativePattern": <Int32> 1,
                "PercentPositivePattern": <Int32> 1,
                "PercentNegativePattern": <Int32> 1,
                "NegativeInfinitySymbol": <String> "-∞",
                "NegativeSign": <String> "-",
                "NumberDecimalDigits": <Int32> 3,
                "NumberDecimalSeparator": <String> ".",
                "NumberGroupSeparator": <String> ",",
                "CurrencyPositivePattern": <Int32> 0,
                "PositiveInfinitySymbol": <String> "∞",
                "PositiveSign": <String> "+",
                "PercentDecimalDigits": <Int32> 3,
                "PercentDecimalSeparator": <String> ".",
                "PercentGroupSeparator": <String> ",",
                "PercentSymbol": <String> "%",
                "PerMilleSymbol": <String> "‰",
                "NativeDigits": <String[]> ...,
                "DigitSubstitution": <DigitShapes> None
            },
            "DateTimeFormat": <DateTimeFormatInfo> {
                "AMDesignator": <String> "a.m.",
                "Calendar": <Calendar> ...,
                "DateSeparator": <String> "-",
                "FirstDayOfWeek": <DayOfWeek> Sunday,
                "CalendarWeekRule": <CalendarWeekRule> FirstDay,
                "FullDateTimePattern": <String> "dddd, MMMM d, yyyy h:mm:ss tt",
                "LongDatePattern": <String> "dddd, MMMM d, yyyy",
                "LongTimePattern": <String> "h:mm:ss tt",
                "MonthDayPattern": <String> "MMMM d",
                "PMDesignator": <String> "p.m.",
                "RFC1123Pattern": <String> "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
                "ShortDatePattern": <String> "yyyy-MM-dd",
                "ShortTimePattern": <String> "h:mm tt",
                "SortableDateTimePattern": <String> "yyyy'-'MM'-'dd'T'HH':'mm':'ss",
                "TimeSeparator": <String> ":",
                "UniversalSortableDateTimePattern": <String> "yyyy'-'MM'-'dd HH':'mm':'ss'Z'",
                "YearMonthPattern": <String> "MMMM yyyy",
                "AbbreviatedDayNames": <String[]> ...,
                "ShortestDayNames": <String[]> ...,
                "DayNames": <String[]> ...,
                "AbbreviatedMonthNames": <String[]> ...,
                "MonthNames": <String[]> ...,
                "IsReadOnly": <Boolean> true,
                "NativeCalendarName": <String> "Gregorian Calendar",
                "AbbreviatedMonthGenitiveNames": <String[]> ...,
                "MonthGenitiveNames": <String[]> ...
            },
            "Calendar": <Calendar> {
                "MinSupportedDateTime": <DateTime> 0001-01-01 00:00:00,
                "MaxSupportedDateTime": <DateTime> 9999-12-31 23:59:59,
                "AlgorithmType": <CalendarAlgorithmType> SolarCalendar,
                "CalendarType": <GregorianCalendarTypes> Localized,
                "Eras": <Int32[]> ...,
                "TwoDigitYearMax": <Int32> 2049,
                "IsReadOnly": <Boolean> true
            },
            "OptionalCalendars": <Calendar[]> [
                ...
            ],
            "UseUserOverride": <Boolean> false,
            "IsReadOnly": <Boolean> true
        }
        """;
    private const string _cultureInfoDumpLvl3 = """
        <CultureInfo> {
            "Parent": <CultureInfo> {
                "Parent": <CultureInfo> {
                    "LCID": <Int32> 127,
                    "KeyboardLayoutId": <Int32> 127,
                    "Name": <String> "",
                    "IetfLanguageTag": <String> "",
                    "DisplayName": <String> "Invariant Language (Invariant Country)",
                    "NativeName": <String> "Invariant Language (Invariant Country)",
                    "EnglishName": <String> "Invariant Language (Invariant Country)",
                    "TwoLetterISOLanguageName": <String> "iv",
                    "ThreeLetterISOLanguageName": <String> "ivl",
                    "ThreeLetterWindowsLanguageName": <String> "IVL",
                    "CompareInfo": <CompareInfo> ...,
                    "TextInfo": <TextInfo> ...,
                    "IsNeutralCulture": <Boolean> false,
                    "CultureTypes": <CultureTypes> SpecificCultures,*
                    "NumberFormat": <NumberFormatInfo> ...,
                    "DateTimeFormat": <DateTimeFormatInfo> ...,
                    "Calendar": <Calendar> ...,
                    "OptionalCalendars": <Calendar[]> ...,
                    "UseUserOverride": <Boolean> false,
                    "IsReadOnly": <Boolean> true
                },
                "LCID": <Int32> 9,
                "KeyboardLayoutId": <Int32> 9,
                "Name": <String> "en",
                "IetfLanguageTag": <String> "en",
                "DisplayName": <String> "English",
                "NativeName": <String> "English",
                "EnglishName": <String> "English",
                "TwoLetterISOLanguageName": <String> "en",
                "ThreeLetterISOLanguageName": <String> "eng",
                "ThreeLetterWindowsLanguageName": <String> "ENU",
                "CompareInfo": <CompareInfo> {
                    "Name": <String> "en",
                    "Version": <SortVersion> ...,
                    "LCID": <Int32> 9
                },
                "TextInfo": <TextInfo> {
                    "ANSICodePage": <Int32> 1252,
                    "OEMCodePage": <Int32> 437,
                    "MacCodePage": <Int32> 10000,
                    "EBCDICCodePage": <Int32> 37,
                    "LCID": <Int32> 9,
                    "CultureName": <String> "en",
                    "IsReadOnly": <Boolean> false,
                    "ListSeparator": <String> ",",
                    "IsRightToLeft": <Boolean> false
                },
                "IsNeutralCulture": <Boolean> true,
                "CultureTypes": <CultureTypes> NeutralCultures,*
                "NumberFormat": <NumberFormatInfo> {
                    "CurrencyDecimalDigits": <Int32> 2,
                    "CurrencyDecimalSeparator": <String> ".",
                    "IsReadOnly": <Boolean> false,
                    "CurrencyGroupSizes": <Int32[]> ...,
                    "NumberGroupSizes": <Int32[]> ...,
                    "PercentGroupSizes": <Int32[]> ...,
                    "CurrencyGroupSeparator": <String> ",",
                    "CurrencySymbol": <String> "¤",
                    "NaNSymbol": <String> "NaN",
                    "CurrencyNegativePattern": <Int32> 1,
                    "NumberNegativePattern": <Int32> 1,
                    "PercentPositivePattern": <Int32> 1,
                    "PercentNegativePattern": <Int32> 1,
                    "NegativeInfinitySymbol": <String> "-∞",
                    "NegativeSign": <String> "-",
                    "NumberDecimalDigits": <Int32> 3,
                    "NumberDecimalSeparator": <String> ".",
                    "NumberGroupSeparator": <String> ",",
                    "CurrencyPositivePattern": <Int32> 0,
                    "PositiveInfinitySymbol": <String> "∞",
                    "PositiveSign": <String> "+",
                    "PercentDecimalDigits": <Int32> 3,
                    "PercentDecimalSeparator": <String> ".",
                    "PercentGroupSeparator": <String> ",",
                    "PercentSymbol": <String> "%",
                    "PerMilleSymbol": <String> "‰",
                    "NativeDigits": <String[]> ...,
                    "DigitSubstitution": <DigitShapes> None
                },
                "DateTimeFormat": <DateTimeFormatInfo> {
                    "AMDesignator": <String> "AM",
                    "Calendar": <Calendar> ...,
                    "DateSeparator": <String> "/",
                    "FirstDayOfWeek": <DayOfWeek> Sunday,
                    "CalendarWeekRule": <CalendarWeekRule> FirstDay,
                    "FullDateTimePattern": <String> "dddd, MMMM d, yyyy h:mm:ss tt",
                    "LongDatePattern": <String> "dddd, MMMM d, yyyy",
                    "LongTimePattern": <String> "h:mm:ss tt",
                    "MonthDayPattern": <String> "MMMM d",
                    "PMDesignator": <String> "PM",
                    "RFC1123Pattern": <String> "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
                    "ShortDatePattern": <String> "M/d/yyyy",
                    "ShortTimePattern": <String> "h:mm tt",
                    "SortableDateTimePattern": <String> "yyyy'-'MM'-'dd'T'HH':'mm':'ss",
                    "TimeSeparator": <String> ":",
                    "UniversalSortableDateTimePattern": <String> "yyyy'-'MM'-'dd HH':'mm':'ss'Z'",
                    "YearMonthPattern": <String> "MMMM yyyy",
                    "AbbreviatedDayNames": <String[]> ...,
                    "ShortestDayNames": <String[]> ...,
                    "DayNames": <String[]> ...,
                    "AbbreviatedMonthNames": <String[]> ...,
                    "MonthNames": <String[]> ...,
                    "IsReadOnly": <Boolean> false,
                    "NativeCalendarName": <String> "Gregorian Calendar",
                    "AbbreviatedMonthGenitiveNames": <String[]> ...,
                    "MonthGenitiveNames": <String[]> ...
                },
                "Calendar": <Calendar> {
                    "MinSupportedDateTime": <DateTime> 0001-01-01 00:00:00,
                    "MaxSupportedDateTime": <DateTime> 9999-12-31 23:59:59,
                    "AlgorithmType": <CalendarAlgorithmType> SolarCalendar,
                    "CalendarType": <GregorianCalendarTypes> Localized,
                    "Eras": <Int32[]> ...,
                    "TwoDigitYearMax": <Int32> 2049,
                    "IsReadOnly": <Boolean> false
                },
                "OptionalCalendars": <Calendar[]> [
                    ...
                ],
                "UseUserOverride": <Boolean> false,
                "IsReadOnly": <Boolean> false
            },
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
            "CompareInfo": <CompareInfo> {
                "Name": <String> "en-CA",
                "Version": <SortVersion> {
                    "FullVersion": <Int32> 26777,
                    "SortId": <Guid> 00006899-0000-0000-0000-000000001009
                },
                "LCID": <Int32> 4105
            },
            "TextInfo": <TextInfo> {
                "ANSICodePage": <Int32> 1252,
                "OEMCodePage": <Int32> 850,
                "MacCodePage": <Int32> 10000,
                "EBCDICCodePage": <Int32> 37,
                "LCID": <Int32> 4105,
                "CultureName": <String> "en-CA",
                "IsReadOnly": <Boolean> true,
                "ListSeparator": <String> ",",
                "IsRightToLeft": <Boolean> false
            },
            "IsNeutralCulture": <Boolean> false,
            "CultureTypes": <CultureTypes> SpecificCultures,*
            "NumberFormat": <NumberFormatInfo> {
                "CurrencyDecimalDigits": <Int32> 2,
                "CurrencyDecimalSeparator": <String> ".",
                "IsReadOnly": <Boolean> true,
                "CurrencyGroupSizes": <Int32[]> [
                    3
                ],
                "NumberGroupSizes": <Int32[]> [
                    3
                ],
                "PercentGroupSizes": <Int32[]> [
                    3
                ],
                "CurrencyGroupSeparator": <String> ",",
                "CurrencySymbol": <String> "$",
                "NaNSymbol": <String> "NaN",
                "CurrencyNegativePattern": <Int32> 1,
                "NumberNegativePattern": <Int32> 1,
                "PercentPositivePattern": <Int32> 1,
                "PercentNegativePattern": <Int32> 1,
                "NegativeInfinitySymbol": <String> "-∞",
                "NegativeSign": <String> "-",
                "NumberDecimalDigits": <Int32> 3,
                "NumberDecimalSeparator": <String> ".",
                "NumberGroupSeparator": <String> ",",
                "CurrencyPositivePattern": <Int32> 0,
                "PositiveInfinitySymbol": <String> "∞",
                "PositiveSign": <String> "+",
                "PercentDecimalDigits": <Int32> 3,
                "PercentDecimalSeparator": <String> ".",
                "PercentGroupSeparator": <String> ",",
                "PercentSymbol": <String> "%",
                "PerMilleSymbol": <String> "‰",
                "NativeDigits": <String[]> [
                    "0",
                    "1",
                    "2",
                    "3",
                    "4",
                    "5",
                    "6",
                    "7",
                    "8",
                    "9"
                ],
                "DigitSubstitution": <DigitShapes> None
            },
            "DateTimeFormat": <DateTimeFormatInfo> {
                "AMDesignator": <String> "a.m.",
                "Calendar": <Calendar> {
                    "MinSupportedDateTime": <DateTime> 0001-01-01 00:00:00,
                    "MaxSupportedDateTime": <DateTime> 9999-12-31 23:59:59,
                    "AlgorithmType": <CalendarAlgorithmType> SolarCalendar,
                    "CalendarType": <GregorianCalendarTypes> Localized,
                    "Eras": <Int32[]> ...,
                    "TwoDigitYearMax": <Int32> 2049,
                    "IsReadOnly": <Boolean> true
                },
                "DateSeparator": <String> "-",
                "FirstDayOfWeek": <DayOfWeek> Sunday,
                "CalendarWeekRule": <CalendarWeekRule> FirstDay,
                "FullDateTimePattern": <String> "dddd, MMMM d, yyyy h:mm:ss tt",
                "LongDatePattern": <String> "dddd, MMMM d, yyyy",
                "LongTimePattern": <String> "h:mm:ss tt",
                "MonthDayPattern": <String> "MMMM d",
                "PMDesignator": <String> "p.m.",
                "RFC1123Pattern": <String> "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",
                "ShortDatePattern": <String> "yyyy-MM-dd",
                "ShortTimePattern": <String> "h:mm tt",
                "SortableDateTimePattern": <String> "yyyy'-'MM'-'dd'T'HH':'mm':'ss",
                "TimeSeparator": <String> ":",
                "UniversalSortableDateTimePattern": <String> "yyyy'-'MM'-'dd HH':'mm':'ss'Z'",
                "YearMonthPattern": <String> "MMMM yyyy",
                "AbbreviatedDayNames": <String[]> [
                    "Sun.",
                    "Mon.",
                    "Tue.",
                    "Wed.",
                    "Thu.",
                    "Fri.",
                    "Sat."
                ],
                "ShortestDayNames": <String[]> [
                    "S",
                    "M",
                    "T",
                    "W",
                    "T",
                    "F",
                    "S"
                ],
                "DayNames": <String[]> [
                    "Sunday",
                    "Monday",
                    "Tuesday",
                    "Wednesday",
                    "Thursday",
                    "Friday",
                    "Saturday"
                ],
                "AbbreviatedMonthNames": <String[]> [
                    "Jan.",
                    "Feb.",
                    "Mar.",
                    "Apr.",
                    "May",
                    "Jun.",
                    "Jul.",
                    "Aug.",
                    "Sep.",
                    "Oct.",
                    "Nov.",
                    "Dec.",
                    ""
                ],
                "MonthNames": <String[]> [
                    "January",
                    "February",
                    "March",
                    "April",
                    "May",
                    "June",
                    "July",
                    "August",
                    "September",
                    "October",
                    "November",
                    "December",
                    ""
                ],
                "IsReadOnly": <Boolean> true,
                "NativeCalendarName": <String> "Gregorian Calendar",
                "AbbreviatedMonthGenitiveNames": <String[]> [
                    "Jan.",
                    "Feb.",
                    "Mar.",
                    "Apr.",
                    "May",
                    "Jun.",
                    "Jul.",
                    "Aug.",
                    "Sep.",
                    "Oct.",
                    "Nov.",
                    "Dec.",
                    ""
                ],
                "MonthGenitiveNames": <String[]> [
                    "January",
                    "February",
                    "March",
                    "April",
                    "May",
                    "June",
                    "July",
                    "August",
                    "September",
                    "October",
                    "November",
                    "December",
                    ""
                ]
            },
            "Calendar": <Calendar> {
                "MinSupportedDateTime": <DateTime> 0001-01-01 00:00:00,
                "MaxSupportedDateTime": <DateTime> 9999-12-31 23:59:59,
                "AlgorithmType": <CalendarAlgorithmType> SolarCalendar,
                "CalendarType": <GregorianCalendarTypes> Localized,
                "Eras": <Int32[]> [
                    1
                ],
                "TwoDigitYearMax": <Int32> 2049,
                "IsReadOnly": <Boolean> true
            },
            "OptionalCalendars": <Calendar[]> [
                {
                    "MinSupportedDateTime": <DateTime> 0001-01-01 00:00:00,
                    "MaxSupportedDateTime": <DateTime> 9999-12-31 23:59:59,
                    "AlgorithmType": <CalendarAlgorithmType> SolarCalendar,
                    "CalendarType": <GregorianCalendarTypes> Localized,
                    "Eras": <Int32[]> ...,
                    "TwoDigitYearMax": <Int32> 2049,
                    "IsReadOnly": <Boolean> false
                }
            ],
            "UseUserOverride": <Boolean> false,
            "IsReadOnly": <Boolean> true
        }
        """;
    private const string _fullNamedTestClassDump = """
        <DotNetToolbox.ObjectExtensionsTests+TestClass> {
            "IntProperty": <System.Int32> 42,
            "StringProperty": <System.String> "Text"
        }
        """;
    private const string _customFormatterDump = """
        <TestClass> {
            "IntProperty": <Int32> 0,042.000,
            "StringProperty": <String> It is a string.
        }
        """;
    private const string _customClassTypeDump = """
        <RuntimeType> {
            "IsCollectible": <Boolean> false,
            "FullName": <String> "DotNetToolbox.ObjectExtensionsTests+CustomClass`1",
            "AssemblyQualifiedName": <String> "DotNetToolbox.ObjectExtensionsTests+CustomClass`1, DotNetToolbox.ObjectDumper.UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=414fc8c314317fa7",
            "Namespace": <String> "DotNetToolbox",
            "GUID": <Guid> bbb0d012-07fc-349f-b31e-9fe0a1c7281f,
            "IsEnum": <Boolean> false,
            "IsConstructedGenericType": <Boolean> false,
            "IsGenericType": <Boolean> true,
            "IsGenericTypeDefinition": <Boolean> true,
            "IsSZArray": <Boolean> false,
            "ContainsGenericParameters": <Boolean> true,
            "StructLayoutAttribute": <Attribute> StructLayoutAttribute,
            "IsFunctionPointer": <Boolean> false,
            "IsUnmanagedFunctionPointer": <Boolean> false,
            "Name": <String> "CustomClass`1",
            "DeclaringType": <Type> ObjectExtensionsTests,
            "Assembly": <Assembly> DotNetToolbox.ObjectDumper.UnitTests v1.0.0.0,
            "BaseType": <Type> Object,
            "IsByRefLike": <Boolean> false,
            "IsGenericParameter": <Boolean> false,
            "IsTypeDefinition": <Boolean> true,
            "IsSecurityCritical": <Boolean> true,
            "IsSecuritySafeCritical": <Boolean> false,
            "IsSecurityTransparent": <Boolean> false,
            "MemberType": <MemberTypes> NestedType,
            "MetadataToken": <Int32> 33554440,
            "ReflectedType": <Type> ObjectExtensionsTests,
            "GenericTypeParameters": <Type[]> [
                T
            ],
            "DeclaredConstructors": <IEnumerable<ConstructorInfo>> [
                <Constructor> .ctor(T value)
            ],
            "DeclaredEvents": <IEnumerable<EventInfo>> [
                <Event> ObjectExtensionsTests+CustomClass<T>+EventHandler<T, ConvertedArgs> OnConverted
            ],
            "DeclaredFields": <IEnumerable<FieldInfo>> [
                <Field> T Default,
                <Field> String Name
            ],
            "DeclaredMembers": <IEnumerable<MemberInfo>> [
                <Method> T get_Value(),
                <Method> TValue ConvertTo(Object obj),
                <Method> Void add_OnConverted(ObjectExtensionsTests+CustomClass<T>+EventHandler<T, ConvertedArgs> value),
                <Method> Void remove_OnConverted(ObjectExtensionsTests+CustomClass<T>+EventHandler<T, ConvertedArgs> value),
                <Constructor> .ctor(T value),
                <Property> T Value,
                <Event> ObjectExtensionsTests+CustomClass<T>+EventHandler<T, ConvertedArgs> OnConverted,
                <Field> T Default,
                <Field> String Name
            ],
            "DeclaredMethods": <IEnumerable<MethodInfo>> [
                <Method> T get_Value(),
                <Method> TValue ConvertTo(Object obj),
                <Method> Void add_OnConverted(ObjectExtensionsTests+CustomClass<T>+EventHandler<T, ConvertedArgs> value),
                <Method> Void remove_OnConverted(ObjectExtensionsTests+CustomClass<T>+EventHandler<T, ConvertedArgs> value)
            ],
            "DeclaredNestedTypes": <IEnumerable<TypeInfo>> [
            ],
            "DeclaredProperties": <IEnumerable<PropertyInfo>> [
                <Property> T Value
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
                NullableAttribute
            ]
        }
        """;
    private const string _fullNamedCustomClassTypeDump = """
        <System.RuntimeType> {
            "IsCollectible": <System.Boolean> false,
            "FullName": <System.String> "DotNetToolbox.ObjectExtensionsTests+CustomClass`1",
            "AssemblyQualifiedName": <System.String> "DotNetToolbox.ObjectExtensionsTests+CustomClass`1, DotNetToolbox.ObjectDumper.UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=414fc8c314317fa7",
            "Namespace": <System.String> "DotNetToolbox",
            "GUID": <System.Guid> bbb0d012-07fc-349f-b31e-9fe0a1c7281f,
            "IsEnum": <System.Boolean> false,
            "IsConstructedGenericType": <System.Boolean> false,
            "IsGenericType": <System.Boolean> true,
            "IsGenericTypeDefinition": <System.Boolean> true,
            "IsSZArray": <System.Boolean> false,
            "ContainsGenericParameters": <System.Boolean> true,
            "StructLayoutAttribute": <Attribute> System.Runtime.InteropServices.StructLayoutAttribute,
            "IsFunctionPointer": <System.Boolean> false,
            "IsUnmanagedFunctionPointer": <System.Boolean> false,
            "Name": <System.String> "CustomClass`1",
            "DeclaringType": <System.Type> DotNetToolbox.ObjectExtensionsTests,
            "Assembly": <System.Reflection.Assembly> DotNetToolbox.ObjectDumper.UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=414fc8c314317fa7,
            "BaseType": <System.Type> System.Object,
            "IsByRefLike": <System.Boolean> false,
            "IsGenericParameter": <System.Boolean> false,
            "IsTypeDefinition": <System.Boolean> true,
            "IsSecurityCritical": <System.Boolean> true,
            "IsSecuritySafeCritical": <System.Boolean> false,
            "IsSecurityTransparent": <System.Boolean> false,
            "MemberType": <System.Reflection.MemberTypes> NestedType,
            "MetadataToken": <System.Int32> 33554440,
            "ReflectedType": <System.Type> DotNetToolbox.ObjectExtensionsTests,
            "GenericTypeParameters": <System.Type[]> [
                T
            ],
            "DeclaredConstructors": <System.Collections.Generic.IEnumerable<System.Reflection.ConstructorInfo>> [
                <Constructor> .ctor(T value)
            ],
            "DeclaredEvents": <System.Collections.Generic.IEnumerable<System.Reflection.EventInfo>> [
                <Event> DotNetToolbox.ObjectExtensionsTests+DotNetToolbox.ObjectExtensionsTests+CustomClass<T>+EventHandler<T, ConvertedArgs> OnConverted
            ],
            "DeclaredFields": <System.Collections.Generic.IEnumerable<System.Reflection.FieldInfo>> [
                <Field> T Default,
                <Field> System.String Name
            ],
            "DeclaredMembers": <System.Collections.Generic.IEnumerable<System.Reflection.MemberInfo>> [
                <Method> T get_Value(),
                <Method> TValue ConvertTo(System.Object obj),
                <Method> System.Void add_OnConverted(DotNetToolbox.ObjectExtensionsTests+DotNetToolbox.ObjectExtensionsTests+CustomClass<T>+EventHandler<T, ConvertedArgs> value),
                <Method> System.Void remove_OnConverted(DotNetToolbox.ObjectExtensionsTests+DotNetToolbox.ObjectExtensionsTests+CustomClass<T>+EventHandler<T, ConvertedArgs> value),
                <Constructor> .ctor(T value),
                <Property> T Value,
                <Event> DotNetToolbox.ObjectExtensionsTests+DotNetToolbox.ObjectExtensionsTests+CustomClass<T>+EventHandler<T, ConvertedArgs> OnConverted,
                <Field> T Default,
                <Field> System.String Name
            ],
            "DeclaredMethods": <System.Collections.Generic.IEnumerable<System.Reflection.MethodInfo>> [
                <Method> T get_Value(),
                <Method> TValue ConvertTo(System.Object obj),
                <Method> System.Void add_OnConverted(DotNetToolbox.ObjectExtensionsTests+DotNetToolbox.ObjectExtensionsTests+CustomClass<T>+EventHandler<T, ConvertedArgs> value),
                <Method> System.Void remove_OnConverted(DotNetToolbox.ObjectExtensionsTests+DotNetToolbox.ObjectExtensionsTests+CustomClass<T>+EventHandler<T, ConvertedArgs> value)
            ],
            "DeclaredNestedTypes": <System.Collections.Generic.IEnumerable<System.Reflection.TypeInfo>> [
            ],
            "DeclaredProperties": <System.Collections.Generic.IEnumerable<System.Reflection.PropertyInfo>> [
                <Property> T Value
            ],
            "ImplementedInterfaces": <System.Collections.Generic.IEnumerable<System.Type>> [
            ],
            "IsInterface": <System.Boolean> false,
            "IsNested": <System.Boolean> true,
            "IsArray": <System.Boolean> false,
            "IsByRef": <System.Boolean> false,
            "IsPointer": <System.Boolean> false,
            "IsGenericTypeParameter": <System.Boolean> false,
            "IsGenericMethodParameter": <System.Boolean> false,
            "IsVariableBoundArray": <System.Boolean> false,
            "HasElementType": <System.Boolean> false,
            "GenericTypeArguments": <System.Type[]> [
            ],
            "Attributes": <System.Reflection.TypeAttributes> NestedPrivate, BeforeFieldInit,
            "IsAbstract": <System.Boolean> false,
            "IsImport": <System.Boolean> false,
            "IsSealed": <System.Boolean> false,
            "IsSpecialName": <System.Boolean> false,
            "IsClass": <System.Boolean> true,
            "IsNestedAssembly": <System.Boolean> false,
            "IsNestedFamANDAssem": <System.Boolean> false,
            "IsNestedFamily": <System.Boolean> false,
            "IsNestedFamORAssem": <System.Boolean> false,
            "IsNestedPrivate": <System.Boolean> true,
            "IsNestedPublic": <System.Boolean> false,
            "IsNotPublic": <System.Boolean> false,
            "IsPublic": <System.Boolean> false,
            "IsAutoLayout": <System.Boolean> true,
            "IsExplicitLayout": <System.Boolean> false,
            "IsLayoutSequential": <System.Boolean> false,
            "IsAnsiClass": <System.Boolean> true,
            "IsAutoClass": <System.Boolean> false,
            "IsUnicodeClass": <System.Boolean> false,
            "IsCOMObject": <System.Boolean> false,
            "IsContextful": <System.Boolean> false,
            "IsMarshalByRef": <System.Boolean> false,
            "IsPrimitive": <System.Boolean> false,
            "IsValueType": <System.Boolean> false,
            "IsSignatureType": <System.Boolean> false,
            "IsSerializable": <System.Boolean> false,
            "IsVisible": <System.Boolean> false,
            "CustomAttributes": <System.Collections.Generic.IEnumerable<System.Reflection.CustomAttributeData>> [
                System.Runtime.CompilerServices.NullableAttribute
            ]
        }
        """;
    private const string _integerTypeDump = """
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
            "Assembly": <Assembly> System.Private.CoreLib v8.0.0.0,
            "BaseType": <Type> ValueType,
            "IsByRefLike": <Boolean> false,
            "IsGenericParameter": <Boolean> false,
            "IsTypeDefinition": <Boolean> true,
            "IsSecurityCritical": <Boolean> true,
            "IsSecuritySafeCritical": <Boolean> false,
            "IsSecurityTransparent": <Boolean> false,
            "MemberType": <MemberTypes> TypeInfo,
            "MetadataToken": <Int32> 33554772,
            "ReflectedType": <Type> null,
            "GenericTypeParameters": <Type[]> [
            ],
            "DeclaredConstructors": <IEnumerable<ConstructorInfo>> [
            ],
            "DeclaredEvents": <IEnumerable<EventInfo>> [
            ],
            "DeclaredFields": <IEnumerable<FieldInfo>> [
                <Field> Int32 MaxValue,
                <Field> Int32 MinValue
            ],
            "DeclaredMembers": <IEnumerable<MemberInfo>> [
                <Method> Int32 CompareTo(Object value),
                <Method> Int32 CompareTo(Int32 value),
                <Method> Boolean Equals(Object obj),
                <Method> Boolean Equals(Int32 obj),
                <Method> Int32 GetHashCode(),
                <Method> String ToString(),
                <Method> String ToString(String format),
                <Method> String ToString(IFormatProvider provider),
                <Method> String ToString(String format, IFormatProvider provider),
                <Method> Boolean TryFormat(Span<Char> destination, Int32& charsWritten, ReadOnlySpan<Char> format, IFormatProvider provider),
                <Method> Boolean TryFormat(Span<Byte> utf8Destination, Int32& bytesWritten, ReadOnlySpan<Char> format, IFormatProvider provider),
                <Method> Int32 Parse(String s),
                <Method> Int32 Parse(String s, NumberStyles style),
                <Method> Int32 Parse(String s, IFormatProvider provider),
                <Method> Int32 Parse(String s, NumberStyles style, IFormatProvider provider),
                <Method> Int32 Parse(ReadOnlySpan<Char> s, NumberStyles style, IFormatProvider provider),
                <Method> Boolean TryParse(String s, Int32& result),
                <Method> Boolean TryParse(ReadOnlySpan<Char> s, Int32& result),
                <Method> Boolean TryParse(ReadOnlySpan<Byte> utf8Text, Int32& result),
                <Method> Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, Int32& result),
                <Method> Boolean TryParse(ReadOnlySpan<Char> s, NumberStyles style, IFormatProvider provider, Int32& result),
                <Method> TypeCode GetTypeCode(),
                <Method> ValueTuple<Int32, Int32> DivRem(Int32 left, Int32 right),
                <Method> Int32 LeadingZeroCount(Int32 value),
                <Method> Int32 PopCount(Int32 value),
                <Method> Int32 RotateLeft(Int32 value, Int32 rotateAmount),
                <Method> Int32 RotateRight(Int32 value, Int32 rotateAmount),
                <Method> Int32 TrailingZeroCount(Int32 value),
                <Method> Boolean IsPow2(Int32 value),
                <Method> Int32 Log2(Int32 value),
                <Method> Int32 Clamp(Int32 value, Int32 min, Int32 max),
                <Method> Int32 CopySign(Int32 value, Int32 sign),
                <Method> Int32 Max(Int32 x, Int32 y),
                <Method> Int32 Min(Int32 x, Int32 y),
                <Method> Int32 Sign(Int32 value),
                <Method> Int32 Abs(Int32 value),
                <Method> Int32 CreateChecked(TOther value),
                <Method> Int32 CreateSaturating(TOther value),
                <Method> Int32 CreateTruncating(TOther value),
                <Method> Boolean IsEvenInteger(Int32 value),
                <Method> Boolean IsNegative(Int32 value),
                <Method> Boolean IsOddInteger(Int32 value),
                <Method> Boolean IsPositive(Int32 value),
                <Method> Int32 MaxMagnitude(Int32 x, Int32 y),
                <Method> Int32 MinMagnitude(Int32 x, Int32 y),
                <Method> Boolean TryParse(String s, IFormatProvider provider, Int32& result),
                <Method> Int32 Parse(ReadOnlySpan<Char> s, IFormatProvider provider),
                <Method> Boolean TryParse(ReadOnlySpan<Char> s, IFormatProvider provider, Int32& result),
                <Method> Int32 Parse(ReadOnlySpan<Byte> utf8Text, NumberStyles style, IFormatProvider provider),
                <Method> Boolean TryParse(ReadOnlySpan<Byte> utf8Text, NumberStyles style, IFormatProvider provider, Int32& result),
                <Method> Int32 Parse(ReadOnlySpan<Byte> utf8Text, IFormatProvider provider),
                <Method> Boolean TryParse(ReadOnlySpan<Byte> utf8Text, IFormatProvider provider, Int32& result),
                <Field> Int32 MaxValue,
                <Field> Int32 MinValue
            ],
            "DeclaredMethods": <IEnumerable<MethodInfo>> [
                <Method> Int32 CompareTo(Object value),
                <Method> Int32 CompareTo(Int32 value),
                <Method> Boolean Equals(Object obj),
                <Method> Boolean Equals(Int32 obj),
                <Method> Int32 GetHashCode(),
                <Method> String ToString(),
                <Method> String ToString(String format),
                <Method> String ToString(IFormatProvider provider),
                <Method> String ToString(String format, IFormatProvider provider),
                <Method> Boolean TryFormat(Span<Char> destination, Int32& charsWritten, ReadOnlySpan<Char> format, IFormatProvider provider),
                <Method> Boolean TryFormat(Span<Byte> utf8Destination, Int32& bytesWritten, ReadOnlySpan<Char> format, IFormatProvider provider),
                <Method> Int32 Parse(String s),
                <Method> Int32 Parse(String s, NumberStyles style),
                <Method> Int32 Parse(String s, IFormatProvider provider),
                <Method> Int32 Parse(String s, NumberStyles style, IFormatProvider provider),
                <Method> Int32 Parse(ReadOnlySpan<Char> s, NumberStyles style, IFormatProvider provider),
                <Method> Boolean TryParse(String s, Int32& result),
                <Method> Boolean TryParse(ReadOnlySpan<Char> s, Int32& result),
                <Method> Boolean TryParse(ReadOnlySpan<Byte> utf8Text, Int32& result),
                <Method> Boolean TryParse(String s, NumberStyles style, IFormatProvider provider, Int32& result),
                <Method> Boolean TryParse(ReadOnlySpan<Char> s, NumberStyles style, IFormatProvider provider, Int32& result),
                <Method> TypeCode GetTypeCode(),
                <Method> ValueTuple<Int32, Int32> DivRem(Int32 left, Int32 right),
                <Method> Int32 LeadingZeroCount(Int32 value),
                <Method> Int32 PopCount(Int32 value),
                <Method> Int32 RotateLeft(Int32 value, Int32 rotateAmount),
                <Method> Int32 RotateRight(Int32 value, Int32 rotateAmount),
                <Method> Int32 TrailingZeroCount(Int32 value),
                <Method> Boolean IsPow2(Int32 value),
                <Method> Int32 Log2(Int32 value),
                <Method> Int32 Clamp(Int32 value, Int32 min, Int32 max),
                <Method> Int32 CopySign(Int32 value, Int32 sign),
                <Method> Int32 Max(Int32 x, Int32 y),
                <Method> Int32 Min(Int32 x, Int32 y),
                <Method> Int32 Sign(Int32 value),
                <Method> Int32 Abs(Int32 value),
                <Method> Int32 CreateChecked(TOther value),
                <Method> Int32 CreateSaturating(TOther value),
                <Method> Int32 CreateTruncating(TOther value),
                <Method> Boolean IsEvenInteger(Int32 value),
                <Method> Boolean IsNegative(Int32 value),
                <Method> Boolean IsOddInteger(Int32 value),
                <Method> Boolean IsPositive(Int32 value),
                <Method> Int32 MaxMagnitude(Int32 x, Int32 y),
                <Method> Int32 MinMagnitude(Int32 x, Int32 y),
                <Method> Boolean TryParse(String s, IFormatProvider provider, Int32& result),
                <Method> Int32 Parse(ReadOnlySpan<Char> s, IFormatProvider provider),
                <Method> Boolean TryParse(ReadOnlySpan<Char> s, IFormatProvider provider, Int32& result),
                <Method> Int32 Parse(ReadOnlySpan<Byte> utf8Text, NumberStyles style, IFormatProvider provider),
                <Method> Boolean TryParse(ReadOnlySpan<Byte> utf8Text, NumberStyles style, IFormatProvider provider, Int32& result),
                <Method> Int32 Parse(ReadOnlySpan<Byte> utf8Text, IFormatProvider provider),
                <Method> Boolean TryParse(ReadOnlySpan<Byte> utf8Text, IFormatProvider provider, Int32& result)
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
                ISignedNumber<Int32>
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
                SerializableAttribute,
                IsReadOnlyAttribute,
                TypeForwardedFromAttribute
            ]
        }
        """;

    #endregion
}
