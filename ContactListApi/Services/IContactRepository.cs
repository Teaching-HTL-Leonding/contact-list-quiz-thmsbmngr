using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactListApi.Services
{
    public interface IContactRepository
    {
        Contact Add(Contact newContact);
        IEnumerable<Contact> GetAll();
        Contact GetById(int id);
        IEnumerable<Contact> GetByName(string nameFilter);
        void Delete(int id);
    }
}
