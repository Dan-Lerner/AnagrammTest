public class Anagramm
{
    // Degree of nonlinear shift
    // The "4" has tested for ASCII strings
    private const byte degree = 3;

    private string sample;
    private long[] sum = new long[degree];

    Dictionary<char, int> StatisticSample = new();
    Dictionary<char, int> Statistic = new();

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="sample">Sample string to test</param>
    public Anagramm(string sample)
    {
        this.sample = sample;

        foreach (char c in sample)
            if (!StatisticSample.ContainsKey(c))
                StatisticSample[c] = 1;
            else
                StatisticSample[c]++;
    }

    /// <summary>
    /// Classic anagramm detection
    /// </summary>
    /// <param name="anagramm">String to conpare with Sample string</param>
    /// <returns>true if anagramm is anagramm of sample</returns>
    public bool IsAnagrammClassic(string anagramm)
    {
        if (sample.Length != anagramm.Length)
            return false;

        Statistic.Clear();
        foreach (var pair in StatisticSample)
            Statistic[pair.Key] = pair.Value;

        foreach (char c in anagramm)
            if (!Statistic.ContainsKey(c) || --Statistic[c] < 0)
                return false;

        foreach (var pair in Statistic)
            if (pair.Value != 0)
                return false;

        return true;
    }

    /// <summary>
    /// Nonlinear algorithm for anagramm detection
    /// ~2 times faster than classic
    /// </summary>
    /// <param name="anagramm">String to conpare with Sample string</param>
    /// <returns>true if anagramm is anagramm of sample</returns>
    public bool IsAnagrammNonlinear(string anagramm)
    {
        if (sample.Length != anagramm.Length)
            return false;

        long sum = 0;

        for (int i = 0; i < sample.Length; i++)
            sum += sample[i] - anagramm[i];

        if (sum != 0)
            return false;

        for (int i = 0; i < sample.Length; i++)
            sum += sample[i] * sample[i] * sample[i] * sample[i] -
                anagramm[i] * anagramm[i] * anagramm[i] * anagramm[i];

        return sum == 0;
    }

    /// <summary>
    /// Standalone function with Nonlinear algorithm for anagramm detection 
    /// without using the heap (unsafe mode)
    /// ~2-3 times faster than classic
    /// </summary>
    /// <param name="sample">Sample string to test</param>
    /// <param name="anagramm">String to conpare with Sample string</param>
    /// <returns>true if anagramm is anagramm of sample</returns>
    public static bool IsAnagrammNonlinearUnsafe(string sample, string anagramm)
    {
        if (sample.Length != anagramm.Length)
            return false;

        long prod1;
        long prod2;

        unsafe
        {
            long* pSum = stackalloc long[degree];
            for (int i = 0; i < degree; i++)
                pSum[i] = 0;

            for (int i = 0; i < sample.Length; i++)
            {
                prod1 = 1;
                prod2 = 1;

                for (int j = 0; j < degree; j++)
                {
                    prod1 *= sample[i];
                    prod2 *= anagramm[i];

                    pSum[j] += prod1 - prod2;
                }
            }

            for (int i = 0; i < degree; i++)
                if (pSum[i] != 0)
                    return false;
        }

        return true;
    }

    /// <summary>
    /// Low performance but contains just 1 line
    /// ~2 times slower than classic
    /// </summary>
    /// <param name="sample">Sample string to test</param>
    /// <param name="anagramm">String to conpare with Sample string</param>
    /// <returns>true if anagramm is anagramm of sample</returns>
    public static bool IsAnagrammLINQ(string sample, string anagramm)
    {
        return sample.Sum(p => p) == anagramm.Sum(p => p) && !sample.Except(anagramm).Any();
    }
}