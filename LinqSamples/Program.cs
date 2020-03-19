using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LinqSamples
{
    class Program
    {
        private const int __GroupCount = 10;

        static void Main(string[] args)
        {
            // https://github.com/infarh/LinqSamples
            // http://megagenerator.ru/namefio/

            const string names_file = "Names.txt";

            var students = GetStudents(names_file);

            var student_surnames = students.Select(s => s.Name);
            foreach (var str in student_surnames)
                Console.WriteLine(str);

            Console.WriteLine("Программа завершена!");
            Console.ReadLine();
        }

        private static IEnumerable<Student> GetStudents(string FileName)
        {
            var rnd = new Random();

            var index = 1;
            using (var reader = File.OpenText(FileName))
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var line_elements = line.Split(' ');
                    if (line_elements.Length != 3) continue;

                    var name = line_elements[0];
                    var sur_name = line_elements[1];
                    var patronymic = line_elements[2];

                    var student = new Student
                    {
                        Id = index,
                        Name = name,
                        SurName = sur_name,
                        Patronymic = patronymic
                    };
                    index++;
                    for (int i = 0, count = rnd.Next(0, 51); i < count; i++)
                        student.Ratings.Add(rnd.Next(2, 6));

                    student.DayOfBirth = new DateTime(
                        rnd.Next(1997, 2005), 
                        rnd.Next(1, 13), 
                        rnd.Next(1, 29));

                    student.GroupId = rnd.Next(1, __GroupCount + 1);

                    yield return student;
                }
        }
    }
}
