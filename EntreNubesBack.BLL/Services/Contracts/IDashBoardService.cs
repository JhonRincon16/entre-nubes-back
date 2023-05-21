using EntreNubesBack.DTO.DashBoard;

namespace EntreNubesBack.BLL.Services.Contracts;

public interface IDashBoardService
{
    Task<DashBoardInfoDto> GetDashBoardInfo();
}