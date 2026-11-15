using ChristmasQuiz.Models;
using System.Collections.Concurrent;

namespace ChristmasQuiz.Data
{
    public static class QuizData
    {
        public static readonly List<QuestionItem> Questions = new()
        {
            new QuestionItem
            {
                Id = 1,
                Text = "Яка українська різдвяна пісня стала відомою у світі як Carol of the Bells?",
                Options = new List<string> { "Добрий вечір тобі", "Щедрик", "Нова радість стала", "Тиха ніч" },
                CorrectOptionIndex = 1
            },
            new QuestionItem
            {
                Id = 2,
                Text = "Скільки традиційних пісних страв готують українці на Святвечір?",
                Options = new List<string> { "7 страв", "9 страв", "12 страв", "15 страв" },
                CorrectOptionIndex = 2
            },
            new QuestionItem
            {
                Id = 3,
                Text = "Як називається головна різдвяна страва з пшениці, меду та маку?",
                Options = new List<string> { "Узвар", "Кутя", "Коровай", "Пампухи" },
                CorrectOptionIndex = 1
            },
            new QuestionItem
            {
                Id = 4,
                Text = "Що символізує різдвяний Дідух у хаті?",
                Options = new List<string> { "Прихід весни", "Дух предків, добробут і врожай", "Захист від холоду", "Зірочку на небі" },
                CorrectOptionIndex = 1
            },
            new QuestionItem
            {
                Id = 5,
                Text = "Хто створив класичну хорову обробку Щедрика?",
                Options = new List<string> { "Микола Лисенко", "Микола Леонтович", "Кирило Стеценко", "Олександр Кошиць" },
                CorrectOptionIndex = 1
            },
            new QuestionItem
            {
                Id = 6,
                Text = "Який головний атрибут носять колядники попереду під час ходи?",
                Options = new List<string> { "Різдвяну зірку", "Ялинкову гілку", "Дзвіночок", "Ліхтар" },
                CorrectOptionIndex = 0
            },
            new QuestionItem
            {
                Id = 7,
                Text = "У якій країні зародилася традиція прикрашати різдвяну ялинку?",
                Options = new List<string> { "Франція", "Велика Британія", "Німеччина", "Італія" },
                CorrectOptionIndex = 2
            }
        };

        public static ConcurrentDictionary<string, QuizSessionData> Sessions { get; } = new();
    }
}
