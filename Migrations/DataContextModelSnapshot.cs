﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebApi.Helpers;

namespace WebApi.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("AccountClass", b =>
                {
                    b.Property<int>("AccountsId")
                        .HasColumnType("integer");

                    b.Property<int>("ClassesId")
                        .HasColumnType("integer");

                    b.HasKey("AccountsId", "ClassesId");

                    b.HasIndex("ClassesId");

                    b.ToTable("AccountClass");
                });

            modelBuilder.Entity("CompetenceTag", b =>
                {
                    b.Property<int>("CompetencesId")
                        .HasColumnType("integer");

                    b.Property<int>("TagsId")
                        .HasColumnType("integer");

                    b.HasKey("CompetencesId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("CompetenceTag");
                });

            modelBuilder.Entity("CurriculumProcessType", b =>
                {
                    b.Property<int>("CurriculaId")
                        .HasColumnType("integer");

                    b.Property<int>("ProcessTypesId")
                        .HasColumnType("integer");

                    b.HasKey("CurriculaId", "ProcessTypesId");

                    b.HasIndex("ProcessTypesId");

                    b.ToTable("CurriculumProcessType");
                });

            modelBuilder.Entity("WebApi.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("AcceptTerms")
                        .HasColumnType("boolean");

                    b.Property<int>("CompetenceType")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<DateTime?>("PasswordReset")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ResetToken")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ResetTokenExpires")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("VerificationToken")
                        .HasColumnType("text");

                    b.Property<DateTime?>("Verified")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("WebApi.Entities.Class", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("CourseId")
                        .HasColumnType("integer");

                    b.Property<int>("CurriculumId")
                        .HasColumnType("integer");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("LocationId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.HasIndex("CurriculumId");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("LocationId");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("WebApi.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("CompetenceId")
                        .HasColumnType("integer");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Reviewer")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CompetenceId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("WebApi.Entities.Competence", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.Property<string>("Action")
                        .HasColumnType("text");

                    b.Property<string>("BasicKnowledge")
                        .HasColumnType("text");

                    b.Property<int>("CompetenceType")
                        .HasColumnType("integer");

                    b.Property<string>("Context")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("FinalResults")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("ProcessId")
                        .HasColumnType("integer");

                    b.Property<string>("SuccessCriteria")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ProcessId");

                    b.ToTable("Competences");
                });

            modelBuilder.Entity("WebApi.Entities.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("DepartmentId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("WebApi.Entities.Curriculum", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Curricula");
                });

            modelBuilder.Entity("WebApi.Entities.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("WebApi.Entities.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("WebApi.Entities.Process", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<int>("CurriculumId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("ProcessTypeId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CurriculumId");

                    b.HasIndex("ProcessTypeId");

                    b.ToTable("Processes");
                });

            modelBuilder.Entity("WebApi.Entities.ProcessType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("CourseId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("ProcessTypes");
                });

            modelBuilder.Entity("WebApi.Entities.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("CompetenceId")
                        .HasColumnType("integer");

                    b.Property<int>("Reviewer")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CompetenceId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("WebApi.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("VocableId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("VocableId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("WebApi.Entities.Vocable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Vocables");
                });

            modelBuilder.Entity("AccountClass", b =>
                {
                    b.HasOne("WebApi.Entities.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApi.Entities.Class", null)
                        .WithMany()
                        .HasForeignKey("ClassesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CompetenceTag", b =>
                {
                    b.HasOne("WebApi.Entities.Competence", null)
                        .WithMany()
                        .HasForeignKey("CompetencesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApi.Entities.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CurriculumProcessType", b =>
                {
                    b.HasOne("WebApi.Entities.Curriculum", null)
                        .WithMany()
                        .HasForeignKey("CurriculaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApi.Entities.ProcessType", null)
                        .WithMany()
                        .HasForeignKey("ProcessTypesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebApi.Entities.Account", b =>
                {
                    b.OwnsMany("WebApi.Entities.RefreshToken", "RefreshTokens", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                            b1.Property<int>("AccountId")
                                .HasColumnType("integer");

                            b1.Property<DateTime>("Created")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<string>("CreatedByIp")
                                .HasColumnType("text");

                            b1.Property<DateTime>("Expires")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<string>("ReplacedByToken")
                                .HasColumnType("text");

                            b1.Property<DateTime?>("Revoked")
                                .HasColumnType("timestamp without time zone");

                            b1.Property<string>("RevokedByIp")
                                .HasColumnType("text");

                            b1.Property<string>("Token")
                                .HasColumnType("text");

                            b1.HasKey("Id");

                            b1.HasIndex("AccountId");

                            b1.ToTable("RefreshToken");

                            b1.WithOwner("Account")
                                .HasForeignKey("AccountId");

                            b1.Navigation("Account");
                        });

                    b.Navigation("RefreshTokens");
                });

            modelBuilder.Entity("WebApi.Entities.Class", b =>
                {
                    b.HasOne("WebApi.Entities.Course", "Course")
                        .WithMany("Classes")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApi.Entities.Curriculum", "Curriculum")
                        .WithMany("Classes")
                        .HasForeignKey("CurriculumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApi.Entities.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApi.Entities.Location", "Location")
                        .WithMany("Classes")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Curriculum");

                    b.Navigation("Department");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("WebApi.Entities.Comment", b =>
                {
                    b.HasOne("WebApi.Entities.Competence", "Competence")
                        .WithMany()
                        .HasForeignKey("CompetenceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Competence");
                });

            modelBuilder.Entity("WebApi.Entities.Competence", b =>
                {
                    b.HasOne("WebApi.Entities.Account", "Account")
                        .WithMany("Competences")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApi.Entities.Process", "Process")
                        .WithMany()
                        .HasForeignKey("ProcessId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Process");
                });

            modelBuilder.Entity("WebApi.Entities.Course", b =>
                {
                    b.HasOne("WebApi.Entities.Department", "Department")
                        .WithMany("Courses")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("WebApi.Entities.Process", b =>
                {
                    b.HasOne("WebApi.Entities.Curriculum", "Curriculum")
                        .WithMany("Processes")
                        .HasForeignKey("CurriculumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebApi.Entities.ProcessType", "ProcessType")
                        .WithMany("Processes")
                        .HasForeignKey("ProcessTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Curriculum");

                    b.Navigation("ProcessType");
                });

            modelBuilder.Entity("WebApi.Entities.ProcessType", b =>
                {
                    b.HasOne("WebApi.Entities.Course", "Course")
                        .WithMany("ProcessTypes")
                        .HasForeignKey("CourseId");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("WebApi.Entities.Review", b =>
                {
                    b.HasOne("WebApi.Entities.Competence", "Competence")
                        .WithMany("Reviews")
                        .HasForeignKey("CompetenceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Competence");
                });

            modelBuilder.Entity("WebApi.Entities.Tag", b =>
                {
                    b.HasOne("WebApi.Entities.Vocable", "Vocable")
                        .WithMany("Tags")
                        .HasForeignKey("VocableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vocable");
                });

            modelBuilder.Entity("WebApi.Entities.Account", b =>
                {
                    b.Navigation("Competences");
                });

            modelBuilder.Entity("WebApi.Entities.Competence", b =>
                {
                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("WebApi.Entities.Course", b =>
                {
                    b.Navigation("Classes");

                    b.Navigation("ProcessTypes");
                });

            modelBuilder.Entity("WebApi.Entities.Curriculum", b =>
                {
                    b.Navigation("Classes");

                    b.Navigation("Processes");
                });

            modelBuilder.Entity("WebApi.Entities.Department", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("WebApi.Entities.Location", b =>
                {
                    b.Navigation("Classes");
                });

            modelBuilder.Entity("WebApi.Entities.ProcessType", b =>
                {
                    b.Navigation("Processes");
                });

            modelBuilder.Entity("WebApi.Entities.Vocable", b =>
                {
                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
