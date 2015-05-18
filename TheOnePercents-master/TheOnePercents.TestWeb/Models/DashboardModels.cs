using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace TheOnePercents.TestWeb.Models
{

    public class DashboardContext : DbContext
    {
        public DashboardContext()
            : base("theonepercentersEntities") 
        {
            
        }
        public DbSet<UserTasks> UserTasks { get; set; }
        public DbSet<Task> Task { get; set; }
    }

    public class Task
    {
        [Key]
        public int TaskID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public bool Active { get; set; }
        public bool Snake { get; set; }

        //public virtual ICollection<UserTasks> UserTasks { get; set; }

        public Task()
        {
            //UserTasks = new List<UserTasks>();
        }
    }

    public class UserTasks
    {
        [Key]
        public int UserTaskID { get; set; }

        [Required]
        public int TaskRefID { get; set; }

        [ForeignKey("TaskRefID")]
        public virtual Task Task { get; set; }

        public int UserID { get; set; }

        [Required]
        public bool TaskCompleted { get; set; }
        
        public DateTime TaskDate { get; set; }
    }
}
