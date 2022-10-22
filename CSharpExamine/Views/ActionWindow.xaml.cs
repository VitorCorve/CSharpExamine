using CSharpExamine.Infrastructure;
using CSharpExamine.Models;
using CSharpExamine.ViewModels;

using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace CSharpExamine.Views
{
    /// <summary>
    /// Логика взаимодействия для ActionWindow.xaml
    /// </summary>
    public partial class ActionWindow : Window
    {
        private readonly MainWindowViewModel viewModel;
        private readonly ACTION_TYPE action;
        private readonly Question question;
        public ActionWindow()
        {
            InitializeComponent();
        }

        public ActionWindow(ACTION_TYPE action, MainWindowViewModel viewModel, string[] themes)
        {
            InitializeComponent();

            this.viewModel = viewModel;
            this.action = action;

            if (themes.Length > 0)
            {
                for (int i = 0; i < themes.Length; i++)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = themes[i];
                    textBlock.FontWeight = FontWeights.Bold;

                    ThemesList.Items.Add(textBlock);
                }

                ThemeText.Text = ((TextBlock)ThemesList.Items[0]).Text;
            }
            else
            {
                TextBlock textBlock = new();
                textBlock.Text = "Common";
                textBlock.FontWeight = FontWeights.Bold;

                ThemesList.Items.Add(textBlock);
                ThemeText.Text = "Common";
            }

            Title = action switch
            {
                ACTION_TYPE.Create => "Question creation",
                ACTION_TYPE.Edit => "Question edition",
                ACTION_TYPE.Delete => "Not supported",
                _ => "Unbound",
            };

            ActionButton.IsEnabled = !string.IsNullOrEmpty(QuestionText.Text) && !string.IsNullOrEmpty(AnswerText.Text);
        }

        public ActionWindow(ACTION_TYPE action, MainWindowViewModel viewModel, Question question, string[] themes)
        {
            InitializeComponent();

            this.viewModel = viewModel;
            this.action = action;
            this.question = question;

            if (themes.Length > 0)
            {
                for (int i = 0; i < themes.Length; i++)
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = themes[i];
                    textBlock.FontWeight = FontWeights.Bold;

                    ThemesList.Items.Add(textBlock);
                }

                ThemeText.Text = ((TextBlock)ThemesList.Items[0]).Text;
            }
            else
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = "Common";
                textBlock.FontWeight = FontWeights.Bold;

                ThemesList.Items.Add(textBlock);
                ThemeText.Text = "Common";
            }

            switch (action)
            {
                case ACTION_TYPE.Create:
                    ActionButton.Content = "Create";
                    Title = "Question creation";
                    break;
                case ACTION_TYPE.Edit:
                    ActionButton.Content = "Save";
                    ThemeText.Text = question.Theme;
                    Title = "Question edition";
                    break;
                case ACTION_TYPE.Delete:
                    ActionButton.Content = "Not supported";
                    Title = "Not supported";
                    break;
                default:
                    ActionButton.Content = "Unbound";
                    Title = "Unbound";
                    break;
            }

            QuestionText.Text = question.Value;
            AnswerText.Text = question.Answer;

            ActionButton.IsEnabled = !string.IsNullOrEmpty(question.Value) && !string.IsNullOrEmpty(question.Value);
        }

        private void ClickCreate(object sender, RoutedEventArgs e)
        {
            Question question = new(QuestionText.Text, AnswerText.Text, ThemeText.Text, this.question?.Identifier);

            switch (action)
            {
                case ACTION_TYPE.Create:
                    ConfirmActionWindow create = new(action, viewModel, question);
                    create.SetCaller(this);
                    create.Show();
                    break;
                case ACTION_TYPE.Edit:
                    ConfirmActionWindow edit = new(action, viewModel, question);
                    edit.SetCaller(this);
                    edit.Show();
                    break;
                case ACTION_TYPE.Delete:
                    break;
                default:
                    break;
            }
        }

        private void ClickCancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ThemesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ThemeText.Text = ThemeText.Text = ((TextBlock)ThemesList.SelectedItem).Text;
        }

        private void QuestionTextChanged(object sender, TextChangedEventArgs e)
        {
            ActionButton.IsEnabled = !string.IsNullOrEmpty(QuestionText.Text) && !string.IsNullOrEmpty(AnswerText.Text);
        }
    }
}
