using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSP.Models
{
    public class MessageCustom : BaseRepository
    {
        #region singleton
        private MessageCustom() { }
        private static MessageCustom get = null;
        public static MessageCustom Get
        {
            get
            {
                if (get == null)
                    get = new MessageCustom();
                return get;
            }
        }
        #endregion singleton
    }
}