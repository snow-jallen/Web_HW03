using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web_HW03.Models;

namespace Web_HW03.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PostTag>()
                .HasKey(boatyMcBoatface => new { boatyMcBoatface.PostId, boatyMcBoatface.TagId });

            builder.Entity<PostTag>()
                .HasOne(pt => pt.Post)
                    .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.PostId);
            
            builder.Entity<PostTag>()
                .HasOne(pt => pt.Tag)
                    .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.TagId);
        }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Web_HW03.Models.PostTag> PostTag { get; set; }
    }
}
