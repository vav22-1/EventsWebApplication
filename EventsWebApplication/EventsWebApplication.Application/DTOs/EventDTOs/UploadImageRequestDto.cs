﻿namespace EventsWebApplication.Application.DTOs.EventDTOs
{
    public class UploadImageRequestDto
    {
        public int EventId { get; set; }
        public byte[] ImageData { get; set; }
    }
}
