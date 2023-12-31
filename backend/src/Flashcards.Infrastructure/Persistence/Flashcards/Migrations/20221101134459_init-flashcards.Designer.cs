﻿// <auto-generated />
using System;
using Flashcards.Infrastructure.Persistence.Flashcards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Flashcards.Infrastructure.Persistence.Flashcards.Migrations
{
    [DbContext(typeof(FlashcardsDbContext))]
    [Migration("20221101134459_init-flashcards")]
    partial class initflashcards
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Flashcards.Domain.Entities.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<short>("Correctness")
                        .HasColumnType("smallint")
                        .HasColumnName("correctness");

                    b.Property<DateTime>("DateAdd")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_add");

                    b.Property<string>("Definition")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("definition");

                    b.Property<int>("SetId")
                        .HasColumnType("integer")
                        .HasColumnName("set_id");

                    b.Property<string>("Term")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("term");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("SetId");

                    b.ToTable("cards", "flashcards");
                });

            modelBuilder.Entity("Flashcards.Domain.Entities.Set", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateAdd")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date_add");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.ToTable("sets", "flashcards");
                });

            modelBuilder.Entity("Flashcards.Domain.Entities.Card", b =>
                {
                    b.HasOne("Flashcards.Domain.Entities.Set", "Set")
                        .WithMany("Cards")
                        .HasForeignKey("SetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Set");
                });

            modelBuilder.Entity("Flashcards.Domain.Entities.Set", b =>
                {
                    b.Navigation("Cards");
                });
#pragma warning restore 612, 618
        }
    }
}
