using System.IO;
using System.IO.Compression;

namespace Nirvana.Util.Compression
{
    public interface ICompression
    {
        byte[] Compress(byte[] bytes);
        bool IsCompressed(byte[] bytes);
        byte[] Decompress(byte[] bytes);
    }

    public class Compression : ICompression
    {
        public byte[] Compress(byte[] bytes)
        {
            using (var input = new MemoryStream(bytes))
            using (var output = new MemoryStream())
            {
                using (var gs = new GZipStream(output, CompressionMode.Compress))
                {
                    input.CopyTo(gs);
                }

                return output.ToArray();
            }
        }

        public bool IsCompressed(byte[] bytes)
        {
            return bytes.Length >= 2 && bytes[0] == 31 && bytes[1] == 139;
        }

        public byte[] Decompress(byte[] bytes)
        {
            using (var input = new MemoryStream(bytes))
            using (var output = new MemoryStream())
            {
                using (var gs = new GZipStream(input, CompressionMode.Decompress))
                {
                    gs.CopyTo(output);
                }

                return output.ToArray();
            }
        }
    }
}