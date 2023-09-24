namespace AnagrammTest.Test
{
    /// <summary>
    /// This class generates all combinations of arrays with
    /// the sum of elements equal to the sum of codes of a given string
    /// </summary>
    class FindEqualSumStrings
    {
        private string sample;
        private ProcessEqual? process = null;
        private char maxNumber;
        private char[] arrEqual;

        public delegate bool ProcessEqual(char[] arr);

        public FindEqualSumStrings(string sample, char maxNumber = char.MaxValue)
        {
            this.sample = sample;
            arrEqual = new char[sample.Length];
            this.maxNumber = maxNumber;
        }

        public bool Start(ProcessEqual process)
        {
            this.process = process;

            long sum = sample.Sum(b => b);
            bool ret = Arrange(0, maxNumber, sum);
            Shift(0);

            return ret;
        }

        private void Shift(int row)
        {
            if (arrEqual[row] < 2 || row >= arrEqual.Length - 1)
            {
                return;
            }

            int backWard = 0;

            do
            {
                if ((backWard > 0 && arrEqual[row] > arrEqual[row + 1]) ||
                    arrEqual[row + 1] == 0)
                {
                    long sum = arrEqual.Where((c, i) => i >= row).Sum(b => b);

                    if (!Arrange(row, arrEqual[row] - 1, sum))
                    {
                        break;
                    }

                    process?.Invoke(arrEqual);
                }

                backWard++;

                Shift(row + 1);
            }
            while (arrEqual[row] > arrEqual[row + 1]);
        }

        private bool Arrange(int row, int maxNumber, long sum)
        {
            int fullRows = (int)(sum / maxNumber);
            int mod = (int)(sum % maxNumber);

            if (row + fullRows + (mod > 0 ? 1 : 0) > arrEqual.Length)
            {
                return false;
            }

            Array.Fill(arrEqual, (char)maxNumber, row, fullRows);

            int pos = row + fullRows;
            if (pos < arrEqual.Length)
            {
                arrEqual[pos++] = (char)mod;

                if (pos < arrEqual.Length)
                    Array.Fill(arrEqual, (char)0, pos, arrEqual.Length - pos);
            }

            return true;
        }
    }
}
