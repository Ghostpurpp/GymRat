using System;
using System.Collections.Generic;

namespace GymRat.Models
{
    public class Session
    {
        public Guid SessionId { get; set; }
        public string UserId { get; set; }
        public DateTime SessionDate { get; set; }
    }
}
