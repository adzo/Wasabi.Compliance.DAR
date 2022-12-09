using System.Net;

class Response
{
    public HttpStatusCode FirstGet { get; set; }
    public HttpStatusCode Put { get; set; }
    public HttpStatusCode SecondGet { get; set; }
    public HttpStatusCode Delete { get; set; }
    public string ServerVersion { get; set; }
    public string ServerName { get; set; }

    public override string ToString()
    {
        return base.ToString();
    }
}