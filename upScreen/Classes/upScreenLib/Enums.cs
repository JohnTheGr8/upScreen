namespace upScreenLib
{
    public enum ClientItemType
    {
        File,
        Folder,
        Other
    }

    public enum FtpProtocol
    {
        FTP,
        SFTP,
        FTPS
    }

    public enum FtpsMethod
    {
        None = 0,
        Implicit = 1,
        Explicit = 2
    }

    public enum ImageExtensions
    {
        PNG = 0,
        JPEG = 1,
        GIF = 2
    }
}
