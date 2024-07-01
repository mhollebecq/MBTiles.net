using MBTiles.Parser;
using System.Collections.Generic;

namespace MBTiles.Web.Services
{
    public class MapService
    {
        static Dictionary<string, MapInfos> maps = new Dictionary<string, MapInfos>();

        public MapInfos Get(string name)
        {
            if (!maps.ContainsKey(name))
                maps.Add(name, MapInfos.FromFile(name));

            return maps[name];

        }
    }
}
