﻿

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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class theonepercentersEntities1 : DbContext
{
    public theonepercentersEntities1()
        : base("name=theonepercentersEntities1")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public DbSet<Company> Companies { get; set; }

    public DbSet<Competition> Competitions { get; set; }

    public DbSet<Contact> Contacts { get; set; }

    public DbSet<Membership> Memberships { get; set; }

    public DbSet<PrincipalUser> PrincipalUsers { get; set; }

    public DbSet<Setting> Settings { get; set; }

    public DbSet<UserCompetition> UserCompetitions { get; set; }

    public DbSet<Task> Tasks { get; set; }

    public DbSet<sysdiagram> sysdiagrams { get; set; }

    public DbSet<UserProfile> UserProfiles { get; set; }

    public DbSet<webpages_Membership> webpages_Membership { get; set; }

    public DbSet<webpages_OAuthMembership> webpages_OAuthMembership { get; set; }

    public DbSet<webpages_Roles> webpages_Roles { get; set; }

    public DbSet<Log4Net_Error> Log4Net_Error { get; set; }

    public DbSet<UserTask> UserTasks { get; set; }

}

}

