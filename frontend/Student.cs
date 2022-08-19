using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frontend
{
    internal class Student
    {
        public string Fname { get; set; }
        public string Lname { get; set; }
        public int Id { get; set; }

        public Student(string fname, string lname)
        {
            Fname = fname;
            Lname = lname;
        }
        public Student() { }
    }
}
