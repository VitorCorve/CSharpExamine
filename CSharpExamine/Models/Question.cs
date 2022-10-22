using System;

namespace CSharpExamine.Models
{
    [Serializable]
    sealed public class Question
    {
        public string Value { get; set; }
        public string Answer { get; set; }
        public string Theme { get; set; }
        public Guid Identifier { get; set; }
        public Question(string value, string answer, string theme, Guid? guid = null)
        {
            Value = value;
            Answer = answer;
            Theme = theme;
            Identifier = guid ??= Guid.NewGuid();
        }
        public Question()
        {

        }
    }
}