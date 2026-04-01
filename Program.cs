
        List<Student> students = new List<Student>
        {
            new Student { Id = 1, Name = "Alice", DateOfBirth = new DateTime(2000, 5, 15) },
            new Student { Id = 2, Name = "Bob", DateOfBirth = new DateTime(1999, 8, 25) },
            new Student { Id = 3, Name = "Charlie", DateOfBirth = new DateTime(2001, 3, 10) },
            new Student { Id = 4, Name = "David", DateOfBirth = new DateTime(2000, 1, 1) },
            new Student { Id = 5, Name = "Eve", DateOfBirth = new DateTime(1998, 11, 30) },
            new Student { Id = 6, Name = "Frank", DateOfBirth = new DateTime(2002, 7, 7) }
        };

        List<Course> courses = new List<Course>
        {
            new Course { Id = 101, Title = "Mathematics", Credits = 4 },
            new Course { Id = 102, Title = "Computer Science", Credits = 3 },
            new Course { Id = 103, Title = "Physics", Credits = 4 },
            new Course { Id = 104, Title = "Chemistry", Credits = 3 }
        };

        List<Enrollment> enrollments = new List<Enrollment>
        {
            new Enrollment { Id = 1, StudentId = 1, CourseId = 101, EnrollmentDate = new DateTime(2023, 1, 15) },
            new Enrollment { Id = 2, StudentId = 1, CourseId = 102, EnrollmentDate = new DateTime(2023, 1, 20) },
            new Enrollment { Id = 3, StudentId = 2, CourseId = 101, EnrollmentDate = new DateTime(2023, 1, 18) },
            new Enrollment { Id = 4, StudentId = 3, CourseId = 103, EnrollmentDate = new DateTime(2023, 1, 22) },
            new Enrollment { Id = 5, StudentId = 3, CourseId = 101, EnrollmentDate = new DateTime(2023, 1, 25) },
            new Enrollment { Id = 6, StudentId = 3, CourseId = 102, EnrollmentDate = new DateTime(2023, 1, 30) },
            new Enrollment { Id = 7, StudentId = 4, CourseId = 104, EnrollmentDate = new DateTime(2023, 2, 1) },
            new Enrollment { Id = 8, StudentId = 5, CourseId = 101, EnrollmentDate = new DateTime(2023, 2, 5) },
            new Enrollment { Id = 9, StudentId = 5, CourseId = 103, EnrollmentDate = new DateTime(2023, 2, 10) },
            new Enrollment { Id = 10, StudentId = 6, CourseId = 102, EnrollmentDate = new DateTime(2023, 2, 15) }
        };

        Console.WriteLine("Task 1: Find all students who are enrolled in the \"Mathematics\" course.");
        var studentsInMath = from s in students
                             join e in enrollments on s.Id equals e.StudentId
                             join c in courses on e.CourseId equals c.Id
                             where c.Title == "Mathematics"
                             select s;
        foreach (var student in studentsInMath.Distinct())
        {
            Console.WriteLine($"- {student.Name}");
        }
        Console.WriteLine();

        Console.WriteLine("Task 2: Find all courses that \"Charlie\" is enrolled in.");
        var charlieCourses = from c in courses
                             join e in enrollments on c.Id equals e.CourseId
                             join s in students on e.StudentId equals s.Id
                             where s.Name == "Charlie"
                             select c;
        foreach (var course in charlieCourses.Distinct())
        {
            Console.WriteLine($"- {course.Title}");
        }
        Console.WriteLine();

        Console.WriteLine("Task 3: Using SelectMany with Hierarchical Data: Flatten a list of enrollments and select students who are enrolled in multiple courses.");
        var studentsInMultipleCourses = enrollments.GroupBy(e => e.StudentId)
                                                .Where(g => g.Count() > 1)
                                                .SelectMany(g => students.Where(s => s.Id == g.Key));
        foreach (var student in studentsInMultipleCourses.Distinct())
        {
            Console.WriteLine($"- {student.Name}");
        }
        Console.WriteLine();

        Console.WriteLine("Task 4: Group students by age range and then by course, and calculate the average age for each group");
        var groupedStudents = from s in students
                              let age = DateTime.Now.Year - s.DateOfBirth.Year
                              let ageRange = age < 20 ? "<20" : (age >= 20 && age < 25 ? "20-24" : ">=25")
                              join e in enrollments on s.Id equals e.StudentId
                              join c in courses on e.CourseId equals c.Id
                              group new { s, age } by new { ageRange, c.Title } into g
                              select new
                              {
                                  AgeRange = g.Key.ageRange,
                                  Course = g.Key.Title,
                                  AverageAge = g.Average(x => x.age)
                              };
        foreach (var group in groupedStudents)
        {
            Console.WriteLine($"Age Range: {group.AgeRange}, Course: {group.Course}, Average Age: {group.AverageAge:F2}");
        }
        Console.WriteLine();

        Console.WriteLine("Task 5: Join Student, Enrollment, and Course and filter by enrollment date and course credits.");
        var filteredEnrollments = from s in students
                                  join e in enrollments on s.Id equals e.StudentId
                                  join c in courses on e.CourseId equals c.Id
                                  where e.EnrollmentDate > new DateTime(2023, 1, 20) && c.Credits >= 4
                                  select new { StudentName = s.Name, CourseTitle = c.Title, e.EnrollmentDate, c.Credits };
        foreach (var item in filteredEnrollments)
        {
            Console.WriteLine($"Student: {item.StudentName}, Course: {item.CourseTitle}, Date: {item.EnrollmentDate.ToShortDateString()}, Credits: {item.Credits}");
        }
        Console.WriteLine();

        Console.WriteLine("Task 6: Calculate the total credits each student has enrolled in.");
        var studentTotalCredits = from s in students
                                  join e in enrollments on s.Id equals e.StudentId
                                  join c in courses on e.CourseId equals c.Id
                                  group c.Credits by s.Name into g
                                  select new
                                  {
                                      StudentName = g.Key,
                                      TotalCredits = g.Sum()
                                  };
        foreach (var item in studentTotalCredits)
        {
            Console.WriteLine($"Student: {item.StudentName}, Total Credits: {item.TotalCredits}");
        }
        Console.WriteLine();

        Console.WriteLine("Task 7: Find the number of students enrolled in each course.");
        var studentsPerCourse = from c in courses
                                join e in enrollments on c.Id equals e.CourseId
                                group e by c.Title into g
                                select new
                                {
                                    CourseTitle = g.Key,
                                    StudentCount = g.Select(x => x.StudentId).Distinct().Count()
                                };
        foreach (var item in studentsPerCourse)
        {
            Console.WriteLine($"Course: {item.CourseTitle}, Number of Students: {item.StudentCount}");
        }
        Console.WriteLine();

        Console.WriteLine("Task 8: Find all courses that a specific student, say \"Bob\", is not enrolled in.");
        var bob = students.FirstOrDefault(s => s.Name == "Bob");
        if (bob != null)
        {
            var bobEnrolledCourseIds = enrollments.Where(e => e.StudentId == bob.Id).Select(e => e.CourseId).ToList();
            var coursesBobNotIn = courses.Where(c => !bobEnrolledCourseIds.Contains(c.Id));
            foreach (var course in coursesBobNotIn)
            {
                Console.WriteLine($"- {course.Title}");
            }
        }
        else
        {
            Console.WriteLine("Bob not found.");
        }
        Console.WriteLine();

