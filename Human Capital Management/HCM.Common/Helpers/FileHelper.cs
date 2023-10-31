namespace HCM.Common.Helpers
{
    using System.Text;

    using Microsoft.AspNetCore.Http;

    public class FileHelper
    {
        public static async Task<string> ReadAsStringAsync(IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(await reader.ReadLineAsync());
            }
            return result.ToString();
        }

        public static async Task<byte[]> ReadAsByteArrAsync(IFormFile file)
        {
            long length = file.Length;
            if (length == 0)
            {
                return null;
            }

            using var fileStream = file.OpenReadStream();
            byte[] bytes = new byte[file.Length];
            await fileStream.ReadAsync(bytes, 0, (int)file.Length);

            return bytes;
        }
    }
}
