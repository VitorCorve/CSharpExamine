using CSharpExamine.Infrastructure;
using CSharpExamine.Models;
using CSharpExamine.Services;
using CSharpExamine.Views;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CSharpExamine.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            randomizer = new();
            Questions = new ObservableCollection<QuestionItem>();
            handler = new();
            Restore.Execute(null);
        }

        private readonly Random randomizer;

        private readonly XmlHandler handler;

        private ObservableCollection<QuestionItem> _questions;

        private QuestionItem _selectedQuestion;

        public ObservableCollection<QuestionItem> Questions
        {
            get => _questions;
            set => Set(ref _questions, value);
        }

        public QuestionItem SelectedQuestion
        {
            get => _selectedQuestion;
            set
            {
                HideExploration();
                Set(ref _selectedQuestion, value);
            }
        }

        private Command _exploreAnswer,
                        _nextQuestion,
                        _openCreate,
                        _openEdit,
                        _createQuestion,
                        _editQuestion,
                        _deleteQuestion,
                        _confirmDelete,
                        _restore;

        public Command ExploreAnswer => _exploreAnswer ??= new Command(obj =>
        {
            SelectedQuestion.Explored = true;
        }, canExecute => SelectedQuestion != null && !SelectedQuestion.Explored);

        public Command NextQuestion => _nextQuestion ??= new Command(obj =>
        {
            if (SelectedQuestion != null)
                SelectedQuestion.Explored = false;

            SelectedQuestion = Questions[randomizer.Next(0, Questions.Count)];
        }, canExecute => Questions != null && Questions.Count > 0);

        public Command CreateQuestion => _createQuestion ??= new Command(async obj =>
        {
            if (obj is Question question)
            {
                QuestionItem item = new(question);
                Questions.Add(item);

                var ordered = new List<Question>(Questions.OrderBy(x => x.Question.Theme).Select(x => x.Question));

                Questions.Clear();

                foreach (var qst in ordered)
                {
                    Questions.Add(new QuestionItem(qst));
                }

                await handler.Save(Questions.Select(x => x.Question).ToList());
            }
        });

        public Command OpenCreate => _openCreate ??= new Command(obj =>
        {
            ActionWindow window = new(
                ACTION_TYPE.Create,
                this,
                Questions is null ? Array.Empty<string>() : Questions.Select(x => x.Question.Theme).Distinct().ToArray());
            window.Show();
        });
        public Command OpenEdit => _openEdit ??= new Command(obj =>
        {
            ActionWindow window = new(
                ACTION_TYPE.Edit,
                this,
                SelectedQuestion.Question,
                Questions is null ? Array.Empty<string>() : Questions.Select(x => x.Question.Theme).Distinct().ToArray());
            window.Show();
        }, canExecute => SelectedQuestion != null);

        public Command EditQuestion => _editQuestion ??= new Command(async obj =>
        {
            if (obj is Question question)
            {
                var itemToEdit = Questions.FirstOrDefault(x => x.Question.Identifier.Equals(question.Identifier));

                if (itemToEdit != null)
                {
                    Questions.Remove(itemToEdit);
                }

                var ordered = new List<Question>(Questions.OrderBy(x => x.Question.Theme).Select(x => x.Question));

                Questions.Clear();

                foreach (var item in ordered)
                {
                    Questions.Add(new QuestionItem(item));
                }

                Questions.Add(new QuestionItem(question));

                await handler.Save(Questions.Select(x => x.Question).ToList());
            }
        }, canExecute => SelectedQuestion != null);

        public Command DeleteQuestion => _deleteQuestion ??= new Command(async obj =>
        {
            if (obj is Question question)
            {
                var itemToRemove = Questions.FirstOrDefault(x => x.Question.Identifier.Equals(question.Identifier)); if (itemToRemove != null)
                {
                    Questions.Remove(itemToRemove);
                }

                var ordered = new List<Question>(Questions.OrderBy(x => x.Question.Theme).Select(x => x.Question));

                Questions.Clear();

                foreach (var item in ordered)
                {
                    Questions.Add(new QuestionItem(item));
                }

                await handler.Save(Questions.Select(x => x.Question).ToList());
            }
        }, canExecute => SelectedQuestion != null);

        public Command Restore => _restore ??= new Command(async obj =>
        {
            var data = await handler.Restore();

            if (data != null && data.Any())
            {
                data = data.OrderBy(x => x.Theme).ToList();
                foreach (var question in data)
                {
                    Questions.Add(new QuestionItem(question));
                }
            }
        });
        public Command ConfirmDelete => _confirmDelete ??= new Command(async obj =>
        {
            ConfirmActionWindow delete = new(ACTION_TYPE.Delete, this, SelectedQuestion.Question);
            delete.Show();
        }, canExecute => SelectedQuestion != null);

        private void HideExploration()
        {
            if (SelectedQuestion is null)
            {
                return;
            }
            else
            {
                SelectedQuestion.Explored = false;
            }
        }
    }
}
