using System;

namespace NSWallet
{
    public interface IBuild
    {
        string GetBuildNumber();
        string GetPlatform();
        string GetVersion();
    }
}
