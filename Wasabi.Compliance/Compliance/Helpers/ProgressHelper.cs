namespace Compliance.Helpers
{
    internal class ProgressHelper
    {
        private static bool _inProgress;
        private static Timer _timer; 
        static char[] progressChars = "-\\|/".ToCharArray();
        static int charIndexer = 0;
        static int _interval = 50;
        static string _actionMessage = ""; 

        public static void ShowProgress(string actionMessage="")
        {
            if(_timer is null)
            {
                _timer = new Timer(Tick, null, _interval, Timeout.Infinite);
            } 

            _inProgress = true;
            _actionMessage = actionMessage;
        } 

        public static void EndProgress()
        {
            _inProgress = false;
        }

        private static void Tick(object state)
        {
            try
            {
                if (!_inProgress)
                {
                    Console.Write("\r");
                    charIndexer = 0;

                }
                else
                {
                    Console.Write($"\rLoading [{progressChars[charIndexer]}]: {_actionMessage}"); 
                    charIndexer++;
                    if (charIndexer == 4) charIndexer = 0;
                    return;
                } 
            }
            finally
            {
                _timer?.Change(_interval, Timeout.Infinite);
                Console.ResetColor();
            }
            _timer?.Dispose();
        }
    }
}
