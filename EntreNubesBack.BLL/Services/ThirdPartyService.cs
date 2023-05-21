using AutoMapper;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.DTO;
using EntreNubesBack.Models;
using Microsoft.EntityFrameworkCore;

namespace EntreNubesBack.BLL.Services;

public class ThirdPartyService : IThirdPartyService
{
    private readonly IThirdPartyRepository _thirdPartyRepository;
    private readonly IGenericRepository<Person> _personRepository;
    private readonly IMapper _mapper;

    public ThirdPartyService(IThirdPartyRepository thirdPartyRepository, IMapper mapper, IGenericRepository<Person> personRepository)
    {
        _thirdPartyRepository = thirdPartyRepository;
        _mapper = mapper;
        _personRepository = personRepository;
    }

    public async Task<List<ThirdPartyDto>> List()
    {
        var thirdParties = await _thirdPartyRepository.Consult(tp => tp.State);
        var aux = thirdParties.Include(tp => tp.IdPersonNavigation);
        return _mapper.Map<List<ThirdPartyDto>>(aux);
    }

    public async Task<ThirdPartyDto> Create(ThirdPartyDto data)
    {
        try
        {
            var aux = await _thirdPartyRepository.Get(tp => tp.Nit == data.Nit || tp.BusinessName == data.BusinessName || tp.CompanyName == data.CompanyName);
            if (aux != null)
                throw new TaskCanceledException("Uno o varios datos ingresados ya estan relacionados con un tercero"); ;
            var newThirdParty = _mapper.Map<ThirdParty>(data);
            newThirdParty.State = true;
            var result = await _thirdPartyRepository.Create(newThirdParty);
            return _mapper.Map<ThirdPartyDto>(result);
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> Edit(ThirdPartyDto dataToEdit)
    {
        try
        {
            var aux = await _thirdPartyRepository.Get(tp => tp.IdThirdParty != dataToEdit.IdThirdParty 
                                                            && (tp.Nit == dataToEdit.Nit || 
                                                            tp.BusinessName == dataToEdit.BusinessName || 
                                                            tp.CompanyName == dataToEdit.CompanyName));
            if (aux != null)
                throw new TaskCanceledException("Uno o varios datos ingresados ya estan relacionados con un tercero"); ;
            var thirdPartyToEdit = await _thirdPartyRepository.Get(tp => tp.IdThirdParty == dataToEdit.IdThirdParty);
            if (thirdPartyToEdit == null)
                throw new TaskCanceledException("No existe el tercero");
        
            thirdPartyToEdit.Address = dataToEdit.Address;
            thirdPartyToEdit.Category = dataToEdit.Category;
            thirdPartyToEdit.Nit = dataToEdit.Nit;
            thirdPartyToEdit.Phone = dataToEdit.Phone;
            thirdPartyToEdit.BusinessName = dataToEdit.BusinessName;
            thirdPartyToEdit.CompanyName = dataToEdit.CompanyName;
            thirdPartyToEdit.ProductServiceName = dataToEdit.ProductServiceName;

            bool response = await _thirdPartyRepository.Edit(thirdPartyToEdit);
            return response;
        }
        catch
        {
            throw;
        }
    }
    
    public async Task<bool> Delete(int idThirdParty)
    {
        var thirdParty = await _thirdPartyRepository.Get(tp => tp.IdThirdParty == idThirdParty);
        if (thirdParty == null)
            throw new TaskCanceledException("El tercero no existe");
        thirdParty.State = false;
        var result = await _thirdPartyRepository.Edit(thirdParty); 
        return result;
    }
    
    public async Task<bool> AddAdvisorToThirdParty(AddAdvisorToThirdPartyDto data)
    {
        try
        {
            var advisor = await _personRepository.Get(p => p.DocumentType == data.AdvisorInfo.DocumentType &&
                                                           p.DocumentNumber == data.AdvisorInfo.DocumentNumber);
            if (advisor != null)
                throw new TaskCanceledException("El documento ingresado ya existe");
        
            var newAdvisor = await _personRepository.Create(new Person()
            {
                DocumentType = data.AdvisorInfo.DocumentType,
                DocumentNumber = data.AdvisorInfo.DocumentNumber,
                PersonName = data.AdvisorInfo.PersonName,
                PhoneNumber = data.AdvisorInfo.PhoneNumber
            });

            var thirdParty = await _thirdPartyRepository.Get(tp => tp.IdThirdParty == data.ThirdPartyId);
            if (thirdParty == null)
                throw new TaskCanceledException("El tercero no existe");
            thirdParty.IdPerson = newAdvisor.IdPerson;
            bool result = await _thirdPartyRepository.Edit(thirdParty);
            if (!result)
                throw new TaskCanceledException("No se pudo relacionar el asesor al tercero");
            return result;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> EditAdvisorInfo(PersonDto advisorInfo)
    {
        try
        {
            var advisor = await _personRepository.Get(p => p.IdPerson == advisorInfo.IdPerson);
            if (advisor == null)
                throw new TaskCanceledException("No existe el asesor");
            var verifyDocument = await _personRepository.Get(p => p.DocumentType.Trim() == advisorInfo.DocumentType
                                                                  && p.DocumentNumber.Trim() == advisorInfo.DocumentNumber 
                                                                  && p.IdPerson != advisor.IdPerson);
            if (verifyDocument != null)
                throw new TaskCanceledException("El documento ingresado ya pertenece a otra persona");
            advisor.DocumentType = advisorInfo.DocumentType;
            advisor.DocumentNumber = advisorInfo.DocumentNumber;
            advisor.PersonName = advisorInfo.PersonName;
            advisor.PhoneNumber = advisorInfo.PhoneNumber;
            bool result = await _personRepository.Edit(advisor);
            if (!result)
                throw new TaskCanceledException("No se ha podido editar la informacion del asesor");
            return result;
        }
        catch
        {
            throw;
        }
    }

    public async Task<bool> DeleteAdvisorFromThirdParty(int idThirdParty, int advisorId)
    {
        try
        {
            var advisor = await _personRepository.Get(p => p.IdPerson == advisorId);
            if (advisor == null)
                throw new TaskCanceledException("No existe el asesor");
            var thirdParty = await _thirdPartyRepository.Get(tp => tp.IdThirdParty == idThirdParty);
            if(thirdParty == null)
                throw new TaskCanceledException("No existe el tercero");
            thirdParty.IdPerson = null;
            var result = await _thirdPartyRepository.Edit(thirdParty);
            if (!result)
                throw new TaskCanceledException("No se pudo eliminar el asesor");
            return result;
        }
        catch
        {
            throw;
        }
    }
}