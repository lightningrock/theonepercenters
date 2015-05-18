using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;


namespace TheOnePercents.TestWeb.Models
{
    public class VideoContext : DbContext
    {
        public VideoContext()
            : base("theonepercentersEntities") 
        {
            
        }

        public DbSet<Video> Videos { get; set; }
    }

    public class Video
    {
        [Key]
        public int VideoID { get; set; }
        public string URL { get; set; }
        public DateTime PublishDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TimeSpan Length { get; set; }
        public string ThumbnailURL { get; set; }
    }
}