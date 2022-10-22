using CSharpExamine.ViewModels;

using System;

namespace CSharpExamine.Models
{
    public class QuestionItem : ViewModelBase
    {
        private bool _explored;
        
        private Question _question;
        public Question Question
        {
            get => _question;
            set => Set(ref _question, value);
        }
        public bool Explored
        {
            get => _explored;
            set => Set(ref _explored, value);
        }

        public QuestionItem(Question question)
        {
            Question = question;
        }
    }
}
