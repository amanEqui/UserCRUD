using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserApp.Models
{
    public class Skill
    {
        public int SkillID { get; set; }
        public string SkillName { get; set; }
        public List<User> Users { get; set; }
    }

}