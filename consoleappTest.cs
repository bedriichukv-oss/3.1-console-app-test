using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

public class ConsoleAppTests
{
    private static readonly Type ProgramType = FindProgramType();

    private static Type FindProgramType()
    {
        Type? type = AppDomain.CurrentDomain
            .GetAssemblies()
            .Select(a => a.GetType("Program", throwOnError: false, ignoreCase: false))
            .FirstOrDefault(t => t is not null);

        return type ?? throw new InvalidOperationException(
            "Could not find generated top-level Program type. Ensure the app assembly is referenced by the test project.");
    }

    private static MethodInfo FindMethod(string name, params Type[] parameterTypes)
    {
        MethodInfo? method = ProgramType.GetMethod(
            name,
            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
            binder: null,
            types: parameterTypes,
            modifiers: null);

        return method ?? throw new MissingMethodException(
            $"Method '{name}({string.Join(", ", parameterTypes.Select(t => t.Name))})' was not found on Program.");
    }

    private static int InvokeAddPrimeNumbersInNumericList(List<int> numbers)
    {
        MethodInfo method = FindMethod("AddPrimeNumbersInNumericList", typeof(List<int>));

        try
        {
            object? result = method.Invoke(null, new object?[] { numbers });
            return (int)result!;
        }
        catch (TargetInvocationException ex) when (ex.InnerException is not null)
        {
            throw ex.InnerException;
        }
    }

    private static bool InvokeIsPrime(int number)
    {
        MethodInfo method = FindMethod("IsPrime", typeof(int));

        try
        {
            object? result = method.Invoke(null, new object?[] { number });
            return (bool)result!;
        }
        catch (TargetInvocationException ex) when (ex.InnerException is not null)
        {
            throw ex.InnerException;
        }
    }

    [Fact]
    public void AddPrimeNumbersInNumericList_NullInput_ThrowsArgumentNullException()
    {
        MethodInfo method = FindMethod("AddPrimeNumbersInNumericList", typeof(List<int>));

        var ex = Assert.Throws<TargetInvocationException>(() => method.Invoke(null, new object?[] { null }));
        Assert.NotNull(ex.InnerException);
        var argEx = Assert.IsType<ArgumentNullException>(ex.InnerException);
        Assert.Equal("numbers", argEx.ParamName);
    }

    [Fact]
    public void AddPrimeNumbersInNumericList_EmptyList_ReturnsZero()
    {
        int result = InvokeAddPrimeNumbersInNumericList(new List<int>());
        Assert.Equal(0, result);
    }

    [Fact]
    public void AddPrimeNumbersInNumericList_NoPrimeValues_ReturnsZero()
    {
        var input = new List<int> { -10, -2, -1, 0, 1, 4, 6, 8, 9, 10, 12 };
        int result = InvokeAddPrimeNumbersInNumericList(input);
        Assert.Equal(0, result);
    }

    [Fact]
    public void AddPrimeNumbersInNumericList_MixedValues_ReturnsExpectedPrimeSum()
    {
        var input = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        int result = InvokeAddPrimeNumbersInNumericList(input);
        Assert.Equal(17, result);
    }

    [Fact]
    public void AddPrimeNumbersInNumericList_NegativesAndDuplicates_ReturnsExpectedPrimeSum()
    {
        var input = new List<int> { -3, -2, -1, 0, 1, 2, 2, 3, 4, 5, 9, 11 };
        int result = InvokeAddPrimeNumbersInNumericList(input);
        Assert.Equal(23, result);
    }

    [Theory]
    [InlineData(-100)]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    public void IsPrime_ValuesLessThanOrEqualToOne_ReturnFalse(int value)
    {
        Assert.False(InvokeIsPrime(value));
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(7)]
    [InlineData(11)]
    [InlineData(13)]
    [InlineData(97)]
    public void IsPrime_PrimeNumbers_ReturnTrue(int value)
    {
        Assert.True(InvokeIsPrime(value));
    }

    [Theory]
    [InlineData(4)]
    [InlineData(6)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    [InlineData(12)]
    [InlineData(15)]
    [InlineData(21)]
    [InlineData(49)]
    public void IsPrime_CompositeNumbers_ReturnFalse(int value)
    {
        Assert.False(InvokeIsPrime(value));
    }
}
