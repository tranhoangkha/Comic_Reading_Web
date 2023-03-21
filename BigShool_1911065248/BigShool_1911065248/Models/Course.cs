namespace BigShool_1911065248.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Course")]
    public partial class Course
    {
        //public List<Category> ListCategory = new List<Category>();
        //public string Name;
        public string LectureName;
        public bool isLogin = false;
        public bool isShowGoing = false;
        public bool IsshowFollow = false;
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string LecturerId { get; set; }

        [Required]
        [StringLength(255)]
        public string Place { get; set; }

        public DateTime DateTime { get; set; }

        public int CategoryId { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public virtual Category Category { get; set; }

        public List<Category> listCategory = new List<Category>();
    }
}
