using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skillsphere.core.Entities
{
    public class TestThumbnail
    {
        public int ThumbnailId { get; set; }
        public int TestId { get; set; }
        public byte[] ImageData { get; set; } = Array.Empty<byte>();
        public DateTime UploadedAt { get; set; }
    }

}
