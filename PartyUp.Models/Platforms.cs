using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PartyUp.Models
{
    public class Platforms
    {
        public enum Platform
        {
            Xbox360,
            XboxOne,
            PlayStation3,
            PlayStation4,
            PC,
            Wii,
            WiiU
        };

        public static string getPrettyName(Platform platform)
        {
            switch (platform)
            {
                case Platform.Xbox360:
                    return "Xbox 360";
                case Platform.XboxOne:
                    return "Xbox One";
                case Platform.PlayStation3:
                    return "PlayStation 3";
                case Platform.PlayStation4:
                    return "PlayStation 4";
                case Platform.PC:
                    return "PC";
                case Platform.Wii:
                    return "Wii";
                case Platform.WiiU:
                    return "Wii U";
                default:
                    return "Xbox One";
            }
        }

        public static string getPrettyName(int platform)
        {
            Platform p = (Platform)platform;
            switch (p)
            {
                case Platform.Xbox360:
                    return "Xbox 360";
                case Platform.XboxOne:
                    return "Xbox One";
                case Platform.PlayStation3:
                    return "PlayStation 3";
                case Platform.PlayStation4:
                    return "PlayStation 4";
                case Platform.PC:
                    return "PC";
                case Platform.Wii:
                    return "Wii";
                case Platform.WiiU:
                    return "Wii U";
                default:
                    return "Xbox One";
            }
        }

        public static int getIntFromName(string platform)
        {
            switch (platform)
            {
                case "Xbox 360":
                    return (int)Platform.Xbox360;
                case "Xbox One":
                    return (int)Platform.XboxOne;
                case "PlayStation 3":
                    return (int)Platform.PlayStation3;
                case "PlayStation 4":
                    return (int)Platform.PlayStation4;
                case "PC":
                    return (int)Platform.PC;
                case "Wii":
                    return (int)Platform.Wii;
                case "Wii U":
                    return (int)Platform.WiiU;
            }

            return 0;
        }
    }
}
