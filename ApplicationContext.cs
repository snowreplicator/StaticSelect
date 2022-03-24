using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ru.zorro.static_select
{

    public class ApplicationContext : DbContext
    {
        private string dbConnectionString;

        public ApplicationContext(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseNpgsql(dbConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public")
                .HasAnnotation("Relational:Collation", "Russian_Russia.1251");

            modelBuilder.Entity<Classifier>(entity =>
            {
                entity.Property(e => e.ClassifierId).HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);
            });

            modelBuilder.Entity<Classifierset>(entity =>
            {
                entity.Property(e => e.ClassifiersetId).HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);
            });

            modelBuilder.Entity<LessonSource>(entity =>
            {
                entity.Property(e => e.LessonSourceId).HasDefaultValueSql("nextval('multi_d_cases.lessonsource_lessonsource_id_seq'::regclass)");
            });

            modelBuilder.Entity<Nodesubset>(entity =>
            {
                entity.Property(e => e.NodesubsetId).HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);
            });

            modelBuilder.HasSequence("classifier_classifier_id_seq", "classifiers")
                .StartsAt(100)
                .HasMax(2147483647);

            modelBuilder.HasSequence("classifierset_classifierset_id_seq", "classifiers")
                .StartsAt(100)
                .HasMax(2147483647);

            modelBuilder.HasSequence("lessonsource_lessonsource_id_seq", "multi_d_cases")
                .StartsAt(100)
                .HasMax(2147483647);

            modelBuilder.HasSequence("nodesubset_nodesubset_id_seq", "classifiers")
                .StartsAt(100)
                .HasMax(2147483647);

            //OnModelCreatingPartial(modelBuilder);
        }

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);


        public virtual DbSet<Classifier> Classifiers { get; set; }
        public virtual DbSet<Classifierset> Classifiersets { get; set; }
        public virtual DbSet<Nodesubset> Nodesubsets { get; set; }

        public virtual DbSet<LessonSource> LessonSources { get; set; }
        public virtual DbSet<OrganizationStructure> OrganizationStructures { get; set; }
        public virtual DbSet<DatatypeTests> DatatypeTests { get; set; }

    }
}
