using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TodoMvc.Models
{
    public class Todo
    {
        [Key]
        public int TodoId { get; set; }

        [Column(name: "UserReference")]
        public int UserReference { get; set; }

        [Column(name: "Title")]
        public string Title { get; set; }

        [Column(name: "Description")]
        public string Description { get; set; }

        [Column(name: "IsDone")]
        public int IsDone { get; set; }

        internal object OrderByDescending(Func<object, object> p)
        {
            throw new NotImplementedException();
        }

        [Column(name: "DateCreated")]
        public DateTime DateCreated { get; set; }

        [Column(name: "DateUpdated")]
        public DateTime DateUpdated { get; set; }

        [Column(name: "Status")]
        public int Status { get; set; }
    }
}