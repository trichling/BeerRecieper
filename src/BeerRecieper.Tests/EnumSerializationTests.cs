using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BeerRecieper.Tests;

public enum TestStatus
{
    Value1,
    Value2,
}

public class TestClass
{
    public required TestStatus EnumProperty { get; set; }
}

public class EnumWrapperJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        if (!typeToConvert.IsGenericType)
            return false;

        return typeToConvert.GetGenericTypeDefinition() == typeof(EnumWrapper<>);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type enumType = typeToConvert.GetGenericArguments()[0];
        Type converterType = typeof(EnumWrapperJsonConverterInner<>).MakeGenericType(enumType);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }

    private class EnumWrapperJsonConverterInner<T> : JsonConverter<EnumWrapper<T>>
        where T : struct, Enum
    {
        public override EnumWrapper<T>? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException(
                    $"Expected string value for enum wrapper, got {reader.TokenType}"
                );
            }

            var value = reader.GetString() ?? throw new JsonException("Enum value cannot be null");
            return new EnumWrapper<T> { Value = value };
        }

        public override void Write(
            Utf8JsonWriter writer,
            EnumWrapper<T> value,
            JsonSerializerOptions options
        )
        {
            writer.WriteStringValue(value.Value);
        }
    }
}

[JsonConverter(typeof(EnumWrapperJsonConverterFactory))]
public class EnumWrapper<T>
    where T : struct, Enum
{
    public required string Value { get; set; }

    public bool TryParse(out T value) => Enum.TryParse<T>(Value, out value);

    public static implicit operator EnumWrapper<T>(T value)
    {
        return new EnumWrapper<T> { Value = value.ToString() };
    }

    public static implicit operator T(EnumWrapper<T> wrapper)
    {
        if (!wrapper.TryParse(out var value))
        {
            throw new JsonException($"Invalid enum value: {wrapper.Value}");
        }
        return value;
    }
}

public class TestClassWithEnumWrapper
{
    public required EnumWrapper<TestStatus> EnumProperty { get; set; }
}

[TestClass]
public class EnumSerializationTests
{
    [TestMethod]
    public void When_DeserializingDefinedEmunValue_ShouldWork()
    {
        // Arrange
        var json = """{"EnumProperty": "Value2"}""";

        // Act
        var result = JsonSerializer.Deserialize<TestClassWithEnumWrapper>(json);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.EnumProperty);
        Assert.AreEqual("Value2", result.EnumProperty.Value);
    }

    [TestMethod]
    public void When_DeserializingUndefinedEnumValue_Should_ThrowException()
    {
        // Arrange
        var json = """{"EnumProperty": "Value3"}""";

        // Act & Assert
        Assert.ThrowsException<JsonException>(() => JsonSerializer.Deserialize<TestClass>(json));
    }

    [TestMethod]
    public void When_DeserializingUndefinedEnumValueToEnumWrapper_Should_Work()
    {
        // Arrange
        var json = """{"EnumProperty": "Value3"}""";

        // Act
        var result = JsonSerializer.Deserialize<TestClassWithEnumWrapper>(json);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.EnumProperty);
        Assert.AreEqual("Value3", result.EnumProperty.Value);
        Assert.ThrowsException<JsonException>(() =>
        {
            _ = (TestStatus)result.EnumProperty;
        });
    }

    [TestMethod]
    public void When_DeserializingDefinedEnumValueToEnumWrapper_Should_Work()
    {
        // Arrange
        var json = """{"EnumProperty": "Value1"}""";

        // Act
        var result = JsonSerializer.Deserialize<TestClassWithEnumWrapper>(json);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.EnumProperty);
        TestStatus enumValue = result.EnumProperty;
        Assert.AreEqual(TestStatus.Value1, enumValue);
    }

    [TestMethod]
    public void When_SerializingEnumWrapper_Should_WriteAsString()
    {
        // Arrange
        var wrapper = new TestClassWithEnumWrapper { EnumProperty = TestStatus.Value1 };

        // Act
        var json = JsonSerializer.Serialize(wrapper);

        // Assert
        Assert.AreEqual("""{"EnumProperty":"Value1"}""", json);
    }
}
