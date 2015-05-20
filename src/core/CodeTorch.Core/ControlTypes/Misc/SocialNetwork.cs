using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class SocialNetwork
    {
        public string LabelText { get; set; }
        public SocialNetworkType NetworkType { get; set; }
        public string UrlToShare { get; set; }
        public string UrlToShareFormatString { get; set; }

        public override string ToString()
        {
            return NetworkType.ToString();
        }
    }

    [Serializable]
    public enum SocialNetworkType
    { 
        FacebookShare = 0,
        //
        FacebookLike = 1,
        //
        FacebookSend = 2,
        //
        FacebookRecommend = 3,
        //
        Twitter = 4,
        //
        GooglePlusOne = 5,
        //
        // Summary:
        //     Creates a styled button to share on Google Bookmarks.
        GoogleBookmarks = 6,
        //
        // Summary:
        //     Creates a styled button to share on Twitter.
        ShareOnTwitter = 7,
        //
        // Summary:
        //     Creates a styled button to share on LinkedIn
        LinkedIn = 8,
        //
        // Summary:
        //     Creates a styled button to share on Delicious.
        Delicious = 9,
        //
        // Summary:
        //     Creates a styled button to share on Blogger.
        Blogger = 10,
        //
        // Summary:
        //     Creates a styled button to share on Digg.
        Digg = 11,
        //
        // Summary:
        //     Creates a styled button to share on Reddit.
        Reddit = 12,
        //
        // Summary:
        //     Creates a styled button to share on StumbleUpon.
        StumbleUpon = 13,
        //
        // Summary:
        //     Creates a styled button to share on MySpace.
        MySpace = 14,
        //
        // Summary:
        //     Creates a styled button to share on Tumblr.
        Tumblr = 15,
        //
        // Summary:
        //     Creates a styled button to share on Facebook.
        ShareOnFacebook = 16,
        //
        // Summary:
        //     Creates a styled button to open the user's default mail agent to send an
        //     e-mail.
        MailTo = 17,
        //
        // Summary:
        //     Creates a styled button to open the SendEmail popup. Its functionality requires
        //     a properly configured SMTP server.
        SendEmail = 18,
        //
        CompactButton = 19,
        //
        LinkedInShare = 20,
        //
        // Summary:
        //     Defines a button related to the Yammer network
        Yammer = 21,
        //
        // Summary:
        //     Utilized for the button for sharing to the Yammer network
        ShareOnYammer = 22,
        //
        // Summary:
        //     Defines a button related to the Pinterest network
        Pinterest = 23,
        //
        // Summary:
        //     Creates a styled button for the Pinterest network
        ShareOnPinterest = 24,
    }
}
