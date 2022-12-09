
//var chars = "-\\|/".ToCharArray();

//var totalLength = 50;

//var j = 0;
//for (int i = 0; i < 101; i++)
//{
//    var percentageLength = (i * totalLength) / 100;
//    await Task.Delay(100);
//    Console.Write($"\rProgress [{chars[j]}]: {new string('|', percentageLength).PadRight(totalLength,'-')} {i}%");
//    j++;
//    if (j == 4) j = 0;
//}
start:
Console.Write("Set total counts: ");
int totalNumber = Int32.Parse( Console.ReadLine());

var totalProgress = 1240;
var consoleProgressPrinter = new ConsoleProgressPrinter(totalNumber);
for (int i = 0; i < totalNumber; i++)
{ 
    await Task.Delay(100); 
    consoleProgressPrinter.ReportProgress();
    //Console.Write($"\rProgress [{chars[j]}]: {new string('|', percentageLength).PadRight(totalLength, '-')} {i}%");
    //j++;
    //if (j == 4) j = 0;
}

Console.ReadKey(); 
Console.Clear();
goto start;


public class ConsoleProgressPrinter: IDisposable
{
    char[] progressChars = "-\\|/".ToCharArray();
    private Timer _timer;
    int _interval = 50;
    private int totalLength = 50;
    private int charIndexer = 0;

    public int TotalProgress { get; set; }
    private int CurrentProgress = 0;
    

    public ConsoleProgressPrinter(int totalProgress)
    {
        TotalProgress = totalProgress;
        _timer = new Timer(Tick, null, _interval, Timeout.Infinite);
    }

    public void ReportProgress()
    {
        CurrentProgress++; 
    }

    private void Tick(object state)
    {
        try
        {
            // Put your code in here
            if (CurrentProgress == 0) return;

            Console.ForegroundColor = ConsoleColor.Green;

            if (CurrentProgress < TotalProgress)
            {
                var percentageLength = (CurrentProgress * totalLength) / TotalProgress;
                var percentage = (CurrentProgress * 100) / TotalProgress;
                Console.Write($"\rProgress [{progressChars[charIndexer]}]: {new string('|', percentageLength).PadRight(totalLength, '-')} {percentage}%");
                charIndexer++;
                if (charIndexer == 4) charIndexer = 0;
                return;
            }

            
            Console.WriteLine($"\rProgress [*]: {new string('|', totalLength).PadRight(totalLength, '-')} 100%");
            
            
        }
        finally
        {
            _timer?.Change(_interval, Timeout.Infinite);
            Console.ResetColor();
        }
        _timer?.Dispose();
    }

    public void Dispose()
    {
        _timer.Dispose();
    }
}


//namespace ListObjectsPaginatorExample
//{
//    using System;
//    using System.Threading.Tasks;
//    using Amazon.S3;
//    using Amazon.S3.Model;

//    /// <summary>
//    /// The following example lists objects in an Amazon Simple Storage
//    /// Service (Amazon S3) bucket. It was created using AWS SDK for .NET 3.5
//    /// and .NET Core 5.0.
//    /// </summary>
//    public class ListObjectsPaginator
//    {
//        private const string BucketName = "doc-example-bucket";

//        public static async Task Main()
//        {
//            IAmazonS3 s3Client = new AmazonS3Client();

//            Console.WriteLine($"Listing the objects contained in {BucketName}:\n");
//            await ListingObjectsAsync(s3Client, BucketName);
//        }

//        /// <summary>
//        /// This method uses a paginator to retrieve the list of objects in an
//        /// an Amazon S3 bucket.
//        /// </summary>
//        /// <param name="client">An Amazon S3 client object.</param>
//        /// <param name="bucketName">The name of the S3 bucket whose objects
//        /// you want to list.</param>
//        public static async Task ListingObjectsAsync(IAmazonS3 client, string bucketName)
//        { 
//            var listObjectsV2Paginator = client.Paginators.ListObjectsV2(new ListObjectsV2Request
//            {
//                BucketName = bucketName,
//            });

//            await foreach (var response in listObjectsV2Paginator.Responses)
//            {
//                Console.WriteLine($"HttpStatusCode: {response.HttpStatusCode}");
//                Console.WriteLine($"Number of Keys: {response.KeyCount}");
//                foreach (var entry in response.S3Objects)
//                {
//                    Console.WriteLine($"Key = {entry.Key} Size = {entry.Size}");
//                }
//            }
//        }
//    }

//}