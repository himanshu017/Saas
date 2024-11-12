using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared.BO
{
    public class PostedLinksBO : CommonAuditFields
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Url is required")]
        [RegularExpression("^(http:\\/\\/www\\.|https:\\/\\/www\\.|http:\\/\\/|https:\\/\\/)?[a-z0-9]+([\\-\\.]{1}[a-z0-9]+)*\\.[a-z]{2,5}(:[0-9]{1,5})?(\\/.*)?$", ErrorMessage = "Please enter a valid Url")]
        public string? Url { get; set; }
        public bool IsActive { get; set; }
    }
}
