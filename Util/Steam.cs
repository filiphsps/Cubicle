using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubicle.NET.Util
{
    public class Steam
    {
        public Steam()
        {
            SteamClient.Init(1882990);
        }
    }
}
