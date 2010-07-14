using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace RIXoxTestClient
{
    // this is the RadioData struct from MiniDeluxe
    [Serializable()]
    public class RadioData : ISerializable, INotifyPropertyChanged
    {
        private string _vfoa;

        public string vfoa { get { return _vfoa; } set { _vfoa = value; NotifyPropertyChanged("vfoa"); } }
        public string vfob { get; set; }
        public string rawmode { get; set; }
        public string dspfilters { get; set; }
        public bool mox { get; set; }

        public string Mode { get; set; }
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

        public RadioData(SerializationInfo info, StreamingContext context)
        {
            vfoa = (string)info.GetValue("vfoa", typeof (string));
            vfob = (string)info.GetValue("vfob", typeof(string));
            Mode = (string)info.GetValue("mode", typeof(string));
            mox = (bool)info.GetValue("mox", typeof(bool));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("vfoa", vfoa);
            info.AddValue("vfob", vfob);
            info.AddValue("mode", Mode);
            info.AddValue("mox", mox);            
        }

        public override String ToString()
        {
            return "VFOA: " + vfoa + " VFOB: " + vfob;
        }
    }
}
