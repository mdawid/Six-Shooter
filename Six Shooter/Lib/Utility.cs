using System.Linq;
using Windows.Networking.Connectivity;

namespace Six_Shooter.Lib
{
    class Utility
    {
        Utility()
        {

        }

        public static string GetLocalIp()
        {
            var icp = NetworkInformation.GetInternetConnectionProfile();

            if (icp?.NetworkAdapter == null) return null;
            var hostname = NetworkInformation.GetHostNames()
                .SingleOrDefault( hn => hn.IPInformation?.NetworkAdapter != null && hn.IPInformation.NetworkAdapter.NetworkAdapterId == icp.NetworkAdapter.NetworkAdapterId);
            // the ip address
            return hostname?.CanonicalName;
        }
    }
}
