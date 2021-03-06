using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;

namespace DataProcessor
{
    class RomanTypeConverter : ITypeConverter
    {
        public object ConvertFromSting(string text, IReader row, MemberMapData memberMapData)
        {
            if (text == "I")
                return 1;

            if (text == "I")
                return 2;

            if (text == "V")
                return 5;

            throw new ArgumentOutOfRangeException(nameof(text));

        }

        public object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            throw new NotImplementedException();
        }

        

        public string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
        {
            throw new NotImplementedException();
        }
    }
}
