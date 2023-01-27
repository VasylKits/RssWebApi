namespace RssManagementWebApi.Services.Interfaces;

public interface IBaseResponse<T>
{
    T Response { get; set; }
    public bool IsError { get; set; }
}