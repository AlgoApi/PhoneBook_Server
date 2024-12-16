using Microsoft.AspNetCore.Mvc;
using PhoneBook.Abstractions;
using PhoneBook.Domain.Entities;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PhoneBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;
        public UserController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: api/<UserController>
        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            var users = await _userRepository.GetAllAsync();
            return users;
        }

        // GET api/<UserController>/5
        [HttpPost("{get}")]
        public async Task<IActionResult> Post_get([FromBody] User entity)
        {
            var entries = await _userRepository.GetSearchAsync(e => e.Email == entity.Email && e.Password == entity.Password);
            if (entries == null || !entries.Any())
            {
                return NotFound(new { message = "No data found in the database." });
            }
            return Ok(entries.ToList()[0].Id);
        }

        // POST api/<UserController>
        // id можно генерировать последовательно
        [HttpPost]
        public async Task<User> Post([FromBody] User entity)
        {
            entity.Id = Guid.NewGuid();
            await _userRepository.AddAsync(entity);
            return entity;
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] User entity)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                return BadRequest("Пользователь не существует");
            }
            await _userRepository.UpdateAsync(user);
            return Ok();
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                return BadRequest("Пользователь не существует");
            }
            await _userRepository.DeleteAsync(id);
            return Ok();
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class PhoneBookController : ControllerBase
    {
        private readonly IRepository<Phonebook> _PhoneBookRepository;

        public PhoneBookController(IRepository<Phonebook> PhoneBookRepository)
        {
            _PhoneBookRepository = PhoneBookRepository;
        }

        [HttpGet("{userId}")]
        public async Task<IEnumerable<Phonebook>> GetPhoneBook(Guid userId)
        {
            var entries = await _PhoneBookRepository.GetSearchAsync(e => e.UserId == userId);
            //var entries = await _PhoneBookRepository.Where(e => e.UserId == userId).ToListAsync();

            return entries;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoneBook(Guid id)
        {
            var user = await _PhoneBookRepository.GetAsync(id);
            if (user == null)
            {
                return BadRequest("Пользователь не существует");
            }
            await _PhoneBookRepository.DeleteAsync(id);
            return Ok();
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhoneBook(Guid id, [FromBody] User entity)
        {
            var user = await _PhoneBookRepository.GetAsync(id);
            if (user == null)
            {
                return BadRequest("Пользователь не существует");
            }
            await _PhoneBookRepository.UpdateAsync(user);
            return Ok();
        }

        [HttpPost]
        public async Task<Phonebook> PostPhoneBook([FromBody] Phonebook entity)
        {
            entity.Id = Guid.NewGuid();
            await _PhoneBookRepository.AddAsync(entity);
            return entity;
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class PhotosController : ControllerBase
    {
        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");

        public PhotosController()
        {
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
                return BadRequest("Файл не найден");

            var randomId = new Random().Next(100000, 999999);
            var fileExtension = Path.GetExtension(photo.FileName);
            var fileName = $"{randomId}_{Path.GetFileNameWithoutExtension(photo.FileName)}{fileExtension}";
            var filePath = Path.Combine(_uploadPath, fileName);


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            return Ok(new { message = "Файл успешно загружен", path = fileName });
        }

        [HttpGet("{fileName}")]
        public IActionResult GetPhoto(string fileName)
        {
            var filePath = Path.Combine(_uploadPath, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound("Файл не найден");

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "image/jpeg");
        }
    }


}
