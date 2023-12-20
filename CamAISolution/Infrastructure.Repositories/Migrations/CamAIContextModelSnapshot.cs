﻿// <auto-generated />
using System;
using Infrastructure.Repositories.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Repositories.Migrations
{
    [DbContext(typeof(CamAIContext))]
    partial class CamAIContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Core.Domain.Entities.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountStatusId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AddressLine")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly?>("Birthday")
                        .HasColumnType("date");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("GenderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<Guid?>("WardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("WorkingShopId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AccountStatusId");

                    b.HasIndex("GenderId");

                    b.HasIndex("WardId");

                    b.HasIndex("WorkingShopId");

                    b.ToTable("Accounts");

                    b.HasData(
                        new
                        {
                            Id = new Guid("6a29f2b2-8383-400f-b2e4-55ed0d73baf2"),
                            AccountStatusId = new Guid("b9a32e8b-b87e-4d30-8b7e-2d0d95ba25d4"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "admin@camai.com",
                            ModifiedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Admin",
                            Password = "9eb622419ace52f259e858a7f2a10743d35e36fe0d22fc2d224c320cbc68d3af"
                        });
                });

            modelBuilder.Entity("Core.Domain.Entities.AccountRole", b =>
                {
                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AccountId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AccountRole");

                    b.HasData(
                        new
                        {
                            AccountId = new Guid("6a29f2b2-8383-400f-b2e4-55ed0d73baf2"),
                            RoleId = new Guid("445e1e75-a049-4080-b7b9-39f0230db10b")
                        });
                });

            modelBuilder.Entity("Core.Domain.Entities.AccountStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("AccountStatuses");

                    b.HasData(
                        new
                        {
                            Id = new Guid("7fe2231d-eff1-49a5-83a0-08b3d24e23c1"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ModifiedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "New"
                        },
                        new
                        {
                            Id = new Guid("b9a32e8b-b87e-4d30-8b7e-2d0d95ba25d4"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ModifiedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Active"
                        },
                        new
                        {
                            Id = new Guid("d6b279cf-9db7-48b4-a757-0e794721ffba"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ModifiedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Inactive"
                        });
                });

            modelBuilder.Entity("Core.Domain.Entities.Brand", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("BrandManagerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BrandStatusId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("BrandManagerId")
                        .IsUnique()
                        .HasFilter("[BrandManagerId] IS NOT NULL");

                    b.HasIndex("BrandStatusId");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("Core.Domain.Entities.BrandStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("BrandStatuses");

                    b.HasData(
                        new
                        {
                            Id = new Guid("fd8b6d47-8022-4c09-812f-b54057bd4609"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ModifiedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Active"
                        },
                        new
                        {
                            Id = new Guid("db3cd2ca-46a9-4ba6-b45f-495399fbe499"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ModifiedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Inactive"
                        });
                });

            modelBuilder.Entity("Core.Domain.Entities.District", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("ProvinceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("ProvinceId");

                    b.ToTable("Districts");
                });

            modelBuilder.Entity("Core.Domain.Entities.Gender", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("Genders");

                    b.HasData(
                        new
                        {
                            Id = new Guid("852936da-122b-4f14-9431-36968d57a30b"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ModifiedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Male"
                        },
                        new
                        {
                            Id = new Guid("ecc62be4-c48f-4d58-bfd5-14e4317352a3"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ModifiedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Female"
                        });
                });

            modelBuilder.Entity("Core.Domain.Entities.Province", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("Provinces");
                });

            modelBuilder.Entity("Core.Domain.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("445e1e75-a049-4080-b7b9-39f0230db10b"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ModifiedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Admin"
                        },
                        new
                        {
                            Id = new Guid("1f8005b4-a7e0-4829-94b0-55d58e79200f"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ModifiedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Technician"
                        },
                        new
                        {
                            Id = new Guid("10fc3a85-cb58-45a1-8f6e-e97859ea6a07"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ModifiedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Brand manager"
                        },
                        new
                        {
                            Id = new Guid("3950dc6f-d89b-42a1-b6de-cc14ec3755ef"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ModifiedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Shop manager"
                        },
                        new
                        {
                            Id = new Guid("e7e269b9-a273-4212-96f3-063c22cb513e"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ModifiedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Employee"
                        });
                });

            modelBuilder.Entity("Core.Domain.Entities.Shop", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AddressLine")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Phone")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("ShopManagerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ShopStatusId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<Guid>("WardId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ShopManagerId")
                        .IsUnique()
                        .HasFilter("[ShopManagerId] IS NOT NULL");

                    b.HasIndex("ShopStatusId");

                    b.HasIndex("WardId");

                    b.ToTable("Shops");
                });

            modelBuilder.Entity("Core.Domain.Entities.ShopStatus", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("ShopStatuses");

                    b.HasData(
                        new
                        {
                            Id = new Guid("7e0ef477-5f32-498b-b1e6-5cb5a79192a4"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ModifiedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Active"
                        },
                        new
                        {
                            Id = new Guid("d49dd0b3-a5c6-4b16-9527-2674578fc05b"),
                            CreatedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ModifiedDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Inactive"
                        });
                });

            modelBuilder.Entity("Core.Domain.Entities.Ward", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("DistrictId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("DistrictId");

                    b.ToTable("Wards");
                });

            modelBuilder.Entity("Core.Domain.Entities.Account", b =>
                {
                    b.HasOne("Core.Domain.Entities.AccountStatus", "AccountStatus")
                        .WithMany()
                        .HasForeignKey("AccountStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domain.Entities.Gender", "Gender")
                        .WithMany()
                        .HasForeignKey("GenderId");

                    b.HasOne("Core.Domain.Entities.Ward", "Ward")
                        .WithMany()
                        .HasForeignKey("WardId");

                    b.HasOne("Core.Domain.Entities.Shop", "WorkingShop")
                        .WithMany("Employees")
                        .HasForeignKey("WorkingShopId");

                    b.Navigation("AccountStatus");

                    b.Navigation("Gender");

                    b.Navigation("Ward");

                    b.Navigation("WorkingShop");
                });

            modelBuilder.Entity("Core.Domain.Entities.AccountRole", b =>
                {
                    b.HasOne("Core.Domain.Entities.Account", "Account")
                        .WithMany("AccountRoles")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domain.Entities.Role", "Role")
                        .WithMany("AccountRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Core.Domain.Entities.Brand", b =>
                {
                    b.HasOne("Core.Domain.Entities.Account", "BrandManager")
                        .WithOne("Brand")
                        .HasForeignKey("Core.Domain.Entities.Brand", "BrandManagerId");

                    b.HasOne("Core.Domain.Entities.BrandStatus", "BrandStatus")
                        .WithMany()
                        .HasForeignKey("BrandStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BrandManager");

                    b.Navigation("BrandStatus");
                });

            modelBuilder.Entity("Core.Domain.Entities.District", b =>
                {
                    b.HasOne("Core.Domain.Entities.Province", "Province")
                        .WithMany("Districts")
                        .HasForeignKey("ProvinceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Province");
                });

            modelBuilder.Entity("Core.Domain.Entities.Shop", b =>
                {
                    b.HasOne("Core.Domain.Entities.Account", "ShopManager")
                        .WithOne("ManagingShop")
                        .HasForeignKey("Core.Domain.Entities.Shop", "ShopManagerId");

                    b.HasOne("Core.Domain.Entities.ShopStatus", "ShopStatus")
                        .WithMany()
                        .HasForeignKey("ShopStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domain.Entities.Ward", "Ward")
                        .WithMany()
                        .HasForeignKey("WardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ShopManager");

                    b.Navigation("ShopStatus");

                    b.Navigation("Ward");
                });

            modelBuilder.Entity("Core.Domain.Entities.Ward", b =>
                {
                    b.HasOne("Core.Domain.Entities.District", "District")
                        .WithMany("Wards")
                        .HasForeignKey("DistrictId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("District");
                });

            modelBuilder.Entity("Core.Domain.Entities.Account", b =>
                {
                    b.Navigation("AccountRoles");

                    b.Navigation("Brand");

                    b.Navigation("ManagingShop");
                });

            modelBuilder.Entity("Core.Domain.Entities.District", b =>
                {
                    b.Navigation("Wards");
                });

            modelBuilder.Entity("Core.Domain.Entities.Province", b =>
                {
                    b.Navigation("Districts");
                });

            modelBuilder.Entity("Core.Domain.Entities.Role", b =>
                {
                    b.Navigation("AccountRoles");
                });

            modelBuilder.Entity("Core.Domain.Entities.Shop", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
