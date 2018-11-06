using ClubDomain.Classes.ClubModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCClubsWeek4.Models
{
    public class ClubCreateView
    {
        [Display(Name = "Club Name")]
        public string ClubName { get; set; }
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }
        [Required]
        [Display(Name = "Student ID")]
        [RegularExpression(@"^[S|s]\d{8}$", 
            ErrorMessage = "Student ID must start with an S and have 9 Characters in Total")]
        public string StudentID { get; set; }
        
    }
}