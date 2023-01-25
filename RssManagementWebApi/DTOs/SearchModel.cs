using System.ComponentModel.DataAnnotations;

namespace RssManagementWebApi.DTOs;

public class SearchModel
{
    [Required]
    public DateTime Data { get; set; }
}