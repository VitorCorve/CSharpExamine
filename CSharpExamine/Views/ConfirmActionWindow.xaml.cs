using CSharpExamine.Infrastructure;
using CSharpExamine.Models;
using CSharpExamine.ViewModels;

using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CSharpExamine.Views
{
    /// <summary>
    /// Логика взаимодействия для ConfirmActionWindow.xaml
    /// </summary>
    public partial class ConfirmActionWindow : Window
    {
        private readonly MainWindowViewModel viewModel;
        private readonly ACTION_TYPE action;
        private readonly Question question;
        private Window caller;
        public ConfirmActionWindow()
        {
            InitializeComponent();
        }

        public ConfirmActionWindow(ACTION_TYPE action, MainWindowViewModel viewModel, Question question)
        {
            InitializeComponent();

            this.viewModel = viewModel;
            this.action = action;
            this.question = question;

            Message.Text = action switch
            {
                ACTION_TYPE.Create => "Create Question?",
                ACTION_TYPE.Edit => "Edit Question?",
                ACTION_TYPE.Delete => "Delete Question?",
                _ => "Create Question?",
            };

            Title = "Confirmation";
        }

        public void SetCaller(Window caller)
        {
            this.caller = caller;
        }

        private void ClickYes(object sender, RoutedEventArgs e)
        {
            switch (action)
            {
                case ACTION_TYPE.Create:
                    viewModel.CreateQuestion.Execute(question);
                    break;
                case ACTION_TYPE.Edit:
                    viewModel.EditQuestion.Execute(question);
                    break;
                case ACTION_TYPE.Delete:
                    viewModel.DeleteQuestion.Execute(question);
                    break;
                default:
                    break;
            }

            if (caller != null)
            {
                caller.Close();
            }

            this.Close();
        }

        private void ClickNo(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
