#Drive

The main goal of the system is storing all useful information of the company. There are several different types of files which associate with suitable applications for viewing information:
- Academy Pro - show information about Binary Academy Pro courses, lectures, videos, slides, useful links etc. Only granted users can create files of this type.
- Events - show information of all events which were in the company (Events, Conferences, Meetups). It can contain text, photos, links. (Maybe feedbacks)
- Docs, Sheets, Slides, Trello - links to remote services like google and trello. It should have extra information like description.
- Images - links to remote images.
- Video - links to youtube.com, vimeo.com and other.
- Links - links to remote resource with name.
- File - physical files. System should limit size and count of files.
- Folder - containers for folder and other files.

All files should be stored in spaces. Space is the container which created by user. Owner can add users to his own space or be added to another space. By default there are two spaces: company space and own space. Every user should be able to create space and add other users to it. Every user should have access to company’s space. Owner should be able to add permission to users for his own space (read, edit). Space should have a bit functionality for filtering, viewing, creating, removing and sharing files.
Every user should be able to create folder or any file (except Academy Pro and Employees). User should be able to share folder or file to other users by creating shared link. Before creating file user should choose type of file and then add minimal info for creation. Each type of files should have own application for CRUD operations. In menu user should be able to search or filter files by spaces or types.

Aside from spaces, there should be grouping by applications. The page should contain all document by selected type which user can read or edit. There also should be searching and other functionality like sharing, removing etc.
