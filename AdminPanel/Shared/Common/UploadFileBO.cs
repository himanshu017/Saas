using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Shared
{
    public class UploadFileBO
    {
        public string? FileName { get; set; }
        public byte[]? FileContent { get; set; }
        public bool KeepExisting { get; set; }
    }
}
