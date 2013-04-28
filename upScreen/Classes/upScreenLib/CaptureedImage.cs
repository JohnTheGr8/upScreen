using System.Drawing;

namespace upScreenLib
{
    public static class CapturedImage
    {
        public static Image Image
        { get; set; }

        public static string LocalPath
        { get; set; }

        public static string Username
        { get; set; }

        public static string RemotePath
        { get; set; }

        public static string Name
        { get; set; }

        public static string Link
        { get; set; }

        public static bool Uploaded
        { get; set; }

        public static Bitmap Bmp
        { get; set; }
    }
}
