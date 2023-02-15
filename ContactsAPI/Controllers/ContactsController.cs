using ContactsAPI.Data;
using ContactsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace ContactsAPI.Controllers
{
    [ApiController]
    //[Route("api/Contacts")]
    [Route("api/[Controller]")]
    public class ContactsController : Controller
    {
        private readonly ContactsAPIDbContext _context;

        public ContactsController(ContactsAPIDbContext db) 
        {
            _context = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetContact()
        {
          return Ok(await _context.contacts.ToListAsync());
            return View();
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetContact([FromRoute] Guid id)
        {
            var contact = await _context.contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(AddContactRequest addContactRequest)
        {
            var contact = new Contact()
            {
                Id = Guid.NewGuid(),
                Address = addContactRequest.Address,
                Email = addContactRequest.Email,
                Phone = addContactRequest.Phone,
                FullName = addContactRequest.FullName

            };
           await _context.contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
            return Ok( contact );
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateContact([FromRoute] Guid id, UpdateContactRequest updateContactRequest)
        {
            var contact = await _context.contacts.FindAsync(id);
            if(contact !=null)
            {
                contact.FullName = updateContactRequest.FullName;
                contact.Address = updateContactRequest.Address;
                contact.Phone = updateContactRequest.Phone;
                contact.Email = updateContactRequest.Email;

                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound(); 
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteContact([FromRoute] Guid id)
        {
            var contact = await _context.contacts.FindAsync(id);
            if(contact!=null)
            {
                _context.Remove(contact);
                await _context.SaveChangesAsync();
                return Ok(contact);
            }
            return NotFound();
        }
    }
}
