using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RIXoxTestClient
{
    // this is the RadioData struct from MiniDeluxe
    class RadioData
    {
        private string _mode;
        private string _band;
        private string _displayMode;
        private string _agc;
        private string _smeter;
        private string _dspfilter;
        private string _preamp;

        public string vfoa { get; set; }
        public string vfob { get; set; }
        public string rawmode { get; set; }
        public string dspfilters { get; set; }
        public ArrayList dspfilterarray { get; set; }
        public bool mox { get; set; }

        public string Mode
        {
            get { return _mode; }
            set
            {
                rawmode = value;
                switch (value)
                {
                    case "00":
                        _mode = "LSB";
                        break;
                    case "01":
                        _mode = "USB";
                        break;
                    case "02":
                        _mode = "DSB";
                        break;
                    case "03":
                        _mode = "CWL";
                        break;
                    case "04":
                        _mode = "CWU";
                        break;
                    case "05":
                        _mode = "FMN";
                        break;
                    case "06":
                        _mode = "AM";
                        break;
                    case "07":
                        _mode = "DIGU";
                        break;
                    case "08":
                        _mode = "SPEC";
                        break;
                    case "09":
                        _mode = "DIGL";
                        break;
                    case "10":
                        _mode = "SAM";
                        break;
                    case "11":
                        _mode = "DRM";
                        break;
                    case "99":
                        _mode = "OFF";
                        break;
                    default:
                        _mode = value;
                        break;
                }
            }
        }
        public string Band
        {
            get { return _band; }
            set
            {
                switch (value)
                {
                    case "160":
                        _band = "160m";
                        break;
                    case "080":
                        _band = "80m";
                        break;
                    case "060":
                        _band = "60m";
                        break;
                    case "040":
                        _band = "40m";
                        break;
                    case "030":
                        _band = "30m";
                        break;
                    case "020":
                        _band = "20m";
                        break;
                    case "017":
                        _band = "17m";
                        break;
                    case "015":
                        _band = "15m";
                        break;
                    case "012":
                        _band = "12m";
                        break;
                    case "010":
                        _band = "10m";
                        break;
                    case "006":
                        _band = "6m";
                        break;
                    case "002":
                        _band = "2m";
                        break;
                    case "888":
                        _band = "GEN";
                        break;
                    case "999":
                        _band = "WWV";
                        break;
                    default:
                        _band = value;
                        break;
                }
            }
        }
        public string DisplayMode
        {
            get { return _displayMode; }
            set
            {
                switch (value)
                {
                    case "0":
                        _displayMode = "Spectrum";
                        break;
                    case "1":
                        _displayMode = "Panadapter";
                        break;
                    case "2":
                        _displayMode = "Scope";
                        break;
                    case "3":
                        _displayMode = "Phase";
                        break;
                    case "4":
                        _displayMode = "Phase2";
                        break;
                    case "5":
                        _displayMode = "Waterfall";
                        break;
                    case "6":
                        _displayMode = "Histogram";
                        break;
                    case "7":
                        _displayMode = "Off";
                        break;
                }
            }
        }
        public string AGC
        {
            get { return _agc; }
            set
            {
                switch (value)
                {
                    case "0":
                        _agc = "Fixed";
                        break;
                    case "1":
                        _agc = "Long";
                        break;
                    case "2":
                        _agc = "Slow";
                        break;
                    case "3":
                        _agc = "Med";
                        break;
                    case "4":
                        _agc = "Fast";
                        break;
                    case "5":
                        _agc = "Custom";
                        break;
                }
            }
        }
        public string Smeter
        {
            get { return _smeter; }
            set
            {
                float i = (float.Parse(value) / 2f) - 121f;
                if (i < -121) _smeter = "0";
                else if (i < -115) _smeter = "1";
                else if (i < -109) _smeter = "2";
                else if (i < -103) _smeter = "3";
                else if (i < -97) _smeter = "4";
                else if (i < -91) _smeter = "5";
                else if (i < -85) _smeter = "6";
                else if (i < -79) _smeter = "7";
                else if (i < -73) _smeter = "8";
                else if (i < -63) _smeter = "9";
                else if (i < -53) _smeter = "10";
                else if (i < -43) _smeter = "11";
                else if (i < -33) _smeter = "12";
                else if (i < -23) _smeter = "13";
                else if (i < -13) _smeter = "14";
            }
        }
        public string DSPFilter
        {
            get { return _dspfilter; }
            set
            {
                try
                {
                    _dspfilter = (String)dspfilterarray[int.Parse(value)];
                }
                catch (Exception)
                {
                    _dspfilter = "UNKN";
                }
            }
        }
        public string Preamp
        {
            get { return _preamp; }
            set
            {
                switch (value)
                {
                    case "0":
                        _preamp = "Off";
                        break;
                    case "1":
                        _preamp = "Low";
                        break;
                    case "2":
                        _preamp = "Med";
                        break;
                    case "3":
                        _preamp = "High";
                        break;
                }
            }
        }
    }
}
