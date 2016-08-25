using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Drive.DataAccess.Entities;

namespace Drive.DataAccess.Context
{
    public class DriveDBInitializer : DropCreateDatabaseIfModelChanges<DriveContext>
    {
        protected override void Seed(DriveContext context)
        {
            User user1 = new User()
            {
                Id = 1,
                GlobalId = "56780659ea7a3b626282103d",
                IsDeleted = false
            };

            User user2 = new User()
            {
                Id = 2,
                GlobalId = "577a177413eb94e209af1ee4",
                IsDeleted = false
            };

            User user3 = new User()
            {
                Id = 3,
                GlobalId = "567921371560298f766909a7",
                IsDeleted = false
            };

            #region Binary Space
            Space space1 = new Space()
            {
                Id = 1,
                Owner = user1,
                Name = "Binary Space",
                Type = SpaceType.BinarySpace,
                Description = "General space for folders and files",
                MaxFileSize = 1073741824,
                MaxFilesQuantity = 1000,
                CreatedAt = new DateTime(2016, 1, 1, 8, 45, 33),
                LastModified = new DateTime(2016, 1, 1, 8, 45, 33),
                IsDeleted = false,
                ContentList = null
            };

            #region Folders Init
            FolderUnit folder1 = new FolderUnit()
            {
                IsDeleted = false,
                Name = "Documents",
                Description = "Helpfull documents for education",
                CreatedAt = new DateTime(2016, 1, 3, 11, 25, 33),
                LastModified = new DateTime(2016, 1, 3, 11, 25, 33),
                Owner = user1,
                Space = space1,
                DataUnits = null
            };
            FolderUnit folder2 = new FolderUnit()
            {
                IsDeleted = false,
                Name = "Other",
                Description = "Other helpfull files",
                CreatedAt = new DateTime(2016, 1, 5, 15, 23, 13),
                LastModified = new DateTime(2016, 1, 5, 15, 23, 13),
                Owner = user1,
                Space = space1,
                DataUnits = null
            };
            FolderUnit folder3 = new FolderUnit()
            {
                IsDeleted = false,
                Name = "History",
                Description = "Logs folder",
                CreatedAt = new DateTime(2016, 1, 5, 17, 13, 43),
                LastModified = new DateTime(2016, 1, 5, 17, 13, 43),
                Owner = user1,
                Space = space1,
                DataUnits = null
            };
            FolderUnit folder4 = new FolderUnit()
            {
                IsDeleted = false,
                Name = "Presentations",
                Description = "Presentations for lessons",
                CreatedAt = new DateTime(2016, 1, 6, 11, 23, 13),
                LastModified = new DateTime(2016, 1, 6, 11, 23, 13),
                Owner = user1,
                Space = space1,
                DataUnits = null
            };
            FolderUnit folder5 = new FolderUnit()
            {
                IsDeleted = false,
                Name = "Helpfull",
                Description = "Other helpfull files",
                CreatedAt = new DateTime(2016, 2, 6, 10, 23, 13),
                LastModified = new DateTime(2016, 2, 6, 10, 23, 13),
                Owner = user1,
                                Space = space1,
                DataUnits = null
            };
            FolderUnit folder6 = new FolderUnit()
            {
                IsDeleted = false,
                Name = "Hometasks",
                Description = "Hometasks for lessons",
                CreatedAt = new DateTime(2016, 2, 6, 13, 13, 45),
                LastModified = new DateTime(2016, 2, 6, 13, 13, 45),
                Owner = user1,
                
                Space = space1,
                DataUnits = null
            };
            FolderUnit folder7 = new FolderUnit()
            {
                IsDeleted = false,
                Name = "Folder",
                Description = "Logs folder",
                CreatedAt = new DateTime(2016, 2, 6, 15, 33, 15),
                LastModified = new DateTime(2016, 2, 6, 15, 33, 15),
                Owner = user1,
                
                Space = space1,
                DataUnits = null
            };
            #endregion
            #region Content filling
            List<DataUnit> space1Content = new List<DataUnit>()
            {
                new FileUnit()
                    {
                        IsDeleted = false,
                        Name = "Current Task",
                        FileType = FileType.Document,
                        Description = "in process",
                        CreatedAt = new DateTime(2016, 5, 6, 18, 23, 45),
                        LastModified = new DateTime(2016, 5, 6, 18, 23, 45),
                        Owner = user2,
                        Link = "",
                        Space = space1
                        
                    },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "New Schema",
                    FileType = FileType.Document,
                    CreatedAt = new DateTime(2016, 5, 13, 13, 43, 15),
                    LastModified =new DateTime(2016, 5, 13, 13, 43, 15),
                    Owner = user3,
                    Link = "",
                    Space = space1
                }
            };
            List<DataUnit> folder1Content = new List<DataUnit>()
            {
                new FileUnit()
                    {
                        IsDeleted = false,
                        Name = "Binary Drive Spec",
                        FileType = FileType.Document,
                        Description = "File in binary space",
                        CreatedAt = new DateTime(2016, 4, 13, 11, 43, 15),
                        LastModified = new DateTime(2016, 4, 13, 11, 43, 15),
                        Owner = user1,
                        Link = "",
                        Space = space1
                    },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Data Model",
                    FileType = FileType.Sheets,
                    CreatedAt = new DateTime(2016, 4, 27, 18, 21, 36),
                    LastModified = new DateTime(2016, 4, 27, 18, 21, 36),
                    Owner = user1,
                    Link = "",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Some Task",
                    FileType = FileType.Document,
                    Description = "in process",
                    CreatedAt = new DateTime(2016, 6, 17, 12, 47, 16),
                    LastModified = new DateTime(2016, 6, 17, 12, 47, 16),
                    Owner = user1,
                    Link = "",
                    Space = space1
                }
            };

            List<DataUnit> folder2Content = new List<DataUnit>()
            {
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Other Links",
                    FileType = FileType.Link,
                    CreatedAt = new DateTime(2016, 7, 11, 8, 18, 26),
                    LastModified = new DateTime(2016, 7, 11, 8, 18, 26),
                    Owner = user1,
                    Link = "",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Some helpfull code",
                    FileType = FileType.Document,
                    Description = "in process",
                    CreatedAt = new DateTime(2016, 4, 10, 9, 45, 16),
                    LastModified = new DateTime(2016, 4, 10, 9, 45, 16),
                    Owner = user1,
                    Link = "",
                    Space = space1
                }
            };
            List<DataUnit> folder3Content = new List<DataUnit>()
            {
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Log1",
                    FileType = FileType.Document,
                    Description = "log file",
                    CreatedAt = new DateTime(2016, 3, 11, 9, 45, 16),
                    LastModified = new DateTime(2016, 3, 11, 9, 45, 16),
                    Owner = user1,
                    Link = "",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Log2",
                    FileType = FileType.Document,
                    Description = "log file",
                    CreatedAt = new DateTime(2016, 3, 14, 19, 25, 13),
                    LastModified = new DateTime(2016, 3, 14, 19, 25, 13),
                    Owner = user1,
                    Link = "",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Log3",
                    FileType = FileType.Document,
                    Description = "log file",
                    CreatedAt = new DateTime(2016, 3, 17, 12, 45, 26),
                    LastModified = new DateTime(2016, 3, 17, 12, 45, 26),
                    Owner = user1,
                    Link = "",
                    Space = space1
                }
            };
            List<DataUnit> folder4Content = new List<DataUnit>()
            {
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Lesson 1",
                    FileType = FileType.Slides,
                    Description = "presentation file",
                    CreatedAt = new DateTime(2016, 4, 11, 16, 15, 46),
                    LastModified = new DateTime(2016, 4, 11, 16, 15, 46),
                    Owner = user1,
                    Link = "",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Lesson 2",
                    FileType = FileType.Slides,
                    Description = "presentation file",
                    CreatedAt = new DateTime(2016, 4, 15, 11, 18, 46),
                    LastModified = new DateTime(2016, 4, 15, 11, 18, 46),
                    Owner = user1,
                    Link = "",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "AngularJS",
                    FileType = FileType.Slides,
                    Description = "presentation file",
                    CreatedAt = new DateTime(2016, 5, 16, 11, 18, 46),
                    LastModified = new DateTime(2016, 5, 16, 11, 18, 46),
                    Owner = user1,
                    Link = "",
                    Space = space1
                }
            };
            List<DataUnit> folder6Content = new List<DataUnit>()
            {
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Lesson 1 HomeTask",
                    FileType = FileType.Document,
                    Description = "presentation file",
                    CreatedAt = new DateTime(2016, 6, 12, 16, 28, 36),
                    LastModified = new DateTime(2016, 6, 12, 16, 28, 36),
                    Owner = user1,
                    Link = "",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Lesson 2 HomeTask",
                    FileType = FileType.Document,
                    Description = "presentation file",
                    CreatedAt = new DateTime(2016, 7, 22, 8, 18, 45),
                    LastModified = new DateTime(2016, 7, 22, 8, 18, 45),
                    Owner = user1,
                    Link = "",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "AngularJS HomeTask",
                    FileType = FileType.Document,
                    Description = "presentation file",
                    CreatedAt = new DateTime(2016, 5, 12, 13, 28, 15),
                    LastModified = new DateTime(2016, 5, 12, 13, 28, 15),
                    Owner = user1,
                    Link = "",
                    Space = space1
                }
            };
            List<DataUnit> folder5Content = new List<DataUnit>()
            {
                 new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Links",
                    FileType = FileType.Document,
                    Description = "presentation file",
                    CreatedAt = new DateTime(2016, 6, 24, 11, 34, 25),
                    LastModified = new DateTime(2016, 6, 24, 11, 34, 25),
                    Owner = user1,
                    Link = "",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Examples",
                    FileType = FileType.Document,
                    Description = "some helpfull examples",
                    CreatedAt = new DateTime(2016, 6, 14, 18, 14, 45),
                    LastModified = new DateTime(2016, 6, 14, 18, 14, 45),
                    Owner = user1,
                    Link = "",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "file_1",
                    FileType = FileType.Document,
                    Description = "general file",
                    CreatedAt = new DateTime(2016, 7, 24, 19, 24, 41),
                    LastModified = new DateTime(2016, 7, 24, 19, 24, 41),
                    Owner = user1,
                    Link = "",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "file_2",
                    FileType = FileType.Document,
                    Description = "general file",
                    CreatedAt = new DateTime(2016, 4, 14, 18, 37, 21),
                    LastModified = new DateTime(2016, 4, 14, 18, 37, 21),
                    Owner = user1,
                    Link = "",
                    Space = space1
                }
            };
            List<DataUnit> folder7Content = new List<DataUnit>()
            {
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "File 1",
                    FileType = FileType.Document,
                    CreatedAt = new DateTime(2016, 5, 24, 8, 17, 34),
                    LastModified = new DateTime(2016, 5, 24, 8, 17, 34),
                    Owner = user1,
                    Link = "",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "File 2",
                    FileType = FileType.Document,
                    Description = "general data",
                    CreatedAt = new DateTime(2016, 6, 14, 13, 57, 14),
                    LastModified = new DateTime(2016, 6, 14, 13, 57, 14),
                    Owner = user1,
                    Link = "",
                    Space = space1
                }
            };
            #endregion
            #region Content assign
            folder7.DataUnits = folder7Content;
            folder6.DataUnits = folder6Content;
            folder5.DataUnits = folder5Content;
            folder3.DataUnits = folder3Content;

            folder2Content.Add(folder7);
            folder2.DataUnits = folder2Content;

            folder4Content.Add(folder5);
            folder4Content.Add(folder6);
            folder4.DataUnits = folder4Content;

            folder1Content.Add(folder4);
            folder1.DataUnits = folder1Content;

            space1Content.Add(folder1);
            space1Content.Add(folder2);
            space1Content.Add(folder3);
            space1.ContentList = space1Content;
            #endregion
            #endregion

            #region My Space
            Space space2 = new Space()
            {
                Id = 2,
                Owner = user1,
                Name = "My Space",
                Type = SpaceType.MySpace,
                Description = "Private space for folders and files",
                MaxFileSize = 1073741824,
                MaxFilesQuantity = 1000,
                CreatedAt = new DateTime(2016, 1, 1, 9, 15, 33),
                LastModified = new DateTime(2016, 1, 1, 9, 15, 33),
                IsDeleted = false,
                ContentList = null
            };
            #region Folder Init
            FolderUnit folder8 = new FolderUnit()
            {
                IsDeleted = false,
                Name = "English",
                Description = "Books and exercises",
                CreatedAt = new DateTime(2016, 1, 17, 16, 45, 33),
                LastModified = new DateTime(2016, 1, 17, 16, 45, 33),
                Owner = user1,
                Space = space2,
                DataUnits = null
            };
            FolderUnit folder9 = new FolderUnit()
            {
                IsDeleted = false,
                Name = "Literature",
                Description = "Books for free time",
                CreatedAt = new DateTime(2016, 1, 14, 15, 25, 33),
                LastModified = new DateTime(2016, 1, 14, 15, 25, 33),
                Owner = user1,
                Space = space2,
                DataUnits = null
            };
            FolderUnit folder10 = new FolderUnit()
            {
                IsDeleted = false,
                Name = "Education",
                Description = "Helpfull materials for education",
                CreatedAt = new DateTime(2016, 1, 14, 11, 45, 33),
                LastModified = new DateTime(2016, 1, 14, 11, 45, 33),
                Owner = user1,
                Space = space2,
                DataUnits = null
            };
            FolderUnit folder11 = new FolderUnit()
            {
                IsDeleted = false,
                Name = "O'Reilly assembly",
                Description = "Many usefull files",
                CreatedAt = new DateTime(2016, 2, 11, 19, 15, 33),
                LastModified = new DateTime(2016, 2, 11, 19, 15, 33),
                Owner = user1,
                
                Space = space2,
                DataUnits = null
            };
            FolderUnit folder12 = new FolderUnit()
            {
                IsDeleted = false,
                Name = ".Net",
                Description = "Files and books for programming",
                CreatedAt = new DateTime(2016, 3, 18, 9, 15, 33),
                LastModified = new DateTime(2016, 3, 18, 9, 15, 33),
                Owner = user1,
                
                Space = space2,
                DataUnits = null
            };
            #endregion
            #region Content filling
            List<DataUnit> space2Content = new List<DataUnit>()
            {
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Notify",
                    FileType = FileType.Document,
                    CreatedAt = new DateTime(2016, 8, 18, 9, 15, 33),
                    LastModified = new DateTime(2016, 8, 18, 9, 15, 33),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "temp Work",
                    FileType = FileType.Document,
                    Description = "some code file",
                    CreatedAt = new DateTime(2016, 8, 15, 15, 7, 23),
                    LastModified = new DateTime(2016, 8, 15, 15, 7, 23),
                    Owner = user1,
                    Link = "",
                    Space = space2
                }
            };
            #region English folder
            List<DataUnit> folder8Content = new List<DataUnit>()
            {
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = " I Still Can’t Speak English",
                    FileType = FileType.Document,
                    CreatedAt = new DateTime(2016, 5, 1, 8, 30, 52),
                    LastModified = new DateTime(2016, 5, 1, 8, 30, 52),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Essential Grammar in Use with Answers",
                    FileType = FileType.Document,
                    Description = "Essential Grammar in Use with Answers and CD-ROM Pack",
                    CreatedAt = new DateTime(2016, 6, 11, 9, 22, 11),
                    LastModified = new DateTime(2016, 6, 11, 9, 22, 11),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Advanced Grammar in Use",
                    FileType = FileType.Document,
                    Description = "Advanced Grammar in Use Second edition is a fully updated version of the highly successful grammar title.",
                    CreatedAt = new DateTime(2016, 4, 12, 15, 12, 10),
                    LastModified = new DateTime(2016, 4, 12, 15, 12, 10),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Collins Cobuild Student's Grammar, Classroom Edition",
                    FileType = FileType.Document,
                    Description = "Classroom Edition",
                    CreatedAt = new DateTime(2016, 4, 10, 11, 12, 10),
                    LastModified = new DateTime(2016, 4, 10, 11, 12, 10),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Headway: Elementary Third Edition: Student's Book",
                    FileType = FileType.Document,
                    Description = "Six-level general English course for adults: Student's Book Elementary level ",
                    CreatedAt = new DateTime(2016, 5, 22, 16, 1, 30),
                    LastModified = new DateTime(2016, 5, 22, 16, 1, 30),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Headway Intermediate - Third Edition",
                    FileType = FileType.Document,
                    Description = "Student's Book Intermediate level",
                    CreatedAt = new DateTime(2016, 4, 20, 11, 33, 40),
                    LastModified = new DateTime(2016, 4, 20, 11, 33, 40),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Headway: Upper-Intermediate Third Edition",
                    FileType = FileType.Document,
                    Description = "Student's Book: Six-level general English course: Student's Book Upper-Intermediate",
                    CreatedAt = new DateTime(2016, 6, 10, 23, 41, 10),
                    LastModified = new DateTime(2016, 6, 10, 23, 41, 10),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "English Grammar in Use Book with Answers",
                    FileType = FileType.Document,
                    Description = "A Self-Study Reference and Practice Book for Intermediate Learners of English",
                    CreatedAt = new DateTime(2016, 7, 11, 14, 34, 50),
                    LastModified = new DateTime(2016, 7, 11, 14, 34, 50),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "English Vocabulary in Use Pre-Intermediate and Intermediate",
                    FileType = FileType.Document,
                    Description = "The second in the family of best-selling vocabulary reference and practice books from elementary to advanced level.",
                    CreatedAt = new DateTime(2016, 6, 4, 9, 45, 33),
                    LastModified = new DateTime(2016, 6, 4, 9, 45, 33),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Practical English Usage",
                    FileType = FileType.Document,
                    Description = "Practical English Usage is a major new reference guide for intermediate and advanced students of English",
                    CreatedAt = new DateTime(2016, 5, 9, 18, 11, 24),
                    LastModified = new DateTime(2016, 5, 9, 18, 11, 24),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "British or American English?: A Handbook of Word and Grammar Patterns",
                    FileType = FileType.Slides,
                    Description = "Speakers of British and American English display some striking differences in their use of grammar. ",
                    CreatedAt = new DateTime(2016, 7, 11, 14, 37, 20),
                    LastModified = new DateTime(2016, 7, 11, 14, 37, 20),
                    Owner = user1,
                    Link = "",
                    Space = space2
                }
            };
            #endregion
            #region Literature folder
            List<DataUnit> folder9Content = new List<DataUnit>()
            {
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Isaac Asimov — I, Robot",
                    FileType = FileType.Document,
                    Description = "",
                    CreatedAt = new DateTime(2016, 6, 21, 15, 7, 23),
                    LastModified = new DateTime(2016, 6, 21, 15, 7, 23),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Bram Stoker - Dracula",
                    FileType = FileType.Document,
                    Description = "",
                    CreatedAt = new DateTime(2016, 4, 21, 11, 34, 23),
                    LastModified = new DateTime(2016, 4, 21, 11, 34, 23),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Scott Fitzgerald - The Great Gatsby",
                    FileType = FileType.Document,
                    Description = "Scott Fitzgerald’s third book, stands as the supreme achievement of his career.",
                    CreatedAt = new DateTime(2016, 5, 2, 15, 24, 13),
                    LastModified = new DateTime(2016, 5, 2, 15, 24, 13),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = " Mark Twain - The Adventures of Huckleberry Finn",
                    FileType = FileType.Document,
                    CreatedAt = new DateTime(2016, 7, 12, 18, 24, 13),
                    LastModified = new DateTime(2016, 7, 12, 18, 24, 13),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Fyodor Dostoyevsky - Crime and Punishment",
                    FileType = FileType.Document,
                    Description = "Crime and Punishment put Dostoevsky at the forefront of Russian writers when it appeared in 1866 and is now one of the most famous and influential novels in world literature",
                    CreatedAt = new DateTime(2016, 5, 2, 17, 14, 33),
                    LastModified = new DateTime(2016, 5, 2, 17, 14, 33),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Oscar Wilde - The Picture of Dorian Gray",
                    FileType = FileType.Document,
                    Description = "Story of a fashionable young man who sells his soul for eternal youth and beauty is the author’s most popular work.",
                    CreatedAt =  new DateTime(2016, 4, 15, 18, 14, 43),
                    LastModified = new DateTime(2016, 4, 15, 18, 14, 43),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "William Shakespeare - Romeo and Juliet",
                    FileType = FileType.Document,
                    CreatedAt = new DateTime(2016, 4, 12, 19, 14, 43),
                    LastModified = new DateTime(2016, 4, 12, 19, 14, 43),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Leo Tolstoy - Anna Karenina",
                    FileType = FileType.Document,
                    Description = "Classic story of doomed love is one of the most admired novels in world literature.",
                    CreatedAt = new DateTime(2016, 5, 11, 17, 24, 13),
                    LastModified = new DateTime(2016, 5, 11, 17, 24, 13),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = " Mark Twain - The Adventures of Huckleberry Finn",
                    FileType = FileType.Document,
                    CreatedAt = new DateTime(2016, 4, 12, 15, 24, 13),
                    LastModified = new DateTime(2016, 4, 12, 15, 24, 13),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "William Shakespeare - Hamlet",
                    Description = "One of the greatest plays of all time, the compelling tragedy of the tormented young prince of Denmark",
                    FileType = FileType.Document,
                    CreatedAt = new DateTime(2016, 6, 10, 18, 24, 13),
                    LastModified = new DateTime(2016, 6, 10, 18, 24, 13),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = " Herman Melville - Moby-Dick; or, The Whale",
                    Description = "One of the greatest works of imaginations in literary history.",
                    FileType = FileType.Document,
                    CreatedAt = new DateTime(2016, 6, 15, 17, 24, 13),
                    LastModified = new DateTime(2016, 6, 15, 17, 24, 13),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "J.R.R. Tolkien - The Hobbit",
                    Description = "",
                    FileType = FileType.Document,
                    CreatedAt = new DateTime(2016, 6, 15, 19, 14, 13),
                    LastModified = new DateTime(2016, 6, 15, 19, 14, 13),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Yann Martel - Life of Pi",
                    Description = "Life of Pi is a fantasy adventure novel by Yann Martel published in 2001",
                    FileType = FileType.Document,
                    CreatedAt = new DateTime(2016, 3, 10, 11, 24, 13),
                    LastModified = new DateTime(2016, 3, 10, 11, 24, 13),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = " Mark Twain - The Adventures of Tom Sawyer",
                    Description = "First of Mark Twain's novels to feature one of the best-loved characters in American fiction",
                    FileType = FileType.Document,
                    CreatedAt = new DateTime(2016, 4, 13, 13, 24, 13),
                    LastModified = new DateTime(2016, 4, 13, 13, 24, 13),
                    Owner = user1,
                    Link = "",
                    Space = space2
                }
            };
            #endregion 
            #region Education folder
            List<DataUnit> folder10Content = new List<DataUnit>()
            {
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Tests",
                    FileType = FileType.Document,
                    CreatedAt = new DateTime(2016, 8, 15, 15, 7, 23),
                    LastModified = new DateTime(2016, 8, 15, 15, 7, 23),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Code",
                    FileType = FileType.Document,
                    Description = "some code file",
                    CreatedAt = new DateTime(2016, 8, 18, 9, 15, 33),
                    LastModified = new DateTime(2016, 8, 18, 9, 15, 33),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Presentation C# Best Practices",
                    FileType = FileType.Slides,
                    Description = "Usefull presentation file",
                    CreatedAt = new DateTime(2016, 6, 21, 15, 7, 23),
                    LastModified = new DateTime(2016, 6, 21, 15, 7, 23),
                    Owner = user1,
                    Link = "",
                    Space = space2
                }
            };

            List<DataUnit> folder11Content = new List<DataUnit>()
            {
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Jay Hilyard - C# 6.0 Cookbook",
                    FileType = FileType.Document,
                    Description = "Completely updated for C# 6.0, the new edition of this bestseller offers more than 150 code recipes to common and not-so-common problems that C# programmers face every day",
                    CreatedAt = new DateTime(2016, 5, 21, 15, 7, 23),
                    LastModified = new DateTime(2016, 5, 21, 15, 7, 23),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Christopher Schmitt - CSS Cookbook 3rd Edition",
                    FileType = FileType.Document,
                    Description = "Cascading Style Sheets (CSS) are a powerful way to enrich the presentation of HTML-based web pages, allowing web authors to give their pages a more sophisticated look and more structure.",
                    CreatedAt = new DateTime(2016, 5, 20, 10, 7, 23),
                    LastModified = new DateTime(2016, 5, 20, 10, 7, 23),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Eric Freeman - Head First Design Patterns",
                    FileType = FileType.Document,
                    Description = "",
                    CreatedAt = new DateTime(2016, 5, 10, 14, 17, 23),
                    LastModified = new DateTime(2016, 5, 10, 14, 17, 23),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Callum Macrae - Learning from jQuery",
                    FileType = FileType.Document,
                    Description = "",
                    CreatedAt = new DateTime(2016, 5, 29, 18, 8, 13),
                    LastModified = new DateTime(2016, 5, 29, 18, 8, 13),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Brad Green, Shyam Seshadri - AngularJS",
                    FileType = FileType.Document,
                    Description = "This hands-on guide introduces you to AngularJS, the open source JavaScript framework.",
                    CreatedAt = new DateTime(2016, 5, 24, 11, 11, 23),
                    LastModified = new DateTime(2016, 5, 24, 11, 11, 23),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Julia Lerman - Programming Entity Framework, 2nd Edition",
                    FileType = FileType.Document,
                    Description = "Building Data Centric Apps with the ADO.NET Entity Framework 4",
                    CreatedAt = new DateTime(2016, 5, 12, 16, 26, 13),
                    LastModified = new DateTime(2016, 5, 12, 16, 26, 13),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Mark Shufflebottom - Learning Bootstrap 3",
                    FileType = FileType.Document,
                    Description = "In this Bootstrap 3 training course, expert author Mark Shufflebottom shows you how to use this front end framework to design a website.",
                    CreatedAt = new DateTime(2016, 5, 28, 19, 33, 45),
                    LastModified = new DateTime(2016, 5, 28, 19, 33, 45),
                    Owner = user1,
                    Link = "",
                    Space = space2
                }
            };
            List<DataUnit> folder12Content = new List<DataUnit>()
            {
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "ANDREW TROELSEN - C# 6.0 and the .NET 4.6 Framework",
                    FileType = FileType.Document,
                    CreatedAt = new DateTime(2016, 3, 18, 12, 45, 15),
                    LastModified = new DateTime(2016, 3, 18, 12, 45, 15),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Mark Seemann - Dependency Injection in .NET",
                    FileType = FileType.Document,
                    Description = "Dependency Injection in .NET, winner of the 2013 Jolt Awards for Productivity, presents core DI patterns in plain C#",
                    CreatedAt = new DateTime(2016, 7, 3, 9, 25, 55),
                    LastModified = new DateTime(2016, 7, 3, 9, 25, 55),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Osherove - The Art of Unit Testing: with examples in C#",
                    FileType = FileType.Document,
                    Description = "The Art of Unit Testing, Second Edition guides you step by step from writing your first simple tests to developing robust test sets that are maintainable, readable, and trustworthy.",
                    CreatedAt = new DateTime(2016, 7, 12, 17, 26, 15),
                    LastModified = new DateTime(2016, 7, 12, 17, 26, 15),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Cleary - Concurrency in C# Cookbook ",
                    FileType = FileType.Document,
                    Description = "With more than 75 code-rich recipes, author Stephen Cleary demonstrates parallel processing and asynchronous programming techniques, using libraries and language features in .Net 4.5 and C# 5.0.",
                    CreatedAt = new DateTime(2016, 7, 22, 7, 46, 25),
                    LastModified = new DateTime(2016, 7, 22, 7, 46, 25),
                    Owner = user1,
                    Link = "",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Joseph Albahari, Ben Albahari - C# 6.0 Pocket Reference",
                    FileType = FileType.Document,
                    CreatedAt = new DateTime(2016, 4, 21, 14, 46, 25),
                    LastModified = new DateTime(2016, 4, 21, 14, 46, 25),
                    Owner = user1,
                    Link = "",
                    Space = space2
                }
            };
            #endregion
            #region Content assign
            folder12.DataUnits = folder12Content;
            folder11.DataUnits = folder11Content;

            folder10Content.Add(folder11);
            folder10Content.Add(folder12);
            folder10.DataUnits = folder10Content;
            folder9.DataUnits = folder9Content;
            folder8.DataUnits = folder8Content;

            space2Content.Add(folder10);
            space2Content.Add(folder9);
            space2Content.Add(folder8);

            space2.ContentList = space2Content;
            #endregion
            #endregion
            #endregion

            #region Roles
            List<User> users1 = new List<User>();
            users1.Add(user1);
            users1.Add(user2);

            List<User> users2 = new List<User>();
            users2.Add(user1);
            users2.Add(user3);

            Role role1 = new Role()
            {
                Id = 1,
                Name = "HR",
                Description = "Hi! We are HRs!",
                Users = users1
            };

            Role role2 = new Role()
            {
                Id = 2,
                Name = "Backend developer",
                Description = "Hi! We are backend developers!",
                Users = users2
            };
            context.Roles.Add(role1);
            context.Roles.Add(role2);
            #endregion
            context.Spaces.Add(space1);
            context.Spaces.Add(space2);
            base.Seed(context);
        }
    }
}