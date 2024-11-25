
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

                // 画像のアップロード
                EndPoints.Blob? blob = null;
                using (var testImageStream = File.OpenRead("TestImage.png"))
                {
                    var uploadResult = service.Repo.UploadBlobAsync(new EndPoints.Repo.UploadBlobContent("image/png", testImageStream)).Result;
                    blob = uploadResult?.Blob;
                }

#if true
                // com.atproto.repo.createRecord を使った方法

                var postParam = new EndPoints.Repo.CreateRecordParam()
                {
                    Repo = handle,
                    Collection = "app.bsky.feed.post",
                    Record = new EndPoints.Record()
                    {
                        Text = "TEST POST: " + message + " (" + DateTimeOffset.Now.ToString() + ")",
                        CreatedAt = DateTimeOffset.Now,
                        Embed = new EndPoints.Embed()
                        {

                            Type = "app.bsky.embed.images",
                            Images = new EndPoints.AttachedImage[]
                            {
                                new EndPoints.AttachedImage()
                                {
                                    //AspectRatio = new EndPoints.AspectRatio() { Width = 525, Height = 280 },
                                    Alt = "Test image",
                                    Image = blob,
                                }
                            }
                        }
                    }
                };

                var result = service.Repo.CreateRecordAsync(postParam).Result;
#else
                // com.atproto.repo.applyWrites を使った方法

                var writeParam = new EndPoints.Repo.ApplyWritesParam()
                {
                    Repo = handle,
                    Validate = true,
                    Writes = new EndPoints.Write[]
                    {
                        new EndPoints.Write()
                        {
                            Type = "com.atproto.repo.applyWrites#create",
                            Collection = "app.bsky.feed.post",
                            Value = new EndPoints.Record()
                            {
                                Text = "TEST POST (W): " + message + " (" + DateTimeOffset.Now.ToString() + ")",
                                CreatedAt = DateTimeOffset.Now,
                                Embed = new EndPoints.Embed()
                                {
                                    Type = "app.bsky.embed.images",
                                    Images = new EndPoints.AttachedImage[]
                                    {
                                        new EndPoints.AttachedImage()
                                        {
                                            //AspectRatio = new EndPoints.AspectRatio() { Width = 525, Height = 280 },
                                            Alt = "Test image",
                                            Image = blob,
                                        }
                                    }
                                }
                            }
                        }
                    }
                };

                var result = service.Repo.ApplyWrites(writeParam).Result;
#endif

#if true
                // Markdown 記法の利用
                var md = "こんにちは！これはリンク埋め込みのテストです。\n\nこれは [GitHub へのリンク](https://github.com/) です。見えますか？";
                //md = "hello, this is test post. This is [Link to GitHub](https://github.com/). Can you see?";

                var recordFromMd = EndPoints.Record.FromMarkdownText(md + "\n\n" + Guid.NewGuid());
                recordFromMd.Embed = EndPoints.Embed.FromExternal(new EndPoints.ExternalReference()
                {
                    Title = "Hoge hoge Web site",
                    Description = "hoge foo bar",
                    Uri = new Uri("https://www.a32kita.net")
                });

                var postParam2 = new EndPoints.Repo.CreateRecordParam()
                {
                    Repo = handle,
                    Collection = "app.bsky.feed.post",
                    Record = recordFromMd,
                };

                var result2 = service.Repo.CreateRecordAsync(postParam2).Result;
#endif
            }

            Console.WriteLine("Please press [Enter] key to exit ...");
            Console.ReadLine();
        }
    }
}
