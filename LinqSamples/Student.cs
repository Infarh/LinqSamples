using System;
using System.Collections.Generic;

namespace LinqSamples
{
    class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string SurName { get; set; }

        public string Patronymic { get; set; }

        public DateTime DayOfBirth { get; set; }

        public List<int> Ratings { get; set; } = new List<int>();

        public int GroupId { get; set; }
    }
}
