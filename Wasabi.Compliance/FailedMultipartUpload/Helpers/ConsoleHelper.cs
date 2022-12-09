using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FailedMultipartUpload.Helpers
{
    internal class ConsoleHelper
    {
        internal static T ReadInput<T>(string message, bool allowEmpty = false, T defaultValue = default)
        {
            Console.WriteLine(message);

            string? tempValue;

            T result = defaultValue;

            do
            {
                tempValue = Console.ReadLine();

                if (allowEmpty)
                {
                    return result;
                }

                if (string.IsNullOrEmpty(tempValue))
                {
                    Error($"Error: {tempValue} Invalid | value should not be empty");
                }
                else
                {
                    try
                    {
                        result = (T)Convert.ChangeType(tempValue, typeof(T)); 
                    }
                    catch (Exception)
                    {
                        Error($"Error: {tempValue} Invalid | Invqlid input");
                    } 
                }


            } while (string.IsNullOrEmpty(tempValue));

            return result;
        }

        static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("    " + message);
            Console.ResetColor();
        }
    }
}
