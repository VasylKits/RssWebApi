using System.ComponentModel.DataAnnotations;

namespace RssManagementWebApi.DTOs;

public class SearchModel
{
    [Required]
    public DateTime Date { get; set; }
}