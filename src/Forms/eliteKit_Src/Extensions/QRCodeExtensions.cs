using System.IO;
using Xamarin.Forms;

namespace eliteKit.Extensions
{
    public static class QRCodeExtensions
    {
        public static ImageSource ToImageSource(this byte[] bytes)
        {
            try
            {
                Stream stream = new MemoryStream(bytes);
                return ImageSource.FromStream(() => { return stream; });
            }
            catch { }
            return null;
        }

        public static void CopyTo(System.IO.Stream input, System.IO.Stream output)
        {
            byte[] buffer = new byte[16 * 1024];
            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }

        public static string ReverseString(string str)
        {
            char[] chars = str.ToCharArray();
            char[] result = new char[chars.Length];
            for (int i = 0, j = str.Length - 1; i < str.Length; i++, j--)
            {
                result[i] = chars[j];
            }
            return new string(result);
        }

        public static bool IsAllDigit(string str)
        {
            foreach (var c in str)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
