using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BlueskySharp.Test.UnitTest101
{
    internal class TestHelper
    {
        public static void AssertAndOutputExceptionDetail(Exception ex)
        {
            if (ex == null)
                return;

            if (ex is AggregateException aggEx)
            {
                foreach (var innEx in aggEx.InnerExceptions)
                {
                    Console.WriteLine($"InnerException: {innEx.GetType().Name} - {innEx.Message}");
                    Console.WriteLine(innEx.StackTrace);
                }

                Assert.Fail("AggregateException occurred during test execution.");
            }
            else
            {
                Console.WriteLine($"Exception: {ex.GetType().Name} - {ex.Message}");
                Console.WriteLine(ex.StackTrace);

                Assert.Fail("Exception occurred during test execution.");
            }
        }

        public static void AssertSessionInfo(BlueskyService blueskyService)
        {
            AssertSessionInfo(blueskyService.GetSessionInfo());
        }

        public static void AssertSessionInfo(BlueskySessionInfo sessionInfo)
        {
            Assert.IsFalse(String.IsNullOrEmpty(sessionInfo.AccessJwt), "AccessJwt is empty");
            Assert.IsFalse(String.IsNullOrEmpty(sessionInfo.RefreshJwt), "RefreshJwt is empty");
            Assert.IsTrue(sessionInfo.AccessJwtExpiration - DateTimeOffset.Now > TimeSpan.FromMinutes(60), "AccessJwt expiration time is too short");
            Assert.IsTrue(sessionInfo.RefreshJwtExpiration - DateTimeOffset.Now > TimeSpan.FromDays(5), "RefreshJwt expiration time is too short");
        }
    }
}
