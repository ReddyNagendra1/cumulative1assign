﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cumulative1assign.Models
{
    public class Teacher
    {
        //The following properties define an Author
        public int TeacherId;
        public string TeacherFname;
        public string TeacherLname;
        public string EmployeeNumber;
        public DateTime HireDate;
        public decimal Salary;

        //parameter-less constructor function

        public Teacher() { }
    }
}
