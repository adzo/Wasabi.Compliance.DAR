//// See https://aka.ms/new-console-template for more information
//using Compliance;
//using Compliance.Helpers;

//start:
//Console.WriteLine("Welcome. Use this script to update the delete after retention on your bucket:");

//await RegionsHelpers.LoadRegionsAsync();

//var consoleHelper = new ConsoleHelper();


//var configuration = consoleHelper.BuildS3Configuration();

//Console.WriteLine();
//try
//{
//    //getting the compliance of the bucket: 
//    Console.WriteLine($"Reading the configuration of your bucket {configuration.BucketName} and ensuring compliance is enabled...");
//    Console.WriteLine();
//    var bucketConfiguration = await ComplianceHelper.LoadBucketConfigurationAsync(configuration);

//    if (bucketConfiguration.Status.Equals("disabled"))
//    {
//        Console.WriteLine("Compliance in this bucket is disabled!");
//    }
//    else
//    {
//        if(bucketConfiguration.IsLocked)


//        //Console.WriteLine("Compliance in this bucket is enabled.");
//        if (bucketConfiguration.DeleteAfterRetention)
//        {
//            Console.WriteLine($"Delete after retention is enabled on the bucket {configuration.BucketName}.");

//            if (ConsoleHelper.AskCustomer("Do you want to disable the delete after retention? [Y/N]:"))
//            {
//                await ComplianceHelper.DisableDeleteAfterRetention(configuration);
//                Console.WriteLine($"  => Delete after retention was successfully disabled for you bucket {configuration.BucketName}");
//            }
//        }
//        else
//        {
//            Console.WriteLine($"Delete after retention is disabled on the bucket {configuration.BucketName}.");

//            if (ConsoleHelper.AskCustomer("Do you want to enable the delete after retention? [Y/N]:"))
//            {
//                await ComplianceHelper.EnableDeleteAfterRetention(configuration);
//                Console.WriteLine($"  => Delete after retention was successfully enabled for you bucket {configuration.BucketName}");
//            }
//        }
//    }

//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.Message);
//}

//Console.WriteLine();
//Console.Write("Run the script again [Y/N]:");
//var userInput = Console.ReadLine();

//if (!string.IsNullOrEmpty(userInput) && (userInput.ToUpper().Trim().Equals("Y") || userInput.ToUpper().Trim().Equals("YES")))
//{
//    Console.Clear();
//    goto start;
//}




