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
        public EventType EventType { get; set; }
        public DateTime EventDate { get; set; }

        public IList<EventContent> ContentLinks { get; set; }
        [Required]
        public FileUnit FileUnit { get; set; }

        public User Author { get; set; }

    }

    public enum EventType
    {
        None,
        Ceremonial,  // открытия, закрытия, вручения, приемы 
        Educational, // семинары, тренинги, круглые столы
        NetWorking,  // конференции, выставки, митапы
        Entertainment // тимбилдинги, праздники и другие развлечения
    }
}
