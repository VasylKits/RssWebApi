using RssManagementWebApi.DTOs;

namespace RssManagementWebApi.Services.Interfaces;

public interface IFeedsService
{
    Task<IBaseResponse<List<FeedResponse>>> GetAllFeedsAsync();
    Task<IBaseResponse<string>> LoadFeedsAsync(LoadModel model);
    Task<IBaseResponse<List<FeedResponse>>> GetFromDateAsync(SearchModel model);
    Task<IBaseResponse<FeedResponse>> SetAsReadAsync(string id);
    Task<IBaseResponse<string>> DeleteFeedsAsync();
}