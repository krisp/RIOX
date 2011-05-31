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
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Collections;

namespace RIOX
{    
    [Serializable()]
    public class RIOXData : ISerializable, INotifyPropertyChanged
    {
        [NonSerialized]
        private Hashtable _data;

        public Hashtable Data { get { return _data; } set { _data = value; NotifyPropertyChanged("data"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public RIOXData()
        {
        }

        // This is for deserialization
        public RIOXData(SerializationInfo info, StreamingContext context)
        {
             Data = (Hashtable)info.GetValue("data", typeof(Hashtable));
        }

        // this is for serialization
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("data", Data);
        }
    }
}
