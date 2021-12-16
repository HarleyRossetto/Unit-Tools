namespace MQHandbookAPI.Models.Macquarie.Handbook.Data.Shared;

using System;
using Newtonsoft.Json;

public class MacquarieMetadataDTO
{
    public DateTime DateRetrieved { get; init; } = DateTime.Now;
    public DateTime? ModificationDate { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public ushort ImplementationYear { get; set; }
    public string StudyLevel { get; set; }
    public string ContentTypeLabel { get; set; }
    public bool Archived { get; set; }
    public bool Working { get; set; }
    public bool Locked { get; set; }
    public bool Live { get; set; }
    public string UrlMapForContent { get; set; }
}
