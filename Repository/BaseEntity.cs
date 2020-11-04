using System;

namespace Repository
{
    public class BaseEntity
    {
        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime LastSavedDate { get; set; }

        public Guid LastSavedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}