using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BucketLifeCyclePolicies
{
    internal class NsLookupHelper
    {
        internal static List<string> LoadIps(string regionName)
        {
            var url = $"s3.{regionName}.wasabisys.com";

            var nsLookUpResult = getNSLookup(url);

            //extracting IPS:

            var regex = "([0-9]{1,3})\\.([0-9]{1,3})\\.([0-9]{1,3})\\.([0-9]{1,3})";

            return Regex.Matches(nsLookUpResult, regex).Skip(1).Select(m => m.Value).ToList();
        }


        static string getNSLookup(string IPAddress)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
            psi.FileName = "nslookup.exe";
            psi.Arguments = IPAddress;
            /// here is the key code (these two lines)
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;

            psi.CreateNoWindow = false;
            p.StartInfo = psi;
            p.Start();

            p.WaitForExit();
            /// this is where the output from nslookup will be stored, p.StandardOutput
            System.IO.StreamReader output = p.StandardOutput;

            System.Text.StringBuilder sb = new StringBuilder();
            while (output.Peek() > -1)
            {
                /// foreach outputed line, store it in the StringBuilder and append a new line after it
                sb.Append(output.ReadLine() + Environment.NewLine);
            }
            
            psi = null; p = null;

            return sb.ToString();
        }
    }
}
