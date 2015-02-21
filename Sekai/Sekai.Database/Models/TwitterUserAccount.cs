using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Entity.Relational.Model;

namespace Sekai.Database.Models
{
    public class TwitterUserAccount
    {
        public Guid Id { get; set; }

        public long UserId { get; set; }
        public string UserName { get; set; }

        public string AccountToken { get; set; }

        public string AccountSecret { get; set; }

        public string ProfileImage { get; set; }

        public bool IsPrimary { get; set; }
    }
}
