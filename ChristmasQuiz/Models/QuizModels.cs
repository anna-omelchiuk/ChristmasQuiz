using System.ComponentModel.DataAnnotations;

namespace ChristmasQuiz.Models
{
    public class UserRegistrationModel
    {
        [Required(ErrorMessage = "Введіть ваше ім'я")]
        [Display(Name = "Ім'я")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть ваш вік")]
        [Range(5, 120, ErrorMessage = "Вік має бути від 5 до 120 років")]
        [Display(Name = "Вік")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Введіть електронну пошту")]
        [EmailAddress(ErrorMessage = "Введіть коректну електронну пошту")]
        [Display(Name = "Електронна пошта")]
        public string Email { get; set; } = string.Empty;
    }

    public class QuestionItem
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new();
        public int CorrectOptionIndex { get; set; }
    }

    public class UserAnswerRecord
    {
        public QuestionItem Question { get; set; } = null!;
        public int SelectedOptionIndex { get; set; }
        public bool IsCorrect => Question.CorrectOptionIndex == SelectedOptionIndex;
    }

    public class QuizSessionData
    {
        public UserRegistrationModel User { get; set; } = new();
        public List<UserAnswerRecord> Answers { get; set; } = new();
        public bool EmailSent { get; set; }
        public string EmailStatusMessage { get; set; } = string.Empty;
    }
}
