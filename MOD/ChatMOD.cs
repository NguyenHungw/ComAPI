using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COM.MOD
{
    public class ChatAdminMOD
    {
        public int ID { get; set; }
        public int RoomID { get; set; }
        public int FromUserID { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
        [DefaultValue(true)]
        public bool? IsFromAdmin { get; set; }

        [DefaultValue(true)]
        public bool IsSeenByAdmin { get; set; }
        [DefaultValue(false)]
        public bool? IsSeenByUser { get; set; } 
    }
    public class ChatUserMOD
    {
        public int ID { get; set; }
        public int RoomID { get; set; }
        public int FromUserID { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
        [DefaultValue(false)]
        public bool? IsFromAdmin { get; set; }

        [DefaultValue(false)]
        public bool IsSeenByAdmin { get; set; }
        [DefaultValue(true)]
        public bool? IsSeenByUser { get; set; }
    }
    public class ChatMOD
    {
        public int ID { get; set; }
        public int RoomID { get; set; }
        public int FromUserID { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }
        public bool IsFromAdmin { get; set; }

        public bool IsSeenByAdmin { get; set; }
        public bool IsSeenByUser { get; set; }
    }
    public class TinNhanChuaDocMOD
    {
        public int ID { get; set; } 
        public int RoomID { get; set;}
        public string FullName { get; set; }    
        public string Message { get; set;}  
        public DateTime SentAt { get; set;}
        public bool IsSeenByAdmin { get; set;}
    }
}
