using System;

namespace ETS2_DualSenseAT_Mod
{
    internal class Constants
    {
        public static string app_id = "227300";
        public static string game_name = "Euro Truck Simulator 2";

        public static void SetGame(string exeName)
        {
            if (exeName.ToLower().Contains("amtrucks"))
            {
                app_id = "270880";
                game_name = "American Truck Simulator";
            }
            else
            {
                app_id = "227300";
                game_name = "Euro Truck Simulator 2";
            }
        }
    }
}
