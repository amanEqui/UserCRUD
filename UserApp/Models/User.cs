using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserApp.Models
{
    public class User
    {
        
            public int UserID { get; set; }
            public string UserName { get; set; }
            public string UserPass { get; set; }
            public string DOB { get; set; }
            public string Gender { get; set; }
            public string Status { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public List<Skill> Skills { get; set; }

        public User()
        {
            Skills = new List<Skill>();
        }


    }

    
}