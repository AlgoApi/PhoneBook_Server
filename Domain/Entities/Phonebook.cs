using PhoneBook.Abstractions;

namespace PhoneBook.Domain.Entities
{
    public class Phonebook : BaseEntity
    {
        public Guid UserId { get; set; } // Внешний ключ
        public string Photo { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Group { get; set; }
        public string Description { get; set; }

        public User? User { get; set; }
    }
}
