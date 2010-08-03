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

namespace RIXoxTestClient
{
    // this is the RadioData struct from MiniDeluxe
    [Serializable()]
    public class RadioData : ISerializable, INotifyPropertyChanged
    {
        [NonSerialized]
        private string _vfoa;
        [NonSerialized]
        private string _vfob;
        [NonSerialized]
        private bool _mox;
        [NonSerialized]
        private string _mode;

        public string vfoa { get { return _vfoa; } set { _vfoa = value; NotifyPropertyChanged("vfoa"); } }
        public string vfob { get { return _vfob; } set { _vfob = value; NotifyPropertyChanged("vfob"); } }
        public string rawmode { get; set; }
        public string dspfilters { get; set; }
        public bool mox { get { return _mox; } set { _mox = value; NotifyPropertyChanged("mox"); } }

        public string Mode { get { return _mode; } set { _mode = value; NotifyPropertyChanged("mode"); } }
        public string Band { get; set; }
        public string DisplayMode { get; set; }
        public string AGC { get; set; }
        public string Smeter { get; set; }
        public string DSPFilter { get; set; }
        public string Preamp { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public RadioData()
        {
        }

        // This is for deserialization
        public RadioData(SerializationInfo info, StreamingContext context)
        {
            vfoa = (string)info.GetValue("vfoa", typeof (string));
            vfob = (string)info.GetValue("vfob", typeof(string));
            Mode = (string)info.GetValue("mode", typeof(string));
            mox = (bool)info.GetValue("mox", typeof(bool));
        }

        // this is for serialization
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("vfoa", vfoa);
            info.AddValue("vfob", vfob);
            info.AddValue("mode", Mode);
            info.AddValue("mox", mox);            
        }

        // this is just for testing
        public override String ToString()
        {
            return "VFOA: " + vfoa + " VFOB: " + vfob;
        }
    }
}
