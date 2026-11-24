using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstract;
using SignalR.DtoLayer.Dtos.ContactDtos;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateContactDto> _createValidator;
        private readonly IValidator<UpdateContactDto> _updateValidator;

        public ContactsController(IContactService ContactService, IMapper mapper, IValidator<CreateContactDto> createValidator, IValidator<UpdateContactDto> updateValidator)
        {
            _contactService = ContactService;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }
        [HttpGet]
        public async Task<IActionResult> ContactList()
        {
            var values = await _contactService.TGetAllAsync();
            var Contacts = _mapper.Map<List<ResultContactDto>>(values);
            return Ok(Contacts);
        }
        [HttpGet("GetContactById/{id}")]
        public async Task<IActionResult> GetContactById(int id)
        {
            var values = await _contactService.TGetByIdAsync(id);
            var Contacts = _mapper.Map<ResultContactDto>(values);
            return Ok(Contacts);
        }
        [HttpPost]
        public async Task<IActionResult> CreateContact(CreateContactDto dto)
        {
            var result = await _createValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var values = _mapper.Map<Contact>(dto);
            await _contactService.TCreateAsync(values);
            return Ok("Ekleme işlemi başarılı.");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            await _contactService.TDeleteAsync(id);
            return Ok("Silme işlemi başarılı.");
        }
        [HttpPut]
        public async Task<IActionResult> UpdateContact(UpdateContactDto dto)
        {
            var result = await _updateValidator.ValidateAsync(dto);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));

            var values = _mapper.Map<Contact>(dto);
            await _contactService.TUpdateAsync(values);
            return Ok("Güncelleme işlemi başarılı.");
        }
    }
}
