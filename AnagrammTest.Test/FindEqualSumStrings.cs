namespace AnagrammTest.Test
{
    /// <summary>
    /// This class generates all combinations of arrays with
    /// the sum of elements equal to the sum of codes of a given string
    /// </summary>
    class FindEqualSumStrings
    {
        private string _sample;
        private ProcessEqual? _process = null;
        private char _maxNumber;
        private char _minNumber;
        private char[] _arrArrange;
        private char[] _arrEqual;

        public delegate bool ProcessEqual(char[] arr);

        public FindEqualSumStrings(string sample, char maxNumber = char.MaxValue, char minNumber = (char)0)
        {
            this._sample = sample;
            _arrArrange = new char[sample.Length];
            _arrEqual = new char[sample.Length];
            this._maxNumber = maxNumber + 1 > char.MaxValue ? char.MaxValue : (char)(maxNumber + 1);
            this._minNumber = minNumber;
        }

        public bool Start(ProcessEqual process)
        {
            if (_sample.Where(b => b - _minNumber < 0).Any())
                return false;

            this._process = process;

            long sum = _sample.Sum(b => b) - _minNumber * _sample.Length;
            bool ret = Arrange(0, (char)(_maxNumber - _minNumber), sum);
            Shift(0);

            return ret;
        }

        private void Shift(int row)
        {
            if (_arrArrange[row] < 2 || row >= _arrArrange.Length - 1)
                return;

            int backWard = 0;
            long sum;

            do
            {
                if ((backWard > 0 && _arrArrange[row] > _arrArrange[row + 1]) ||
                    _arrArrange[row + 1] == 0)
                {
                    sum = 0;
                    for (int i = row; i < _arrArrange.Length; i++)
                        sum += _arrArrange[i];

                    if (!Arrange(row, _arrArrange[row] - 1, sum))
                        break;

                    if (_process is not null)
                    {
                        for (int i = 0; i < _arrArrange.Length; i++)
                            _arrEqual[i] = (char)(_arrArrange[i] + _minNumber);

                        _process.Invoke(_arrEqual);
                    }
                }

                backWard++;

                Shift(row + 1);
            }
            while (_arrArrange[row] > _arrArrange[row + 1]);
        }

        private bool Arrange(int row, int maxNumber, long sum)
        {
            int fullRows = (int)(sum / maxNumber);
            int mod = (int)(sum % maxNumber);

            if (row + fullRows + (mod > 0 ? 1 : 0) > _arrArrange.Length)
                return false;

            int pos = row;
            for (; pos < row + fullRows; pos++)
                _arrArrange[pos] = (char)maxNumber;

            if (pos < _arrArrange.Length)
            {
                _arrArrange[pos++] = (char)mod;

                for (; pos < _arrArrange.Length; pos++)
                    _arrArrange[pos] = (char)0;
            }

            return true;
        }
    }
}
