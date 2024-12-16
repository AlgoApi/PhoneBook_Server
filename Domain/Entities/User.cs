using PhoneBook.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneBook.Domain.Entities
{
    public class User: BaseEntity
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<Phonebook> Phonebook { get; set; } = new List<Phonebook>();

    }
}
