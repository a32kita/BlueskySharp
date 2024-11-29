using BlueskySharp.CustomCovertersAndPolicies;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace BlueskySharp.Endpoints.BskyActor
{
    /// <summary>
    /// Bsky.Actor endpoint
    /// </summary>
    public class BskyActorEndpoint : EndpointBase
    {
        internal BskyActorEndpoint(BlueskyService parent)
            : base(parent)
        {
            // NOP
        }

        /// <summary>
        /// Get detailed profile view of an actor. Does not require auth, but contains relevant metadata with auth.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public async Task<ProfileViewDetailed> GetProfileAsync(GetProfileParameter parameter)
        {
            return await this.ExecuteQuery<GetProfileParameter, ProfileViewDetailed>("xrpc/app.bsky.actor.getProfile", parameter);
        }


        public class ProfileViewBasic
        {
            public string Did
            {
                get;
                set;
            }

            public string Handle
            {
                get;
                set;
            }

            public string DisplayName
            {
                get;
                set;
            }

            public string Avatar
            {
                get;
                set;
            }

            public ProfileAssociated Associated
            {
                get;
                set;
            }

            public ViewerState Viewer
            {
                get;
                set;
            }

            // public xxx.Label[] Labels

            [JsonConverter(typeof(CustomDateTimeOffsetConverter))]
            public DateTimeOffset CreatedAt
            {
                get;
                set;
            }
        }


        public class ProfileViewDetailed : ProfileView
        {
            public string Banner
            {
                get;
                set;
            }

            public int FollowersCount
            {
                get;
                set;
            }

            public int FollowsCount
            {
                get;
                set;
            }

            public long PostsCount
            {
                get;
                set;
            }

            // public xxx.StarterPackViewBasic JoinedViaStarterPack

            // public xxx.StrongRef PinnedPost
        }

        public class ProfileAssociated
        {
            public int Lists
            {
                get;
                set;
            }

            public int Feedgens
            {
                get;
                set;
            }

            public int StarterPacks
            {
                get;
                set;
            }

            public bool Labeler
            {
                get;
                set;
            }

            public ProfileAssociatedChat Chat
            {
                get;
                set;
            }
        }

        public class ProfileAssociatedChat
        {
            public ProfileAssociatedChat_AllowIncoming AllowIncoming
            {
                get;
                set;
            }
        }

        public enum ProfileAssociatedChat_AllowIncoming
        {
            All,
            None,
            Following,
        }

        /// <summary>
        /// Metadata about the requesting account's relationship with the subject account. Only has meaningful content for authed requests.
        /// </summary>
        public class ViewerState
        {
            public bool Muted
            {
                get;
                set;
            }

            //public Object MutedByList

            public bool BlockedBy
            {
                get;
                set;
            }

            public string Blocking
            {
                get;
                set;
            }

            public string BlockingByList
            {
                get;
                set;
            }

            public string Following
            {
                get;
                set;
            }

            public string FollowedBy
            {
                get;
                set;
            }

            // public KnownFollowers KnownFollowers
        }
    }
}
