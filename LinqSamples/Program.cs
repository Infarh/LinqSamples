using System;
using System.CodeDom;
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

            IEnumerable<Student> students = GetStudents(names_file).ToArray();

            var avg_ratings = students.Select(s => new
            {
                Student = s,
                AvgRating = s.Ratings.Average()
            });

            foreach (var value in avg_ratings)
                Console.WriteLine("{0} {1} {2} -- {3}",
                    value.Student.SurName,
                    value.Student.Name,
                    value.Student.Patronymic,
                    value.AvgRating);

            Console.WriteLine();
            Console.WriteLine("Students count: {0}", students.Count());

            var best_student = students.Where(s => s.Ratings.Average() >= 4);
            Console.WriteLine("Best students count: {0}", best_student.Count());

            var other_students = students.Where(s => s.Ratings.Average() < 4);
            Console.WriteLine("Other students count: {0}", other_students.Count());

            Console.WriteLine();
            Console.WriteLine(new string('-', Console.BufferWidth));
            var arranged_students = students.OrderBy(s => s.Ratings.Average());
            foreach (var value in arranged_students)
                Console.WriteLine("{0} {1} {2} -- {3}",
                    value.SurName,
                    value.Name,
                    value.Patronymic,
                    value.Ratings.Average());

            var groups = Enumerable.Range(1, __GroupCount)
               .Select(i => new Group
                {
                    Id = i,
                    Name = $"Группа {i}"
                });

            var student_groups = students.Join(
                groups,
                student => student.GroupId,
                group => group.Id,
                (student, group) => new { Student = student, Group = group });

            Console.WriteLine();
            Console.WriteLine(new string('-', Console.BufferWidth));
            foreach (var info in student_groups)
                Console.WriteLine("{0} {1} {2} - {3}",
                    info.Student.SurName,
                    info.Student.Name,
                    info.Student.Patronymic,
                    info.Group.Name);

            var group_of_students = student_groups.GroupBy(info => info.Group.Name);
            var student_dict_of_groups = group_of_students.ToDictionary(
                g => g.Key,
                g => g.ToArray());

            var students_of_group7 = student_dict_of_groups["Группа 7"];
            Console.WriteLine();
            Console.WriteLine(new string('-', Console.BufferWidth));
            foreach (var info in students_of_group7)
                Console.WriteLine("{0} {1} {2} - {3}",
                    info.Student.SurName,
                    info.Student.Name,
                    info.Student.Patronymic,
                    info.Group.Name);

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
                    for (int i = 0, count = rnd.Next(1, 51); i < count; i++)
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
