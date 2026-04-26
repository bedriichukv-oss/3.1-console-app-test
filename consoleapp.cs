int AddPrimeNumbersInNumericList(List<int> numbers)
{
    if (numbers is null)
    {
        throw new ArgumentNullException(nameof(numbers));
    }

    int sum = 0;
    foreach (int number in numbers)
    {
        if (IsPrime(number))
        {
            sum += number;
        }
    }
    return sum;
}

bool IsPrime(int number)
{
    if (number <= 1)
    {
        return false;
    }

    int limit = (int)Math.Sqrt(number);
    for (int i = 2; i <= limit; i++)
    {
        if (number % i == 0) return false;
    }
    return true;
}

// create a list of numbers for testing
List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };    
int sumOfPrimes = AddPrimeNumbersInNumericList(numbers);
Console.WriteLine($"Sum of prime numbers: {sumOfPrimes}");