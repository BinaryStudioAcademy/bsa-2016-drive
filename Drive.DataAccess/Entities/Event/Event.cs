using Drive.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Drive.DataAccess.Entities.Event
{
    public class Event : IEntity
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string EventType { get; set; }
        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime EventDate { get; set; }

        public IList<EventContent> ContentList { get; set; }
        [Required]
        public FileUnit FileUnit { get; set; }

    }

    //public enum EventType
    //{
    //    None,
    //    Ceremonial,  // открытия, закрытия, вручения, приемы 
    //    Educational, // семинары, тренинги, круглые столы
    //    NetWorking,  // конференции, выставки, митапы
    //    Entertainment // тимбилдинги, праздники и другие развлечения
    //}
}
