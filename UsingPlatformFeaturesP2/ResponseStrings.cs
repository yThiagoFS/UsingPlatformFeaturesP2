namespace UsingPlatformFeaturesP2
{
    public class ResponseStrings
    {
        public static string DefaultResponse = @"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset=""utf-8"" />
                    <link rel=""stylesheet"" href=""~/lib/bootstrap/dist/css/bootstrap-grid.min.css"" />
                    <title>Error</title>
                </head>
                <body>
                    <h3 class=""text-danger"">Error {0}/h3>
                    <h6>You can go back to the <a href=""/"">Homepage</a></h6>
                </body>
                </html>";
    }
}
