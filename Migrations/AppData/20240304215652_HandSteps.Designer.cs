﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PostgreSQL.Data;

#nullable disable

namespace ai_poker_coach.Migrations.AppData
{
    [DbContext(typeof(AppDataContext))]
    [Migration("20240304215652_HandSteps")]
    partial class HandSteps
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ai_poker_coach.Data.Action", b =>
                {
                    b.Property<int>("ActionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ActionId"));

                    b.Property<int>("Bet")
                        .HasColumnType("integer");

                    b.Property<int>("Decision")
                        .HasColumnType("integer");

                    b.Property<int>("HandId")
                        .HasColumnType("integer");

                    b.Property<int>("Player")
                        .HasColumnType("integer");

                    b.Property<int>("Step")
                        .HasColumnType("integer");

                    b.HasKey("ActionId");

                    b.HasIndex("HandId");

                    b.ToTable("Actions");
                });

            modelBuilder.Entity("ai_poker_coach.Data.Card", b =>
                {
                    b.Property<int>("CardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CardId"));

                    b.Property<int>("HandId")
                        .HasColumnType("integer");

                    b.Property<int>("Player")
                        .HasColumnType("integer");

                    b.Property<int>("Step")
                        .HasColumnType("integer");

                    b.Property<string>("Suit")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("CardId");

                    b.HasIndex("HandId");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("ai_poker_coach.Data.Hand", b =>
                {
                    b.Property<int>("HandId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("HandId"));

                    b.Property<string>("Analysis")
                        .HasColumnType("text");

                    b.Property<int>("Ante")
                        .HasColumnType("integer");

                    b.Property<int>("BigBlind")
                        .HasColumnType("integer");

                    b.Property<int>("BigBlindAnte")
                        .HasColumnType("integer");

                    b.Property<int>("GameStyle")
                        .HasColumnType("integer");

                    b.Property<int>("MyStack")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("PlayerCount")
                        .HasColumnType("integer");

                    b.Property<string>("PlayerNotes")
                        .HasColumnType("text");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.Property<int>("SmallBlind")
                        .HasColumnType("integer");

                    b.Property<string>("Winners")
                        .HasColumnType("text");

                    b.HasKey("HandId");

                    b.ToTable("Hands");
                });

            modelBuilder.Entity("ai_poker_coach.Data.Action", b =>
                {
                    b.HasOne("ai_poker_coach.Data.Hand", "Hand")
                        .WithMany("Actions")
                        .HasForeignKey("HandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hand");
                });

            modelBuilder.Entity("ai_poker_coach.Data.Card", b =>
                {
                    b.HasOne("ai_poker_coach.Data.Hand", "Hand")
                        .WithMany("Cards")
                        .HasForeignKey("HandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hand");
                });

            modelBuilder.Entity("ai_poker_coach.Data.Hand", b =>
                {
                    b.Navigation("Actions");

                    b.Navigation("Cards");
                });
#pragma warning restore 612, 618
        }
    }
}
