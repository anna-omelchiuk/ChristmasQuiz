using ChristmasQuiz.Data;
using ChristmasQuiz.Models;
using ChristmasQuiz.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChristmasQuiz.Controllers
{
    public class QuizController : Controller
    {
        private readonly EmailSender emailSender;

        public QuizController(EmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new UserRegistrationModel());
        }

        [HttpPost]
        public IActionResult Index(UserRegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string sessionId = Guid.NewGuid().ToString();
            QuizData.Sessions[sessionId] = new QuizSessionData
            {
                User = model
            };

            return RedirectToAction(nameof(Question), new { sessionId, index = 0 });
        }

        [HttpGet]
        public IActionResult Question(string sessionId, int index = 0)
        {
            if (string.IsNullOrEmpty(sessionId) || !QuizData.Sessions.TryGetValue(sessionId, out var session))
            {
                return RedirectToAction(nameof(Index));
            }

            if (index < 0 || index >= QuizData.Questions.Count)
            {
                return RedirectToAction(nameof(Result), new { sessionId });
            }

            ViewBag.SessionId = sessionId;
            ViewBag.CurrentIndex = index;
            ViewBag.TotalQuestions = QuizData.Questions.Count;
            ViewBag.UserName = session.User.Name;

            return View(QuizData.Questions[index]);
        }

        [HttpPost]
        public async Task<IActionResult> Question(string sessionId, int index, int? selectedOption)
        {
            if (string.IsNullOrEmpty(sessionId) || !QuizData.Sessions.TryGetValue(sessionId, out var session))
            {
                return RedirectToAction(nameof(Index));
            }

            if (selectedOption == null)
            {
                ViewBag.Error = "Будь ласка, оберіть один варіант відповіді!";
                ViewBag.SessionId = sessionId;
                ViewBag.CurrentIndex = index;
                ViewBag.TotalQuestions = QuizData.Questions.Count;
                ViewBag.UserName = session.User.Name;
                return View(QuizData.Questions[index]);
            }

            var question = QuizData.Questions[index];
            var existingAnswer = session.Answers.FirstOrDefault(a => a.Question.Id == question.Id);

            if (existingAnswer != null)
            {
                existingAnswer.SelectedOptionIndex = selectedOption.Value;
            }
            else
            {
                session.Answers.Add(new UserAnswerRecord
                {
                    Question = question,
                    SelectedOptionIndex = selectedOption.Value
                });
            }

            int nextIndex = index + 1;
            if (nextIndex < QuizData.Questions.Count)
            {
                return RedirectToAction(nameof(Question), new { sessionId, index = nextIndex });
            }

            var mailResult = await emailSender.SendResultsAsync(session);
            session.EmailSent = mailResult.Success;
            session.EmailStatusMessage = mailResult.Message;

            return RedirectToAction(nameof(Result), new { sessionId });
        }

        [HttpGet]
        public IActionResult Result(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId) || !QuizData.Sessions.TryGetValue(sessionId, out var session))
            {
                return RedirectToAction(nameof(Index));
            }

            return View(session);
        }
    }
}
