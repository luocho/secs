using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SECSEntity
{
    public class SETEntity
    {
        public static SETEntity SecsObject { get;private set; }
        public string IP { get; set; }
        public int Port { get; set; }
        public int DeviceID { get; set; }
        public ConnectionModel connectionModel { get; set; } = ConnectionModel.Active;

        public void Init(SETEntity Object) { 
            SecsObject=Object;
        }
    }
    public enum ConnectionModel
    {
        Active,
        Passive
    }
}
