using System.Runtime.CompilerServices;

namespace BlueskySharp.Test.UnitTest101
{
    [TestClass]
    public class UnitTest101001
    {
        // Bluesky login information
        private static string s_handle;
        private static string s_appPassword;

        private BlueskyService? _bskyService;


        /// <summary>
        /// 
        /// </summary>
        static UnitTest101001()
        {
            s_handle = string.Empty;
            s_appPassword = string.Empty;
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            // Prompts the user for credentials at the start of a unit test

            var envValue = Environment.GetEnvironmentVariable("BSKYSHARP_TEST_AUTH");
            if (String.IsNullOrEmpty(envValue))
            {
                throw new InvalidOperationException("Authentication token is not set in environment variables 'BSKYSHARP_TEST_AUTH'.");
            }

            var envValueSptl = envValue.Split(',');
            if (envValueSptl.Length != 2)
                throw new InvalidOperationException();

            s_handle = envValueSptl[0].Trim();
            s_appPassword = envValueSptl[1].Trim();
        }

        private async Task _wait()
        {
            await Task.Delay(1000);
        }

#if true
        [TestMethod]
        public async Task Server_CreateSession()
        {
            try
            {
                var bskySv = await BlueskyService.LoginWithLoginInfoAsync(
                    BlueskyServiceInfo.BskySocial,
                    new() { Handle = s_handle, Password = s_appPassword });

                Assert.IsNotNull(bskySv, "bskySv is null");
                TestHelper.AssertSessionInfo(bskySv);
            }
            catch (Exception ex)
            {
                TestHelper.AssertAndOutputExceptionDetail(ex);
            }
            finally
            {
                await this._wait();
            }
        }
#endif

#if true
        [TestMethod]
        public async Task Server_CreateSession_Error01()
        {
            try
            {
                var bskySv = await BlueskyService.LoginWithLoginInfoAsync(
                    BlueskyServiceInfo.BskySocial,
                    new() { Handle = s_handle, Password = "invalidpassword" });
            }
            catch (BlueskySharpErrorException ex)
            {
                Assert.AreEqual(ex.Error, "AuthenticationRequired");
            }
            catch (Exception ex)
            {
                TestHelper.AssertAndOutputExceptionDetail(ex);
            }
            finally
            {
                await this._wait();
            }
        }
#endif

#if true
        [TestMethod]
        public async Task Server_DeleteSession()
        {
            try
            {
                var bskySv = await BlueskyService.LoginWithLoginInfoAsync(
                    BlueskyServiceInfo.BskySocial,
                    new() { Handle = s_handle, Password = s_appPassword });

                await bskySv.Server.DeleteSessionAsync();
            }
            catch (Exception ex)
            {
                TestHelper.AssertAndOutputExceptionDetail(ex);
            }
            finally
            {
                await this._wait();
            }
        }
#endif

        private async Task _createCommonSession()
        {
            if (this._bskyService != null)
                return;

            this._bskyService = await BlueskyService.LoginWithLoginInfoAsync(
                BlueskyServiceInfo.BskySocial,
                new() { Handle = s_handle, Password = s_appPassword });
        }

        private async Task _deleteCommonSession()
        {
            if (this._bskyService == null)
                return;

            await this._bskyService.Server.DeleteSessionAsync();
            this._bskyService.Dispose();
            this._bskyService = null;
        }

#if true
        [TestMethod]
        public async Task Repo_CreateRecord_01()
        {
            await this._createCommonSession();
            if (this._bskyService == null)
                throw new InvalidOperationException();

            var postTime = DateTimeOffset.Now;
            var postParam = new Endpoints.Repo.CreateRecordParam()
            {
                Repo = s_handle,
                Collection = "app.bsky.feed.post",
                Record = new()
                {
                    Text = $"[BlueskySharp] TEST POST - Plain text\n\n{Guid.NewGuid().ToString()} ({postTime.ToString()})",
                    CreatedAt = postTime,
                }
            };

            try
            {
                var postResult = await this._bskyService.Repo.CreateRecordAsync(postParam);

                Assert.IsNotNull(postResult, "postResult is null");
            }
            catch (Exception ex)
            {
                TestHelper.AssertAndOutputExceptionDetail(ex);
            }
            finally
            {
                await this._wait();
            }
        }
#endif

#if true
        [TestMethod]
        public async Task Repo_CreateRecord_02()
        {
            await this._createCommonSession();
            if (this._bskyService == null)
                throw new InvalidOperationException();

            var postTime = DateTimeOffset.Now;
            var postParam = new Endpoints.Repo.CreateRecordParam()
            {
                Repo = s_handle,
                Collection = "app.bsky.feed.post",
                Record = Endpoints.Record.FromMarkdownText($"[BlueskySharp] TEST POST - Plain text [with link](https://github.com/a32kita/BlueskySharp)\n\n{Guid.NewGuid().ToString()} ({postTime.ToString()})"),
            };

            try
            {
                var postResult = await this._bskyService.Repo.CreateRecordAsync(postParam);

                Assert.IsNotNull(postResult, "postResult is null");
            }
            catch (Exception ex)
            {
                TestHelper.AssertAndOutputExceptionDetail(ex);
            }
            finally
            {
                await this._wait();
            }
        }
#endif

#if true
        [TestMethod]
        public async Task Repo_CreateRecord_03()
        {
            await this._createCommonSession();
            if (this._bskyService == null)
                throw new InvalidOperationException();

            var postTime = DateTimeOffset.Now;
            var postParam = new Endpoints.Repo.CreateRecordParam()
            {
                Repo = s_handle,
                Collection = "app.bsky.feed.post",
                Record = new()
                {
                    Text = $"[BlueskySharp] TEST POST - Plain text with card\n\n{Guid.NewGuid().ToString()} ({postTime.ToString()})",
                    CreatedAt = postTime,
                    Embed = Endpoints.Embed.FromExternal(new()
                    {
                        Title = "Explorers of the Binary World (Example card)",
                        Description = "This is example card.",
                        Uri = new Uri("https://www.a32kita.net"),
                    })
                }
            };

            try
            {
                var postResult = await this._bskyService.Repo.CreateRecordAsync(postParam);

                Assert.IsNotNull(postResult, "postResult is null");
            }
            catch (Exception ex)
            {
                TestHelper.AssertAndOutputExceptionDetail(ex);
            }
            finally
            {
                await this._wait();
            }
        }
#endif

#if true
        [TestMethod]
        public async Task Repo_CreateRecord_04()
        {
            await this._createCommonSession();
            if (this._bskyService == null)
                throw new InvalidOperationException();

            var imageMs = ImageGenerator.CreateTestImage();
            if (imageMs == null)
                throw new InvalidOperationException();

            Endpoints.Repo.UploadBlobResult? uploadResult = null;
            try
            {
                uploadResult = await this._bskyService.Repo.UploadBlobAsync(
                    new("image/png", imageMs));
                Assert.IsNotNull(uploadResult, "postResult is null");
            }
            catch (Exception ex)
            {
                TestHelper.AssertAndOutputExceptionDetail(ex);
            }

            var blob = uploadResult?.Blob;

            var postTime = DateTimeOffset.Now;
            var postParam = new Endpoints.Repo.CreateRecordParam()
            {
                Repo = s_handle,
                Collection = "app.bsky.feed.post",
                Record = new()
                {
                    Text = $"[BlueskySharp] TEST POST - Plain text with Image\n\n{Guid.NewGuid().ToString()} ({postTime.ToString()})",
                    CreatedAt = postTime,
                    Embed = Endpoints.Embed.FromImages([
                        new()
                        {
                            Image = blob,
                            Alt = String.Empty, //"Test image",
                        }
                    ])
                }
            };

            try
            {
                var postResult = await this._bskyService.Repo.CreateRecordAsync(postParam);

                Assert.IsNotNull(postResult, "postResult is null");
            }
            catch (Exception ex)
            {
                TestHelper.AssertAndOutputExceptionDetail(ex);
            }
            finally
            {
                await this._wait();
            }
        }
#endif

#if true
        [TestMethod]
        public async Task BskyActor_GetProfile()
        {
            await this._createCommonSession();
            if (this._bskyService == null)
                throw new InvalidOperationException();

            try
            {
                var profile = await this._bskyService.BskyActor.GetProfileAsync(new()
                {
                    Actor = "a32kita.net"
                });

                Assert.IsNotNull(profile, "profile is null");
                Assert.AreEqual(profile.Handle, "a32kita.net", false, $"Handle is not 'a32kita.net' ({profile.Handle}).");
                Assert.AreEqual(profile.Did, "did:plc:razh7yhowxfxzxswdkerdbhf", false, $"Handle is not 'did:plc:razh7yhowxfxzxswdkerdbhf' ({profile.Handle}).");
            }
            catch (Exception ex)
            {
                TestHelper.AssertAndOutputExceptionDetail(ex);
            }
            finally
            {
                await this._wait();
            }
        }
#endif
    }
}