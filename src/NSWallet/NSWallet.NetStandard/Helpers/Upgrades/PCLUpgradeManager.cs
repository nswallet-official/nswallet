using System;
using System.Collections.Generic;
using System.Diagnostics;
using NSWallet.Shared;

namespace NSWallet.Helpers
{
    public static partial class PCLUpgradeManager
    {
        public static void PrepareUpdate()
        {
            PrepareUpdateTo02();
        }

        public static void RemoveAfterUpdate()
        {
            RemoveAfterUpdate02();
        }
    }
}