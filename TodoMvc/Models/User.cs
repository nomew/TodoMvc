using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoMvc.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Column(name: "Username")]
        public string Username { get; set; }

        [Column(name: "Password")]
        public string Password { get; set; }

        [Column(name: "FirstName")]
        public string FirstName { get; set; }

        [Column(name: "LastName")]
        public string LastName { get; set; }

        [Column(name: "Gender")]
        public int Gender { get; set; }

        [Column(name: "BirthDate")]
        public string BirthDate { get; set; }

        [Column(name: "DateCreated")]
        public DateTime DateCreated { get; set; }

        [Column(name: "DateUpdated")]
        public DateTime DateUpdated { get; set; }

        [Column(name: "Status")]
        public int Status { get; set; }
    }
}