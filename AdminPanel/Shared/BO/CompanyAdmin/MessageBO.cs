using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class MessageBO : CommonAuditFields
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public long? GroupId { get; set; }
        public bool IsSms { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        public string? MessageText { get; set; }
        public IEnumerable<long>? CustomerIds { get; set; }
    }
}
