using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdditionalProject.Models
{
    public class Student
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string AlbumNumber { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [ForeignKey("ClassGroup")]
        public int GroupID { get; set; }
        public ClassGroup ClassGroup { get; set; }
        public bool IsAbsent { get; set; }
        
        public Student()
        {

        }
        public Student(string albumNumber, string firstName, string lastName, int groupId, bool isAbsent)
        {
            AlbumNumber = albumNumber;
            FirstName = firstName;
            LastName = lastName;
            GroupID = groupId;
            IsAbsent = isAbsent;
        }
    }
}
