using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sekai.Database.Models
{
    public class TimelineEntry
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsMentions { get; set; }

        public long AccountUserId { get; set; }

        public string UserName { get; set; }

        public string TrendTag { get; set; }
    }
}
