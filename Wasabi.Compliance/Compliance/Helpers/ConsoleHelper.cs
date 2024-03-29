﻿

using Compliance.Helpers;
using Compliance.Models;

namespace Compliance
{
    internal class ConsoleHelper
    {

        public void WelcomeMessage()
        {
            Console.WriteLine("Wasabi Compliance Editor");
            PrintSeparator();
        }



        internal string RenameBucket()
        {
            var result = ReadParameter("Bucket name");
            PrintSeparator();
            return result;
        }

        public S3Configuration BuildS3Configuration()
        {
            Console.WriteLine();
            Console.WriteLine("Configure your account");

            var accessKeyId = ReadParameter("Access key id", message: "    Enter your access key id");
            var secretkey = ReadParameter("Secret Key", message: "    Enter your secret key");
            var bucketName = ReadParameter("Bucket", message: "    Enter the name of your bucket");
            var regionName = ReadRegion();

            PrintSeparator();
            return new()
            {
                AccessKeyId = accessKeyId,
                SecretKey = secretkey,
                BucketName = bucketName,
                Region = regionName
            };
        }

        internal static bool AskCustomer(string v)
        {
            Console.Write(v);
            var userInput = Console.ReadLine();

            return (!string.IsNullOrEmpty(userInput) && (userInput.ToUpper().Trim().Equals("Y") || userInput.ToUpper().Trim().Equals("YES")));
        }

        private string ReadParameter(string paramName, bool allowEmpty = false, string message = "")
        {
            var temp = "";
            do
            {
                if (string.IsNullOrEmpty(message))
                {
                    message = paramName;
                }
                Console.Write($"{message}: ");
                temp = Console.ReadLine();

                if (allowEmpty)
                {
                    return temp;
                }

                if (string.IsNullOrEmpty(temp))
                {
                    Error($"Error: Invalid {paramName} | Please enter a valid value");
                }

            } while (string.IsNullOrEmpty(temp));

            return temp;
        }

        private string ReadRegion()
        {
            var tempRegion = "";
            do
            {
                Console.Write($"    Enter the region of your bucket (default is [us-east-1]): ");
                tempRegion = Console.ReadLine();


                if (string.IsNullOrEmpty(tempRegion))
                {
                    tempRegion = "us-east-1";
                }
                else
                {
                    tempRegion = tempRegion.ToLower().Trim();

                    if (RegionsHelpers.Regions.Any() && !RegionsHelpers.Regions.Contains(tempRegion))
                    {
                        Error($"region [{tempRegion}] is not valid, please enter one of the following:\n({string.Join(", ", RegionsHelpers.Regions)})");
                        tempRegion = "";
                    }
                }

            } while (string.IsNullOrEmpty(tempRegion));

            return tempRegion;
        }


        private void PrintSeparator()
        {
            Console.WriteLine("");
        }

        private void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("    "+message);
            Console.ResetColor();
        }


        internal class TableHelper
        {
            static int tableWidth = 73;

            internal static void PrintLine()
            {
                Console.WriteLine(new string('-', tableWidth));
            }

            internal static void PrintRow(params string[] columns)
            {
                int width = (tableWidth - columns.Length) / columns.Length;
                string row = "|";

                foreach (string column in columns)
                {
                    row += AlignCentre(column, width) + "|";
                }

                Console.WriteLine(row);
            }

            internal static string AlignCentre(string text, int width)
            {
                text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

                if (string.IsNullOrEmpty(text))
                {
                    return new string(' ', width);
                }
                else
                {
                    return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
                }
            }


        }
    }
}
