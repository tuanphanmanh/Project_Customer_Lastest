using Abp.IdentityServer4vNext;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using esign.Authorization.Delegation;
using esign.Authorization.Roles;
using esign.Authorization.Users;
using esign.Chat;
using esign.Editions;
using esign.Friendships;
using esign.MultiTenancy;
using esign.MultiTenancy.Accounting;
using esign.MultiTenancy.Payments;
using esign.Storage;
using esign.Master;
using esign.Esign;

namespace esign.EntityFrameworkCore
{
    public class esignDbContext : AbpZeroDbContext<Tenant, Role, User, esignDbContext>, IAbpPersistedGrantDbContext
    {
        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public virtual DbSet<RecentPassword> RecentPasswords { get; set; }

        // Esign Entity
        public virtual DbSet<EsignJobLog> EsignJobLogs { get; set; }
        public virtual DbSet<EsignComments> EsignComments { get; set; }
        public virtual DbSet<EsignDocumentList> EsignDocumentLists { get; set; }
        public virtual DbSet<EsignPosition> EsignPositions { get; set; }
        public virtual DbSet<EsignPrivateMessage> EsignPrivateMessages { get; set; }
        public virtual DbSet<EsignReminderHistory> EsignReminderHistorys { get; set; }
        public virtual DbSet<EsignRequest> EsignRequests { get; set; }
        public virtual DbSet<EsignRequestCategory> EsignRequestCategories { get; set; }
        public virtual DbSet<EsignSignerList> EsignSignerLists { get; set; }
        public virtual DbSet<EsignSignerNotification> EsignSignerNotifications { get; set; }
        public virtual DbSet<EsignSignerTemplateLink> EsignSignerTemplateLinks { get; set; }
        public virtual DbSet<EsignStatusSignerHistory> EsignStatusSignerHistories { get; set; }
        public virtual DbSet<EsignTransferSignerHistory> EsignTransferSignerHistories { get; set; }
        public virtual DbSet<EsignActivityHistory> EsignActivityHistories { get; set; }
        public virtual DbSet<EsignSignerNotificationDetail> EsignSignerNotificationDetails { get; set; }
        public virtual DbSet<MstEsignActiveDirectory> MstEsignActiveDirectories { get; set; }
        public virtual DbSet<MstEsignCategory> MstEsignCategories { get; set; }
        public virtual DbSet<MstEsignDepartment> MstEsignDepartments { get; set; }
        public virtual DbSet<MstEsignDivision> MstEsignDivisions { get; set; }
        public virtual DbSet<MstEsignSignerTemplate> MstEsignSignerTemplates { get; set; }
        public virtual DbSet<MstEsignStatus> MstEsignStatuss { get; set; }
        public virtual DbSet<MstEsignAccountOtherSystem> MstEsignAccountOtherSystem { get; set; }
        public virtual DbSet<MstEsignSystems> MstEsignSystems { get; set; }
        public virtual DbSet<MstEsignUserImage> MstEsignUserImages { get; set; }
        public virtual DbSet<MstActivityHistory> MstActivityHistories { get; set; }
        public virtual DbSet<MstEsignColor> MstEsignColors { get; set; }
        public virtual DbSet<EsignSignature> EsignSignature { get; set; }
        public virtual DbSet<MstEsignConfig> MstEsignConfigs { get; set; }
        public virtual DbSet<MstActivityHistory> MstActivityHistorys { get; set; }
        public virtual DbSet<EsignCommentsHistory> EsignCommentsHistories { get; set; }
        public virtual DbSet<EsignFollowUp> EsignFollowUps { get; set; }
        public virtual DbSet<EsignFollowUpHistory> EsignFollowUpHistories { get; set; }
        public virtual DbSet<EsignSignerSearchHistory> EsignSignerSearchHistories { get; set; }
        public virtual DbSet<EsignKeywordSearchHistory> EsignKeywordSearchHistories { get; set; }
        public virtual DbSet<EsignApiOtherSystem> EsignApiOtherSystems { get; set; }
        public virtual DbSet<MstEsignLogo> MstEsignLogos { get; set; }
        public virtual DbSet<EsignUserDevice> EsignUserDevices { get; set; }
        public virtual DbSet<EsignFCMDeviceToken> EsignFCMDeviceTokens { get; set; }
        public virtual DbSet<MstEsignEmailTemplate> MstEsignEmailTemplates { get; set; }

        public virtual DbSet<MstEsignHrTitles> MstInvHrTitless { get; set; }

        public virtual DbSet<MstEsignHrPosition> MstInvHrPositions { get; set; }

        public virtual DbSet<MstEsignHrOrgStructure> MstInvHrOrgStructures { get; set; }
        public virtual DbSet<MstEsignAffiliate> MstEsignAffiliate { get; set; }
        public virtual DbSet<EsignLogSign> EsignLogSigns { get; set; }
        public virtual DbSet<EsignCfgEmailAndNotiTemplate> EsignCfgEmailAndNotiTemplates { get; set; }
        public virtual DbSet<EsignReadStatus> EsignReadStatus { get; set; }
        public virtual DbSet<EsignVersionApp> EsignVersionApps { get; set; }
        public virtual DbSet<EsignReferenceRequest> EsignReferenceRequests { get; set; }
        public virtual DbSet<EsignMultiAffiliateAction> EsignMultiAffiliateAction { get; set; }

        public esignDbContext(DbContextOptions<esignDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique()
                    .HasFilter("[IsDeleted] = 0");
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}
