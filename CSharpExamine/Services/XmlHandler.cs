using CSharpExamine.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CSharpExamine.Services
{
    public class XmlHandler
    {
        private const string Storage = "Data";

        public XmlHandler()
        {
            Directory.CreateDirectory(GetPath());
        }

        public async Task<ICollection<Question>> Restore()
        {
            string[] files = Directory.GetFiles(GetPath());

            if (files.Any())
            {
                string file = files.FirstOrDefault(x => x.Contains("Questions.xml"));

                if (string.IsNullOrEmpty(file))
                {
                    return null;
                }
                else
                {
                    XmlSerializer serializer = new(typeof(QuestionsXml));

                    try
                    {
                        using FileStream stream = new(Path.Combine(GetPath(), "Questions.xml"), FileMode.OpenOrCreate);
                        QuestionsXml? questions = serializer.Deserialize(stream) as QuestionsXml;

                        return questions.Questions;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
        }

        public async Task Save(List<Question> questions)
        {
            XmlSerializer serializer = new(typeof(QuestionsXml));

            QuestionsXml xml = new()
            {
                Questions = questions
            };

            using FileStream stream = new(Path.Combine(GetPath(), "Questions.xml"), FileMode.OpenOrCreate);

            serializer.Serialize(stream, xml);
        }

        private string GetPath() => Path.Combine(Environment.CurrentDirectory, Storage);
    }
}
