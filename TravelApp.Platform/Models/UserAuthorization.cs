using System.ComponentModel.DataAnnotations;

namespace TravelApp.Platform.Models
{
    public class UserAuthorization
    {
        [Required(ErrorMessage = "Обязательное поле")]
        [EmailAddress(ErrorMessage = "Некорректный Email")]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Формат: user@example.com")]
        public required string Email { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Минимум 8 символов")]
        public required string Password { get; set; }
    }
}
