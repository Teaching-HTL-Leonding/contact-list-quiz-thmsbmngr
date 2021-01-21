using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactListApi.Services
{
    public record Contact(int Id, string FirstName, string LastName, string Email);

    public class ContactRepository : IContactRepository
    {
        private List<Contact> Contacts { get; } = new();

        public Contact Add(Contact newContact)
        {
            Contacts.Add(newContact);
            return newContact;
        }

        public void Delete(int id)
        {
            var contactToDelete = GetById(id);
            if (contactToDelete == null) 
                throw new ArgumentException("No contact exists with the given id", nameof(id));

            Contacts.Remove(contactToDelete);
        }

        public IEnumerable<Contact> GetAll() => Contacts;

        public Contact GetById(int id) => Contacts.FirstOrDefault(contact => contact.Id == id);

        public IEnumerable<Contact> GetByName(string nameFilter)
        {
            var toLower = nameFilter.ToLower();
            return Contacts.Where(contact => contact.FirstName.ToLower().Contains(toLower)
                || contact.LastName.ToLower().Contains(toLower));
        }
    }
}
