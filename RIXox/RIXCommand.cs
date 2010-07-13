using System;
using System.Runtime.Serialization;

namespace RIXox
{
    [Serializable()]
    public class RIXCommand : ISerializable
    {
        public String Command { get; set; }
        public String Data { get; set; }
        public int ClientId { get; set; }

        public RIXCommand(String Command, String Data, int ClientId)
        {
            this.Command = Command;
            this.Data = Data;
            this.ClientId = ClientId;
        }

        public RIXCommand(SerializationInfo info, StreamingContext context)
        {
            Command = (String) info.GetValue("Command", typeof (String));
            Data = (String)info.GetValue("Data", typeof(String));
            ClientId = (int) info.GetValue("ClientId", typeof (int));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Command", Command);
            info.AddValue("Data", Data);
            info.AddValue("ClientId", ClientId);
        }

        public RIXCommand() {}        
    }
}
