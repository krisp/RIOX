/* This file is part of RIOX.
   RIOX is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   MiniDeluxe is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with RIOX.  If not, see <http://www.gnu.org/licenses/>.
   
   RIOX is Copyright (C) 2010 by K1FSY
*/

using System;
using System.Runtime.Serialization;

namespace RIOX
{
    [Serializable()]
    public class RIOXCommand : ISerializable
    {
        public String Command { get; set; }
        public String Data { get; set; }        

        public RIOXCommand(String Command, String Data)
        {
            this.Command = Command;
            this.Data = Data;            
        }

        public RIOXCommand(SerializationInfo info, StreamingContext context)
        {
            Command = (String) info.GetValue("Command", typeof (String));
            Data = (String)info.GetValue("Data", typeof(String));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Command", Command);
            info.AddValue("Data", Data);            
        }

        public RIOXCommand() {}        
    }
}
