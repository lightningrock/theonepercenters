
//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace TheOnePercents.Model
{

using System;
    using System.Collections.Generic;
    
public partial class UserTask
{

    public int UserTaskID { get; set; }

    public int TaskID { get; set; }

    public int UserID { get; set; }

    public int CompetitionID { get; set; }

    public System.DateTime Date { get; set; }

    public bool Completed { get; set; }



    public virtual webpages_Membership webpages_Membership { get; set; }

    public virtual Task Task { get; set; }

}

}
