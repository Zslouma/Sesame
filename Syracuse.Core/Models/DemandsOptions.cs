using System;
using System.Collections.Generic;
using System.Text;

namespace Syracuse.Mobitheque.Core.Models
{
    public class DemandsOptions
    {
        public DemandsOptions(int demandId, string message)
        {
            this.demandId = demandId;
            this.message = message;
        }

        public int demandId { get; set; }

        public string message { get; set; }

    }

}
