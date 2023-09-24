using System.Diagnostics;

string sample = "abcdefjhijklmnop";
//string strSample2 = "gggfcbbb";

Anagramm anagramm = new Anagramm(sample);
Random rnd = new Random();
string random;

bool isAnagramm;

Stopwatch sw;
double timeClassic = 0;
double timeNL = 0;

for (int i = 0; i < 1000; i++)
{
    random = GenerateString(sample);

    // Nonlinear algorythm check
    sw = Stopwatch.StartNew();
    anagramm.IsAnagrammNonlinear(random);
    //Anagramm.IsAnagrammNonlinearUnsafe(sample, random);
    //Anagramm.IsAnagrammLINQ(sample, random);
    sw.Stop();
    // Average nonlinear time
    timeNL = (timeNL * i + sw.Elapsed.Ticks) / (i + 1);

    // Classic algorythm check
    sw = Stopwatch.StartNew();
    isAnagramm = anagramm.IsAnagrammClassic(random);
    sw.Stop();
    // Average classic time
    timeClassic = (timeClassic * i + sw.Elapsed.Ticks) / (i + 1);
        
    if (isAnagramm)
    {
        Console.WriteLine("\"{0}\" is anagramm of \"{1}\"", random, sample);
    }
}

Console.WriteLine("Classic anagramm detection avg time = {0:f02}", timeClassic);
Console.WriteLine("Nonlinear anagramm detection avg time = {0:f02}", timeNL);

/// <summary>
/// This method generates a string of the same Length as the samlpe
/// String may or may not be Anagramm of sample string
/// </summary>
string GenerateString(string sample)
{
    char[] chars = new char[sample.Length];
    
    if (rnd.Next(10) == 1)
    {
        // Generates Anagramm
        var bytes = sample.ToList();
        int pos;

        for (int i = 0; i < sample.Length; i++)
        {
            pos = rnd.Next(bytes.Count);

            chars[i] = bytes[pos];
            bytes.RemoveAt(pos);
        }
    }
    else
    {
        // Generates random string
        for (int i = 0; i < sample.Length; i++)
            chars[i] = (char)rnd.Next('A', 'w'); ;
    }

    return new string(chars);
}