using ServiceBook.Models.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceBook.Models.DatabaseModels
{
    public abstract class BaseEntity : IEntity<string>
    {
        public BaseEntity()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        [Required]
        public string Id { get; set; }
    }
}
