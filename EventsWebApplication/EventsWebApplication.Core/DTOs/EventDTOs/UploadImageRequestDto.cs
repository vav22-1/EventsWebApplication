using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsWebApplication.Core.DTOs.EventDTOs
{
    public class UploadImageRequestDto
    {
        public int EventId { get; set; }
        public byte[] ImageData { get; set; }
    }
}
