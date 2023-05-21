using EntreNubesBack.DTO;

namespace EntreNubesBack.BLL.Services.Contracts;

public interface IThirdPartyService
{
    Task<List<ThirdPartyDto>> List();
    Task<ThirdPartyDto> Create(ThirdPartyDto data);
    Task<bool> Edit(ThirdPartyDto dataToEdit);
    Task<bool> Delete(int idThirdParty);
    Task<bool> AddAdvisorToThirdParty(AddAdvisorToThirdPartyDto data);
    Task<bool> EditAdvisorInfo(PersonDto advisorInfo);
    Task<bool> DeleteAdvisorFromThirdParty(int idThirdParty ,int advisorId);
}