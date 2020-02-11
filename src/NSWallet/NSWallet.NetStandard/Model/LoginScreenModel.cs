using System.Collections.Generic;

namespace NSWallet
{
    public class LoginScreenModel
    {
        public string Password { get; set; }

        public List<string> Features { get; set; }

        public double FeaturesHeight { get; set; }

        public int AnimationStatus { get; set; }
    }
}

