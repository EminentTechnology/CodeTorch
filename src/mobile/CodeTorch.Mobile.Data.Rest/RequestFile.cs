namespace CodeTorch.Mobile.Data.Rest
{
    public class RequestFile
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public byte[] Bytes { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }
    }
}