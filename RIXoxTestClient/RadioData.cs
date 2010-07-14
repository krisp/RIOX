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
