using ContactListApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContactListApi.Controllers
{
    [Route("/contacts")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactsRepository repository;

        public ContactsController(IContactsRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet(Name = nameof(GetAllPeople))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Contact>))]
        [Produces("application/json")]
        public IActionResult GetAllPeople() => Ok(repository.GetAll());

        [HttpPost(Name = nameof(AddPerson))]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Contact))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [Produces("application/json")]
        public IActionResult AddPerson([Required, FromBody] Contact newPerson)
        {
            if (newPerson.Id < 1)
            {
                return BadRequest("Invalid id");
            }
            if (newPerson.Email == null || newPerson.Email.Length == 0)
            {
                return BadRequest($"Required field {nameof(newPerson.Email)} empty or missing");
            }

            repository.Add(newPerson);
            return CreatedAtAction(nameof(DeletePerson), new { id = newPerson.Id }, newPerson);
        }

        [HttpDelete("{personId}", Name = nameof(DeletePerson))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult DeletePerson([Required, FromRoute] int personId)
        {
            if (personId < 0)
            {
                return BadRequest("Invalid ID supplied");
            }
            try
            {
                repository.Delete(personId);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            return NoContent();
        }

        [HttpGet("findByName", Name = nameof(FindPersonByName))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Contact>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [Produces("application/json")]
        public IActionResult FindPersonByName([Required, FromQuery(Name = "nameFilter")] string nameFilter)
        {
            if (nameFilter.Length == 0)
                return BadRequest("Invalid or missing name");

            var peopleFilteredByName = repository.GetByName(nameFilter);
            return Ok(peopleFilteredByName);
        }
    }
}
