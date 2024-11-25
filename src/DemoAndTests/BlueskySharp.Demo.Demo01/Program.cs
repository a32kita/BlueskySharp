namespace BlueskySharp.Demo.Demo01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Your handle  >");
            var handle = Console.ReadLine();

            Console.Write("App Password >");
            var password = Console.ReadLine();

            Console.WriteLine("Logging in ...");
            var service = BlueskyService.LoginWithLoginInfoAsync(
                BlueskyServiceInfo.BskySocial,
                new BlueskyLoginInfo() { Handle = handle, Password = password }).Result;

            Console.WriteLine("Login is success !!");

            while (true)
            {
                Console.Write("Post message >>");
                var message = Console.ReadLine();
                if (String.IsNullOrEmpty(message))
                    break;

                var postParam = new EndPoints.Repo.CreateRecordParam()
                {
                    Repo = handle,
                    Collection = "app.bsky.feed.post",
                    Record = new EndPoints.Record()
                    {
                        Text = "TEST POST: " + message + " (" + DateTimeOffset.Now.ToString() + ")",
                        CreatedAt = DateTimeOffset.Now,
                    }
                };

                var result = service.Repo.CreateRecordAsync(postParam).Result;
            }

            Console.WriteLine("Please press [Enter] key to exit ...");
            Console.ReadLine();
        }
    }
}
