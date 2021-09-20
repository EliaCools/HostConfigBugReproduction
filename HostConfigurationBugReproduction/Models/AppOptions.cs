using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostConfigurationBugReproduction.Models
{
    public class AppOptions
    {
        public string ApiBaseUrl { get; set; } = null!;

        public OAuthOptions OAuthOptions { get; set; } = null!;
    }

    public class OAuthOptions
    {
        public string RedirectUrl { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public string Team { get; set; } = null!;
    }
}
