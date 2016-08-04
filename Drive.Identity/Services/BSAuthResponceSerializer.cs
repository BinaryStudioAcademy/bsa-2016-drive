using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Drive.Identity.Entities;

namespace Drive.Identity.Services
{
    internal class BSAuthResponceSerializer
    {
        public static BSAuthResponse Deserialize(string response)
        {
            var jsonFormatter = new DataContractJsonSerializer(typeof(BSAuthResponse));
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(response)))
            {
                return (BSAuthResponse)jsonFormatter.ReadObject(ms);
            }
        }

    }
}
