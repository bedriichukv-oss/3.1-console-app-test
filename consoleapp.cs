// Sample usage:
// 1) Create a test list of numbers.
// 2) Sum only the prime values in that list.
// 3) Print the resulting sum to the console.
List<int> numbers = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };    
int sumOfPrimes = Program.AddPrimeNumbersInNumericList(numbers);
Console.WriteLine($"Sum of prime numbers: {sumOfPrimes}");

partial class Program
{
    // Adds together only the prime values found in the provided number list.
    // Throws ArgumentNullException when the input list is null.
    internal static int AddPrimeNumbersInNumericList(List<int> numbers)
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

    // Returns true when the input is a prime number; otherwise false.
    // Uses trial division up to the square root of the input for efficiency.
    internal static bool IsPrime(int number)
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
}