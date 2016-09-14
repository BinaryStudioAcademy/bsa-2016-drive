using System;
using System.Collections.Generic;
using System.Data.Entity;
using Drive.DataAccess.Entities;
using Drive.DataAccess.Entities.Event;
using Drive.DataAccess.Entities.Pro;

namespace Drive.DataAccess.Context
{
    public class DriveDBInitializer : DropCreateDatabaseIfModelChanges<DriveContext>
    {
        protected override void Seed(DriveContext context)
        {
            User user1 = new User()
            {
                Id = 1,
                GlobalId = "577a16659829fe050adb3f5c",
                IsDeleted = false
            };

            User user2 = new User()
            {
                Id = 2,
                GlobalId = "577a171c9829fe050adb3f5d",
                IsDeleted = false
            };

            User user3 = new User()
            {
                Id = 3,
                GlobalId = "577a17669829fe050adb3f5e",
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
                MaxFileSize = 100,
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
                        Name = "Class diagram",
                        FileType = FileType.Document,
                        Description = "Class diagram for Drive",
                        CreatedAt = new DateTime(2016, 5, 6, 18, 23, 45),
                        LastModified = new DateTime(2016, 5, 6, 18, 23, 45),
                        Owner = user2,
                        Link = "https://drive.google.com/file/d/0B3JV038xc3jlSEhiMXg0VmphRUE/view?usp=sharing",
                        Space = space1

                    },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Drive API",
                    FileType = FileType.Document,
                    Description = "Drive API for Drive",
                    CreatedAt = new DateTime(2016, 5, 13, 13, 43, 15),
                    LastModified =new DateTime(2016, 5, 13, 13, 43, 15),
                    Owner = user3,
                    Link = "https://github.com/BinaryStudioAcademy/bsa-2016-drive/blob/develop/documentation/Drive_API_v1.2.0.yaml",
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
                    Link = "https://docs.google.com/document/d/1F_BRxuLWEsqU4VMu_VhQFwN_qXMjdIeH0VFXWfeWW1s/edit",
                    Space = space1
                    },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Data Model",
                    FileType = FileType.Sheets,
                    Description = "Drive data model",
                    CreatedAt = new DateTime(2016, 4, 27, 18, 21, 36),
                    LastModified = new DateTime(2016, 4, 27, 18, 21, 36),
                    Owner = user1,
                    Link = "https://docs.google.com/document/d/1RTIUOfE6oxjWkcGK02bg29TKFNPFZOMEOTNhx3baREs/edit",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "NLog",
                    FileType = FileType.Document,
                    Description = "NLog documentation for Drive",
                    CreatedAt = new DateTime(2016, 6, 17, 12, 47, 16),
                    LastModified = new DateTime(2016, 6, 17, 12, 47, 16),
                    Owner = user1,
                    Link = "https://docs.google.com/document/d/13xWywdVyLEhSFbDFmzxAamjU5chE90TGy6MdEBYM5uI/edit",
                    Space = space1
                }
            };

            List<DataUnit> folder2Content = new List<DataUnit>()
            {
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Angular",
                    FileType = FileType.Links,
                    Description = "Part 1",
                    CreatedAt = new DateTime(2016, 7, 11, 8, 18, 26),
                    LastModified = new DateTime(2016, 7, 11, 8, 18, 26),
                    Owner = user1,
                    Link = "https://www.youtube.com/watch?v=N6JJWT9f9Ew",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "ASP.NET Routing",
                    FileType = FileType.Links,
                    Description = "Routing",
                    CreatedAt = new DateTime(2016, 4, 10, 9, 45, 16),
                    LastModified = new DateTime(2016, 4, 10, 9, 45, 16),
                    Owner = user1,
                    Link = "https://msdn.microsoft.com/en-us/library/cc668201.aspx",
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
                    Description = "Drive log file",
                    CreatedAt = new DateTime(2016, 3, 11, 9, 45, 16),
                    LastModified = new DateTime(2016, 3, 11, 9, 45, 16),
                    Owner = user1,
                    Link = "https://github.com/BinaryStudioAcademy/bsa-2016-drive/blob/develop/Drive.WebHost/Logs/fulllog.txt",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Log2",
                    FileType = FileType.Document,
                    Description = "Drive log file",
                    CreatedAt = new DateTime(2016, 3, 14, 19, 25, 13),
                    LastModified = new DateTime(2016, 3, 14, 19, 25, 13),
                    Owner = user1,
                    Link = "https://github.com/BinaryStudioAcademy/bsa-2016-drive/blob/e423670c219e83a5c4702c7c6b25bbcd3acbce15/Drive.WebHost/Logs/fulllog.txt",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Log3",
                    FileType = FileType.Document,
                    Description = "Drive log file",
                    CreatedAt = new DateTime(2016, 3, 17, 12, 45, 26),
                    LastModified = new DateTime(2016, 3, 17, 12, 45, 26),
                    Owner = user1,
                    Link = "https://github.com/BinaryStudioAcademy/bsa-2016-drive/blob/3fd3d5ba2ef01b6d2e930dcfbb3861e424500d24/Drive.WebHost/Logs/fulllog.txt",
                    Space = space1
                }
            };
            List<DataUnit> folder4Content = new List<DataUnit>()
            {
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Angular 2",
                    FileType = FileType.Slides,
                    Description = "presentation file",
                    CreatedAt = new DateTime(2016, 4, 11, 16, 15, 46),
                    LastModified = new DateTime(2016, 4, 11, 16, 15, 46),
                    Owner = user1,
                    Link = "http://www.slideshare.net/NigamGoyal/angular-js-2-63907052",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Angular",
                    FileType = FileType.Slides,
                    Description = "Presentation file",
                    CreatedAt = new DateTime(2016, 4, 15, 11, 18, 46),
                    LastModified = new DateTime(2016, 4, 15, 11, 18, 46),
                    Owner = user1,
                    Link = "http://mattiash.github.io/angular-presentation/presentation.html#1",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Introduction to Angularjs",
                    FileType = FileType.Slides,
                    Description = "Presentation file for introduction to Angularjs",
                    CreatedAt = new DateTime(2016, 5, 16, 11, 18, 46),
                    LastModified = new DateTime(2016, 5, 16, 11, 18, 46),
                    Owner = user1,
                    Link = "http://www.slideshare.net/manishekhawat/angularjs-22960631",
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
                    Description = "C# HomeTask 1",
                    CreatedAt = new DateTime(2016, 6, 12, 16, 28, 36),
                    LastModified = new DateTime(2016, 6, 12, 16, 28, 36),
                    Owner = user1,
                    Link = "http://simbelyne.blogspot.com/2013/05/c-lesson-1-introduction-to-programming.html",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Lesson 2 HomeTask",
                    FileType = FileType.Document,
                    Description = "C# HomeTask 2",
                    CreatedAt = new DateTime(2016, 7, 22, 8, 18, 45),
                    LastModified = new DateTime(2016, 7, 22, 8, 18, 45),
                    Owner = user1,
                    Link = "http://simbelyne.blogspot.com/2013/05/c-lesson-2-primitive-data-types-and.html",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "AngularJS HomeTask",
                    FileType = FileType.Document,
                    Description = "Build a Todo App with Angular JS HomeTask",
                    CreatedAt = new DateTime(2016, 5, 12, 13, 28, 15),
                    LastModified = new DateTime(2016, 5, 12, 13, 28, 15),
                    Owner = user1,
                    Link = "https://ilovecoding.org/lessons/build-a-todo-app-with-angular-js",
                    Space = space1
                }
            };
            List<DataUnit> folder5Content = new List<DataUnit>()
            {
                 new FileUnit()
                {
                    IsDeleted = false,
                    Name = "ASP.NET MVC 6 Full Tutorial",
                    FileType = FileType.Links,
                    Description = "Tutorial",
                    CreatedAt = new DateTime(2016, 6, 24, 11, 34, 25),
                    LastModified = new DateTime(2016, 6, 24, 11, 34, 25),
                    Owner = user1,
                    Link = "https://www.youtube.com/watch?v=4EGDxkWoUOY",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "AngularJS Examples",
                    FileType = FileType.Document,
                    Description = "Some helpfull examples",
                    CreatedAt = new DateTime(2016, 6, 14, 18, 14, 45),
                    LastModified = new DateTime(2016, 6, 14, 18, 14, 45),
                    Owner = user1,
                    Link = "http://fastandfluid.com/publicdownloads/AngularJSIn60MinutesIsh_DanWahlin_May2013.pdf",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Professional ASP.NET MVC 5.pdf",
                    FileType = FileType.Document,
                    Description = "Book",
                    CreatedAt = new DateTime(2016, 7, 24, 19, 24, 41),
                    LastModified = new DateTime(2016, 7, 24, 19, 24, 41),
                    Owner = user1,
                    Link = "http://www.cs.unsyiah.ac.id/~frdaus/PenelusuranInformasi/File-Pdf/Professional%20ASP.NET%20MVC%205.pdf",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Creating Mobile Apps with Xamarin.Forms",
                    FileType = FileType.Document,
                    Description = "Book",
                    CreatedAt = new DateTime(2016, 4, 14, 18, 37, 21),
                    LastModified = new DateTime(2016, 4, 14, 18, 37, 21),
                    Owner = user1,
                    Link = "http://aka.ms/xamebook",
                    Space = space1
                }
            };
            List<DataUnit> folder7Content = new List<DataUnit>()
            {
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Introducing Microsoft SQL Server 2014",
                    FileType = FileType.Document,
                    Description = "File",
                    CreatedAt = new DateTime(2016, 5, 24, 8, 17, 34),
                    LastModified = new DateTime(2016, 5, 24, 8, 17, 34),
                    Owner = user1,
                    Link = "http://aka.ms/684751pdf",
                    Space = space1
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "AngularJs tutorial",
                    FileType = FileType.Document,
                    Description = "This tutorial teaches you basics of AngularJS and its programming concepts",
                    CreatedAt = new DateTime(2016, 6, 14, 13, 57, 14),
                    LastModified = new DateTime(2016, 6, 14, 13, 57, 14),
                    Owner = user1,
                    Link = "https://www.tutorialspoint.com/angularjs/angularjs_tutorial.pdf",
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
                MaxFileSize = 100,
                MaxFilesQuantity = 1000,
                CreatedAt = new DateTime(2016, 1, 1, 9, 15, 33),
                LastModified = new DateTime(2016, 1, 1, 9, 15, 33),
                IsDeleted = false,
                ContentList = null
            };

            Space space3 = new Space()
            {
                Id = 3,
                Owner = user2,
                Name = "My Space",
                Type = SpaceType.MySpace,
                Description = "Private space for folders and files",
                MaxFileSize = 100,
                MaxFilesQuantity = 1000,
                CreatedAt = new DateTime(2016, 1, 1, 9, 15, 33),
                LastModified = new DateTime(2016, 1, 1, 9, 15, 33),
                IsDeleted = false,
                ContentList = null
            };

            Space space4 = new Space()
            {
                Id = 4,
                Owner = user3,
                Name = "My Space",
                Type = SpaceType.MySpace,
                Description = "Private space for folders and files",
                MaxFileSize = 100,
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
                    Name = "Dependency injection",
                    Description = "Specification for Drive",
                    FileType = FileType.Document,
                    CreatedAt = new DateTime(2016, 8, 18, 9, 15, 33),
                    LastModified = new DateTime(2016, 8, 18, 9, 15, 33),
                    Owner = user1,
                    Link = "https://docs.google.com/document/d/141uIJFq-XDGXE6l_E7pRO4MA_BTOVMkk6KXbN-Yw3ig/edit",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "UnitOfWork",
                    FileType = FileType.Document,
                    Description = "UnitOfWork specification for Drive",
                    CreatedAt = new DateTime(2016, 8, 15, 15, 7, 23),
                    LastModified = new DateTime(2016, 8, 15, 15, 7, 23),
                    Owner = user1,
                    Link = "https://docs.google.com/document/d/1-JUEdHh2ZsIonOgeY8QEcAYFBPFsSi64DQ3hBjiwAbQ/edit",
                    Space = space2
                },
                 new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Mockups",
                    FileType = FileType.Images,
                    Description = "Mockups  for Drive",
                    CreatedAt = new DateTime(2016, 8, 15, 15, 7, 23),
                    LastModified = new DateTime(2016, 8, 15, 15, 7, 23),
                    Owner = user1,
                    Link = "https://ninjamock.com/s/3WSXD",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "DriveBoard",
                    FileType = FileType.Trello,
                    Description = "Trello for Drive team",
                    CreatedAt = new DateTime(2016, 8, 15, 15, 7, 23),
                    LastModified = new DateTime(2016, 8, 15, 15, 7, 23),
                    Owner = user1,
                    Link = "https://trello.com/b/FaDwdwiM/driveboard",
                    Space = space2
                }
            };
            #region English folder
            List<DataUnit> folder8Content = new List<DataUnit>()
            {
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "I Still Can’t Speak English",
                    FileType = FileType.Document,
                    Description = "Make Your Own Free Social Learning, Real Practice English Course and Finally Speak English Comfortably",
                    CreatedAt = new DateTime(2016, 5, 1, 8, 30, 52),
                    LastModified = new DateTime(2016, 5, 1, 8, 30, 52),
                    Owner = user1,
                    Link = "https://www.amazon.com/Still-Cant-Speak-English-Comfortably-ebook/dp/B014QI07AE",
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
                    Link = "https://www.amazon.com/Essential-Grammar-Answers-CD-ROM-Pack/dp/052167543X",
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
                    Link = "https://www.amazon.com/Advanced-Grammar-Answers-Martin-Hewings/dp/0521532914",
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
                    Link = "https://www.amazon.com/Collins-COBUILD-Students-Grammar-Classroom/dp/0003705641",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Headway: Elementary Third Edition: Student's Book",
                    FileType = FileType.Document,
                    Description = "Six-level general English course for adults: Student's Book Elementary level",
                    CreatedAt = new DateTime(2016, 5, 22, 16, 1, 30),
                    LastModified = new DateTime(2016, 5, 22, 16, 1, 30),
                    Owner = user1,
                    Link = "https://www.amazon.com/New-Headway-Elementary-Six-level-Paperback/dp/B00IIB78I0",
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
                    Link = "https://www.amazon.co.uk/New-Headway-Intermediate-Third-Students/dp/019438750X",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Headway: Upper-Intermediate Third Edition",
                    FileType = FileType.Document,
                    Description = "Student's Workbook CDs Upper-intermediate level",
                    CreatedAt = new DateTime(2016, 6, 10, 23, 41, 10),
                    LastModified = new DateTime(2016, 6, 10, 23, 41, 10),
                    Owner = user1,
                    Link = "https://www.amazon.co.uk/New-Headway-Upper-Intermediate-Students-Upper-intermediate/dp/0194393097",
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
                    Link = "https://www.amazon.com/English-Grammar-Use-Answers-CD-ROM/dp/052118939X",
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
                    Link = "https://www.amazon.com/English-Vocabulary-Pre-Intermediate-Intermediate-CD-ROM/dp/0521614651",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Practical English Usage",
                    FileType = FileType.Document,
                    Description = "This unique reference guide addresses problem points in the language as encountered by learners and their teachers",
                    CreatedAt = new DateTime(2016, 5, 9, 18, 11, 24),
                    LastModified = new DateTime(2016, 5, 9, 18, 11, 24),
                    Owner = user1,
                    Link = "https://www.amazon.com/Practical-English-Usage-Michael-Swan/dp/0194420981",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "British or American English?: A Handbook of Word and Grammar Patterns",
                    FileType = FileType.Document,
                    Description = "Speakers of British and American English display some striking differences in their use of grammar. ",
                    CreatedAt = new DateTime(2016, 7, 11, 14, 37, 20),
                    LastModified = new DateTime(2016, 7, 11, 14, 37, 20),
                    Owner = user1,
                    Link = "https://www.amazon.com/British-American-English-Handbook-Patterns/dp/0521379938",
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
                    Name = "Isaac Asimov's I, Robot: To Protect",
                    FileType = FileType.Document,
                    Description = "First in an all-new trilogy inspired by Isaac Asimov's legendary science fiction collection I, Robot",
                    CreatedAt = new DateTime(2016, 6, 21, 15, 7, 23),
                    LastModified = new DateTime(2016, 6, 21, 15, 7, 23),
                    Owner = user1,
                    Link = "https://www.amazon.com/Isaac-Asimovs-I-Robot-Protect/dp/0451464893",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Bram Stoker - Dracula",
                    FileType = FileType.Document,
                    Description = "This is the Complete Unabridged Collectors Edition of Dracula, the 1897 classic horror novel by Irish author Bram Stoker, featuring as its primary antagonist the vampire Count Dracula",
                    CreatedAt = new DateTime(2016, 4, 21, 11, 34, 23),
                    LastModified = new DateTime(2016, 4, 21, 11, 34, 23),
                    Owner = user1,
                    Link = "https://www.amazon.com/Dracula-Bram-Stoker/dp/1936828154",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Scott Fitzgerald - The Great Gatsby",
                    FileType = FileType.Document,
                    Description = "Scott Fitzgerald’s third book, stands as the supreme achievement of his career",
                    CreatedAt = new DateTime(2016, 5, 2, 15, 24, 13),
                    LastModified = new DateTime(2016, 5, 2, 15, 24, 13),
                    Owner = user1,
                    Link = "https://www.amazon.com/Great-Gatsby-F-Scott-Fitzgerald/dp/0743273567",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Mark Twain - The Adventures of Huckleberry Finn",
                    FileType = FileType.Document,
                    Description = "The Newsouth Edition",
                    CreatedAt = new DateTime(2016, 7, 12, 18, 24, 13),
                    LastModified = new DateTime(2016, 7, 12, 18, 24, 13),
                    Owner = user1,
                    Link = "https://www.amazon.com/Mark-Twains-Adventures-Huckleberry-Finn/dp/1603062351",
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
                    Link = "https://www.amazon.com/Crime-Punishment-Fyodor-Dostoyevsky/dp/0486415872",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "The Picture of Dorian Gray (Dover Thrift Editions)",
                    FileType = FileType.Document,
                    Description = "Story of a fashionable young man who sells his soul for eternal youth and beauty is the author’s most popular work.",
                    CreatedAt =  new DateTime(2016, 4, 15, 18, 14, 43),
                    LastModified = new DateTime(2016, 4, 15, 18, 14, 43),
                    Owner = user1,
                    Link = "https://www.amazon.com/Picture-Dorian-Dover-Thrift-Editions/dp/0486278077",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "William Shakespeare - Romeo and Juliet",
                    FileType = FileType.Document,
                    Description = "One of Shakespeare's most popular and accessible plays, Romeo and Juliet tells the story of two star-crossed lovers and the unhappy fate that befell them as a result of a long and bitter feud between their families",
                    CreatedAt = new DateTime(2016, 4, 12, 19, 14, 43),
                    LastModified = new DateTime(2016, 4, 12, 19, 14, 43),
                    Owner = user1,
                    Link = "https://www.amazon.com/Romeo-Juliet-Dover-Thrift-Editions/dp/0486275574",
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
                    Link = "https://www.amazon.com/Anna-Karenina-Bantam-Classics-Tolstoy/dp/0553213466",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Mark Twain - The Adventures of Huckleberry Finn",
                    FileType = FileType.Document,
                    Description = "1st Edition",
                    CreatedAt = new DateTime(2016, 4, 12, 15, 24, 13),
                    LastModified = new DateTime(2016, 4, 12, 15, 24, 13),
                    Owner = user1,
                    Link = "https://www.amazon.com/Adventures-Huckleberry-Finn-Mark-Twain/dp/0486280616",
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
                    Link = "https://www.amazon.com/Hamlet-Folger-Library-Shakespeare-William/dp/074347712X",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Herman Melville - Moby-Dick; or, The Whale",
                    Description = "One of the greatest works of imaginations in literary history.",
                    FileType = FileType.Document,
                    CreatedAt = new DateTime(2016, 6, 15, 17, 24, 13),
                    LastModified = new DateTime(2016, 6, 15, 17, 24, 13),
                    Owner = user1,
                    Link = "https://www.amazon.com/Moby-Dick-Herman-Melville/dp/1503280780",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "J.R.R. Tolkien - The Hobbit",
                    Description = "A great modern classic and the prelude to THE LORD OF THE RINGS",
                    FileType = FileType.Document,
                    CreatedAt = new DateTime(2016, 6, 15, 19, 14, 13),
                    LastModified = new DateTime(2016, 6, 15, 19, 14, 13),
                    Owner = user1,
                    Link = "https://www.amazon.com/Hobbit-J-R-Tolkien/dp/054792822X",
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
                    Link = "https://www.amazon.com/Life-Pi-Yann-Martel/dp/0156027321",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Mark Twain - The Adventures of Tom Sawyer",
                    Description = "First of Mark Twain's novels to feature one of the best-loved characters in American fiction",
                    FileType = FileType.Document,
                    CreatedAt = new DateTime(2016, 4, 13, 13, 24, 13),
                    LastModified = new DateTime(2016, 4, 13, 13, 24, 13),
                    Owner = user1,
                    Link = "https://www.amazon.com/Adventures-Tom-Sawyer-Amazon-Classics/dp/0996584838",
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
                    Name = "C# Test",
                    FileType = FileType.Links,
                    Description = "This C# Test simulates a real online certification exams",
                    CreatedAt = new DateTime(2016, 8, 15, 15, 7, 23),
                    LastModified = new DateTime(2016, 8, 15, 15, 7, 23),
                    Owner = user1,
                    Link = "http://www.tutorialspoint.com/csharp/csharp_online_test.htm",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Nature",
                    FileType = FileType.Images,
                    Description = "Sample image",
                    CreatedAt = new DateTime(2016, 8, 15, 15, 7, 23),
                    LastModified = new DateTime(2016, 8, 15, 15, 7, 23),
                    Owner = user1,
                    Link = "https://flic.kr/p/9kNyZ4",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "City",
                    FileType = FileType.Images,
                    Description = "Sample image",
                    CreatedAt = new DateTime(2016, 8, 15, 15, 7, 23),
                    LastModified = new DateTime(2016, 8, 15, 15, 7, 23),
                    Owner = user1,
                    Link = "https://flic.kr/p/ek58Tx",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Angular 1 Style Guide",
                    FileType = FileType.Document,
                    Description = "Angular style guide for teams",
                    CreatedAt = new DateTime(2016, 8, 18, 9, 15, 33),
                    LastModified = new DateTime(2016, 8, 18, 9, 15, 33),
                    Owner = user1,
                    Link = "https://github.com/johnpapa/angular-styleguide/blob/master/a1/README.md",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "C# coding standards, good programming principles & refactoring",
                    FileType = FileType.Slides,
                    Description = "Usefull presentation file",
                    CreatedAt = new DateTime(2016, 6, 21, 15, 7, 23),
                    LastModified = new DateTime(2016, 6, 21, 15, 7, 23),
                    Owner = user1,
                    Link = "http://www.slideshare.net/EyobLube/c-coding-standards-good-programming-principles-refactoring",
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
                    Link = "https://www.amazon.com/C-6-0-Cookbook-Jay-Hilyard-ebook/dp/B015YOJS6I",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Christopher Schmitt - CSS Cookbook 3rd Edition",
                    FileType = FileType.Document,
                    Description = "Learn how to solve the real problems you face with CSS. This cookbook offers hundreds of practical examples for using CSS to format your web pages, and includes code samples you can use right away.",
                    CreatedAt = new DateTime(2016, 5, 20, 10, 7, 23),
                    LastModified = new DateTime(2016, 5, 20, 10, 7, 23),
                    Owner = user1,
                    Link = "https://www.amazon.com/CSS-Cookbook-3rd-Animal-Guide/dp/059615593X",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Eric Freeman - Head First Design Patterns",
                    FileType = FileType.Document,
                    Description = "This edition of Head First Design Patterns—now updated for Java 8—shows you the tried-and-true, road-tested patterns used by developers to create functional, elegant, reusable, and flexible software.",
                    CreatedAt = new DateTime(2016, 5, 10, 14, 17, 23),
                    LastModified = new DateTime(2016, 5, 10, 14, 17, 23),
                    Owner = user1,
                    Link = "https://www.amazon.co.uk/Head-First-Design-Patterns-Freeman/dp/0596007124",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Callum Macrae - Learning from jQuery",
                    FileType = FileType.Document,
                    Description = "If you're comfortable with jQuery but a bit shaky with JavaScript, this concise guide will help you expand your knowledge of the language-especially the code that jQuery covers up for you",
                    CreatedAt = new DateTime(2016, 5, 29, 18, 8, 13),
                    LastModified = new DateTime(2016, 5, 29, 18, 8, 13),
                    Owner = user1,
                    Link = "https://www.amazon.com/Learning-jQuery-Callum-Macrae/dp/1449335195",
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
                    Link = "http://shop.oreilly.com/product/0636920028055.do",
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
                    Link = "https://www.amazon.com/Programming-Entity-Framework-second-Text/dp/B004PLG1OO",
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
                    Link = "http://shop.oreilly.com/product/110000503.do",
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
                    Description = "This new 7th edition of Pro C# 6.0 and the .NET 4.6 Platform has been completely revised and rewritten to reflect the latest changes to the C# language specification and new advances in the .NET Framework.",
                    CreatedAt = new DateTime(2016, 3, 18, 12, 45, 15),
                    LastModified = new DateTime(2016, 3, 18, 12, 45, 15),
                    Owner = user1,
                    Link = "https://www.amazon.com/C-6-0-NET-4-6-Framework-ebook/dp/B015XFLAF0",
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
                    Link = "https://www.amazon.com/Dependency-Injection-NET-Mark-Seemann/dp/1935182501",
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
                    Link = "https://www.amazon.com/Art-Unit-Testing-examples/dp/1617290890",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = " Stephen Cleary - Concurrency in C# Cookbook",
                    FileType = FileType.Document,
                    Description = "With more than 75 code-rich recipes, author Stephen Cleary demonstrates parallel processing and asynchronous programming techniques, using libraries and language features in .Net 4.5 and C# 5.0.",
                    CreatedAt = new DateTime(2016, 7, 22, 7, 46, 25),
                    LastModified = new DateTime(2016, 7, 22, 7, 46, 25),
                    Owner = user1,
                    Link = "https://www.amazon.com/Concurrency-C-Cookbook-Stephen-Cleary/dp/1449367569",
                    Space = space2
                },
                new FileUnit()
                {
                    IsDeleted = false,
                    Name = "Joseph Albahari, Ben Albahari - C# 6.0 Pocket Reference",
                    FileType = FileType.Document,
                    Description = "When you need answers for programming with C# 6.0, this practical and tightly focused book tells you exactly what you need to know—without long introductions or bloated samples",
                    CreatedAt = new DateTime(2016, 4, 21, 14, 46, 25),
                    LastModified = new DateTime(2016, 4, 21, 14, 46, 25),
                    Owner = user1,
                    Link = "https://www.amazon.com/6-0-Pocket-Reference-Instant-Programmers/dp/1491927410",
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
            context.Spaces.Add(space3);
            context.Spaces.Add(space4);

            #region Academy Pro

            var tag1 = new Tag { Name = "Tag 1" };
            var tag2 = new Tag { Name = "Tag 2" };
            var tag3 = new Tag { Name = "Tag 3" };

            context.Tags.Add(tag1);
            context.Tags.Add(tag2);
            context.Tags.Add(tag3);

            var lecture1 = new Lecture
            {
                Name = "Lecture 1",
                Author = user1,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                StartDate = DateTime.Now.AddDays(5),
                Description = "Lecture desc"
            };

            lecture1.ContentList = new List<ContentLink>
            {
                new ContentLink
                {
                    LinkType = LinkType.Video,
                    Name = "Video sample 1",
                    Description = "Video sample description",
                    IsDeleted = false,
                    Link = "https://www.youtube.com/watch?v=QH2-TGUlwu4",
                    Lecture = lecture1
                },
                new ContentLink
                {
                    LinkType = LinkType.Video,
                    Name = "Video sample 2",
                    Description = "Video sample description",
                    IsDeleted = false,
                    Link = "https://www.youtube.com/watch?v=QH2-TGUlwu4",
                    Lecture = lecture1
                },
                new ContentLink
                {
                    LinkType = LinkType.Repository,
                    Name = "Repository sample 1",
                    Description = "Repository sample description",
                    IsDeleted = false,
                    Link = "https://github.com/BinaryStudioAcademy/bsa-2016-drive",
                    Lecture = lecture1
                },
                new ContentLink
                {
                    LinkType = LinkType.Slide,
                    Name = "Slide sample 1",
                    Description = "Slide sample description",
                    IsDeleted = false,
                    Link = "https://docs.google.com/presentation/d/1ViNfYiQCHhJvX5O0SvZ9EJgjI9FqjHZ7OQwFCE4KGlA/embed?start=false&loop=false&delayms=3000",
                    Lecture = lecture1
                }
            };

            var lecture2 = new Lecture
            {
                Name = "Lecture 2",
                Author = user1,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                StartDate = DateTime.Now.AddDays(10),
                Description = "Lecture desc"
            };

            lecture2.ContentList = new List<ContentLink>
            {
                new ContentLink
                {
                    LinkType = LinkType.Video,
                    Name = "Video sample 1",
                    Description = "Video sample description",
                    IsDeleted = false,
                    Link = "https://www.youtube.com/watch?v=QH2-TGUlwu4",
                    Lecture = lecture2
                },
                new ContentLink
                {
                    LinkType = LinkType.Video,
                    Name = "Video sample 2",
                    Description = "Video sample description",
                    IsDeleted = false,
                    Link = "https://www.youtube.com/watch?v=QH2-TGUlwu4",
                    Lecture = lecture2
                },
                new ContentLink
                {
                    LinkType = LinkType.Repository,
                    Name = "Repository sample 1",
                    Description = "Repository sample description",
                    IsDeleted = false,
                    Link = "https://github.com/BinaryStudioAcademy/bsa-2016-drive",
                    Lecture = lecture2
                },
                new ContentLink
                {
                    LinkType = LinkType.Slide,
                    Name = "Slide sample 1",
                    Description = "Slide sample description",
                    IsDeleted = false,
                    Link = "https://docs.google.com/presentation/d/1ViNfYiQCHhJvX5O0SvZ9EJgjI9FqjHZ7OQwFCE4KGlA/embed?start=false&loop=false&delayms=3000",
                    Lecture = lecture2
                }
            };

            var academy1 = new AcademyProCourse
            {
                FileUnit = new FileUnit
                {
                    Owner = user1,
                    FileType = FileType.AcademyPro,
                    Name = "Course 1",
                    Description =
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris sed tempus quam. Ut lobortis, mauris sed aliquam placerat, lectus libero venenatis metus, vitae mattis risus sem et ex. Etiam sed dictum dui. Vestibulum id nisl maximus sem auctor consequat.",
                    CreatedAt = DateTime.Now,
                    LastModified = DateTime.Now,
                    IsDeleted = false,
                    Space = space1
                },
                StartDate = DateTime.Now.AddDays(5),
                Lectures = new List<Lecture>
                {
                    lecture1,
                    lecture2
                },
                IsDeleted = false,
                Tags = new List<Tag> { tag1, tag2 },
                Author = user1
            };

            var lecture3 = new Lecture
            {
                Name = "Lecture 1",
                Author = user1,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                StartDate = DateTime.Now.AddDays(5),
                Description = "Lecture desc"
            };

            lecture3.ContentList = new List<ContentLink>
            {
                new ContentLink
                {
                    LinkType = LinkType.Video,
                    Name = "Video sample 1",
                    Description = "Video sample description",
                    IsDeleted = false,
                    Link = "https://www.youtube.com/watch?v=QH2-TGUlwu4",
                    Lecture = lecture3
                },
                new ContentLink
                {
                    LinkType = LinkType.Video,
                    Name = "Video sample 2",
                    Description = "Video sample description",
                    IsDeleted = false,
                    Link = "https://www.youtube.com/watch?v=QH2-TGUlwu4",
                    Lecture = lecture3
                },
                new ContentLink
                {
                    LinkType = LinkType.Repository,
                    Name = "Repository sample 1",
                    Description = "Repository sample description",
                    IsDeleted = false,
                    Link = "https://github.com/BinaryStudioAcademy/bsa-2016-drive",
                    Lecture = lecture3
                },
                new ContentLink
                {
                    LinkType = LinkType.Slide,
                    Name = "Slide sample 1",
                    Description = "Slide sample description",
                    IsDeleted = false,
                    Link = "https://docs.google.com/presentation/d/1ViNfYiQCHhJvX5O0SvZ9EJgjI9FqjHZ7OQwFCE4KGlA/embed?start=false&loop=false&delayms=3000",
                    Lecture = lecture3
                }
            };

            var lecture4 = new Lecture
            {
                Name = "Lecture 2",
                Author = user1,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                StartDate = DateTime.Now.AddDays(10),
                Description = "Lecture desc"
            };

            lecture4.ContentList = new List<ContentLink>
            {
                new ContentLink
                {
                    LinkType = LinkType.Video,
                    Name = "Video sample 1",
                    Description = "Repository sample description",
                    IsDeleted = false,
                    Link = "https://www.youtube.com/watch?v=QH2-TGUlwu4",
                    Lecture = lecture4
                },
                new ContentLink
                {
                    LinkType = LinkType.Video,
                    Name = "Video sample 2",
                    Description = "Video sample description",
                    IsDeleted = false,
                    Link = "https://www.youtube.com/watch?v=QH2-TGUlwu4",
                    Lecture = lecture4
                },
                new ContentLink
                {
                    LinkType = LinkType.Repository,
                    Name = "Repository sample 1",
                    Description = "Repository sample description",
                    IsDeleted = false,
                    Link = "https://github.com/BinaryStudioAcademy/bsa-2016-drive",
                    Lecture = lecture4
                },
                new ContentLink
                {
                    LinkType = LinkType.Slide,
                    Name = "Slide sample 1",
                    Description = "Slide sample description",
                    IsDeleted = false,
                    Link = "https://docs.google.com/presentation/d/1ViNfYiQCHhJvX5O0SvZ9EJgjI9FqjHZ7OQwFCE4KGlA/embed?start=false&loop=false&delayms=3000",
                    Lecture = lecture4
                }
            };

            var academy2 = new AcademyProCourse
            {
                FileUnit = new FileUnit
                {
                    Owner = user1,
                    FileType = FileType.AcademyPro,
                    Name = "Course 2",
                    Description =
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris sed tempus quam. Ut lobortis, mauris sed aliquam placerat, lectus libero venenatis metus, vitae mattis risus sem et ex. Etiam sed dictum dui. Vestibulum id nisl maximus sem auctor consequat.",
                    CreatedAt = DateTime.Now,
                    LastModified = DateTime.Now,
                    IsDeleted = false,
                    Space = space1
                },
                StartDate = DateTime.Now.AddDays(5),
                Lectures = new List<Lecture>
                {
                    lecture3,
                    lecture4
                },
                IsDeleted = false,
                Tags = new List<Tag> { tag2, tag3 },
                Author = user1
            };

            context.AcademyProCourses.Add(academy1);
            context.AcademyProCourses.Add(academy2);
            #endregion

            var ev = new Event
            {
                FileUnit = new FileUnit
                {
                    Owner = user1,
                    FileType = FileType.Events,
                    Name = "Event 1",
                    Description =
                        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris sed tempus quam. Ut lobortis, mauris sed aliquam placerat, lectus libero venenatis metus, vitae mattis risus sem et ex. Etiam sed dictum dui. Vestibulum id nisl maximus sem auctor consequat.",
                    CreatedAt = DateTime.Now,
                    LastModified = DateTime.Now,
                    IsDeleted = false,
                    Space = space1
                },
                Author = user1,
                EventDate = DateTime.Now,
                EventType = EventType.Entertainment,
                IsDeleted = false
            };

            var content = new List<EventContent>
            {
                new EventContent
                {
                    Event = ev,
                    IsDeleted = false,
                    ContentType = ContentType.Text,
                    CreatedAt = DateTime.Now,
                    LastModified = DateTime.Now,
                    Order = 1,
                    Name = "Event Content 1",
                    Description = "Event content description",
                    Content = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut fermentum tempus libero, eu rutrum metus facilisis et. Ut eu quam euismod risus malesuada efficitur vel vitae nulla. Pellentesque facilisis sapien erat, ut porttitor tortor ornare sit amet. Maecenas commodo erat eget nisi ullamcorper facilisis. Donec convallis rutrum nisi id dapibus. Phasellus mauris nibh, commodo ut porttitor sed, iaculis tincidunt lacus. Nullam eget dolor diam. Aliquam erat volutpat
Duis ante erat, sagittis id sollicitudin sed, facilisis et enim. Aliquam molestie, quam a faucibus faucibus, dui dui elementum urna, quis bibendum ipsum erat non nisl. Aliquam ac justo tristique, dignissim orci sed, vulputate ligula. Sed cursus quis nibh non cursus. Phasellus ac lectus ut felis porttitor bibendum et at lorem. Curabitur scelerisque ligula et ipsum maximus viverra. Ut varius suscipit odio, vitae malesuada neque volutpat id. Morbi gravida lorem pharetra aliquam ornare. Mauris tellus mi, sollicitudin eget porta ac, dictum eget enim."
                },
                new EventContent
                {
                    Event = ev,
                    IsDeleted = false,
                    ContentType = ContentType.Photo,
                    CreatedAt = DateTime.Now,
                    LastModified = DateTime.Now,
                    Order = 2,
                    Name = "Noooooooooooooooo!",
                    Description = "I am your father, Luke!",
                    Content = "http://assets2.ignimgs.com/2015/08/06/darth-vader-crossed-arms-1280jpg-88461e1280wjpg-67c0c2_1280w.jpg"
                },
                new EventContent
                {
                    Event = ev,
                    IsDeleted = false,
                    ContentType = ContentType.Link,
                    CreatedAt = DateTime.Now,
                    LastModified = DateTime.Now,
                    Order = 3,
                    Name = "Event Content 3",
                    Description = "Event content description",
                    Content = "https://youtube.com"
                },
                new EventContent
                {
                    Event = ev,
                    IsDeleted = false,
                    ContentType = ContentType.Video,
                    CreatedAt = DateTime.Now,
                    LastModified = DateTime.Now,
                    Order = 4,
                    Name = "Event Content 4",
                    Description = "Event content description",
                    Content = "https://www.youtube.com/watch?v=LowVhCfLm68"
                },
                new EventContent
                {
                    Event = ev,
                    IsDeleted = false,
                    ContentType = ContentType.Text,
                    CreatedAt = DateTime.Now,
                    LastModified = DateTime.Now,
                    Order = 5,
                    Name = "Event Content 5",
                    Description = "Event content description",
                    Content = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut fermentum tempus libero, eu rutrum metus facilisis et. Ut eu quam euismod risus malesuada efficitur vel vitae nulla. Pellentesque facilisis sapien erat, ut porttitor tortor ornare sit amet. Maecenas commodo erat eget nisi ullamcorper facilisis. Donec convallis rutrum nisi id dapibus. Phasellus mauris nibh, commodo ut porttitor sed, iaculis tincidunt lacus. Nullam eget dolor diam. Aliquam erat volutpat
Duis ante erat, sagittis id sollicitudin sed, facilisis et enim. Aliquam molestie, quam a faucibus faucibus, dui dui elementum urna, quis bibendum ipsum erat non nisl. Aliquam ac justo tristique, dignissim orci sed, vulputate ligula. Sed cursus quis nibh non cursus. Phasellus ac lectus ut felis porttitor bibendum et at lorem. Curabitur scelerisque ligula et ipsum maximus viverra. Ut varius suscipit odio, vitae malesuada neque volutpat id. Morbi gravida lorem pharetra aliquam ornare. Mauris tellus mi, sollicitudin eget porta ac, dictum eget enim."
                },
            };

            ev.ContentLinks = content;

            context.Events.Add(ev);

            base.Seed(context);
        }
    }
}