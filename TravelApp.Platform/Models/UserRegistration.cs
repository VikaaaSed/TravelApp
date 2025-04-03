using System.ComponentModel.DataAnnotations;

namespace TravelApp.Platform.Models
{
    public class UserRegistration
    {
        [Required(ErrorMessage = "Обязательное поле")]
        public required string FirstName { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [EmailAddress(ErrorMessage = "Некорректный Email")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Формат: user@example.com")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        [Range(10, 90, ErrorMessage = "Недопустимый возраст")]
        public required int Age { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Минимум 8 символов")]
        public required string Password { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public required string RepeatPassword { get; set; }
    }
}
