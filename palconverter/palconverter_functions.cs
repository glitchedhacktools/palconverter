using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace palconverter
{
    public class Palconverter_functions
    {
        public List<Color_256> colorList = new List<Color_256>();

        public string ReadJASC(string path)
        {
            colorList.Clear();
            string result = File.ReadAllText(path);
            if (result.Contains("JASC-PAL"))
            {
                SaveJASCColorsToList(result);
                return result;
            }
            return null;
        }

        public string ReadHex(string path)
        {
            colorList.Clear();
            string result = "";
            int i;
            byte[] alternate = File.ReadAllBytes(path);
            for (i = 0; i < alternate.Length; i++)
            {
                result = result + alternate[i].ToString("X2");
                if (i < alternate.Length)
                {
                    result = result + ' ';
                }
            }
            SaveHexColorsToList(result);
            return result;
        }

        public void SaveHexColorsToList(string pal)
        {
            int i;
            string[] bytes = pal.Split(' ');
            for (i = 0; i < bytes.Length-1; i = i + 3)
            {
                Color_256 current = new Color_256();
                current.R = int.Parse(bytes[i + 0], System.Globalization.NumberStyles.HexNumber);
                current.G = int.Parse(bytes[i + 1], System.Globalization.NumberStyles.HexNumber);
                current.B = int.Parse(bytes[i + 2], System.Globalization.NumberStyles.HexNumber);
                colorList.Add(current);
            }
        }

        public void SaveJASCColorsToList(string pal)
        {
            int i;
            string[] bytes = pal.Split('\n');
            Color_256 black = new Color_256();
            black.R = 0;
            black.G = 0;
            black.B = 0;
            for (i = 3; i < 258; i++)
            {
                try
                {
                    Color_256 current = new Color_256();
                    current.R = int.Parse(bytes[i].Split(' ')[0]);
                    current.G = int.Parse(bytes[i].Split(' ')[1]);
                    current.B = int.Parse(bytes[i].Split(' ')[2]);
                    colorList.Add(current);
                }
                catch
                {
                    colorList.Add(black);
                }
            }
        }

        public string ConvertACTtoJASC(int numColors)
        {
            string JASC_result = "JASC-PAL\r\n0100\r\n" + numColors + "\r\n";
            int i;
            for (i = 0; i < numColors; i++)
            {
                JASC_result += colorList[i].R;
                JASC_result += " ";
                JASC_result += colorList[i].G;
                JASC_result += " ";
                JASC_result += colorList[i].B;
                JASC_result += "\r\n";
            }
            return JASC_result;
        }

        public string ConvertJASCtoACT(int numColors)
        {
            string ACT_result = "";
            int i;
            for (i = 0; i < numColors; i++)
            {
                    ACT_result += colorList[i].R.ToString("X2");
                    ACT_result += " ";
                    ACT_result += colorList[i].G.ToString("X2");
                    ACT_result += " ";
                    ACT_result += colorList[i].B.ToString("X2");
                    ACT_result += " ";
            }
            return ACT_result;
        }

        public int SaveJASCfile(string pal, string path)
        {
            File.WriteAllText(path, pal);
            return 0;
        }
    }

    public class Color_256
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
    }
}
